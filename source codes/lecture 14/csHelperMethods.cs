using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using HtmlAgilityPack;
using System.Security.Cryptography;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using Newtonsoft.Json;
using System.Reflection;
using System.Threading;

namespace lecture_13
{
    public static class csHelperMethods
    {
        public static int irCrawledUrlCount = 0;
        public static int irDiscoveredUrlCount = 0;

        public static void clearDatabase()
        {
            using (var context = new DBCrawling())
            {
                var ctx = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext;
                ctx.ExecuteStoreCommand("truncate table tblMainUrls");
            }
        }

        public class crawlingResult : tblMainUrl
        {
            public crawlingResult()
            {
                this.LastCrawlingDate = new DateTime(1900, 1, 1);
                this.IsCrawled = false;
                this.CompressionPercent = 0;
                this.FetchTimeMS = 0;
                this.LinkDepthLevel = 0;
                this.PageTile = null;
                this.SourceCode = null;
                this.Url = "";
                this.UrlHash = "";
                this.DiscoverDate = DateTime.Now;
                this.ParentUrlHash = "";
                this.CrawlTryCounter = 0;
            }

            public bool blcrawlSuccess = true;
            public List<string> lstDiscoveredLinks = new List<string>();
        }

        public static void crawlPage(string srUrlToCrawl, int irUrlDepthLevel, string _srParentUrl, DateTime _dtDiscoverDate)
        {
            var vrLocalUrl = srUrlToCrawl;
            crawlingResult crawlResult = new crawlingResult();
            crawlResult.Url = vrLocalUrl;
            if (!string.IsNullOrEmpty(_srParentUrl))
                crawlResult.ParentUrlHash = _srParentUrl;
            if (_dtDiscoverDate != DateTime.MinValue)
                crawlResult.DiscoverDate = _dtDiscoverDate;

            Stopwatch swTimerCrawling = new Stopwatch();
            swTimerCrawling.Start();

            HtmlWeb wbClient = new HtmlWeb();//you should use httpwebrequest for more control and better performance
            wbClient.AutoDetectEncoding = true;
            wbClient.BrowserTimeout = new TimeSpan(0, 2, 0);
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            try
            {
                doc = wbClient.Load(crawlResult.Url);
                crawlResult.SourceCode = doc.Text;
            }
            catch (Exception E)
            {
                crawlResult.blcrawlSuccess = false;
                logError(E, "crawlPage");
            }

            Interlocked.Increment(ref irCrawledUrlCount);

            swTimerCrawling.Stop();
            crawlResult.FetchTimeMS = Convert.ToInt32(swTimerCrawling.ElapsedMilliseconds);
            crawlResult.LastCrawlingDate = DateTime.Now;
            saveCrawlInDatabase(crawlResult);

            if (crawlResult.blcrawlSuccess)
            {
                extractLinks(crawlResult, doc);
                saveDiscoveredLinksInDatabaseForFutureCrawling(crawlResult);
            }

            doc = null;

        }

        private static object _lockDatabaseAdd = new object();

        private static void saveDiscoveredLinksInDatabaseForFutureCrawling(crawlingResult crawlResult)
        {
            lock (_lockDatabaseAdd)
            {
                using (var context = new DBCrawling())
                {
                    HashSet<string> hsProcessedUrls = new HashSet<string>();

                    foreach (var vrPerLink in crawlResult.lstDiscoveredLinks)
                    {
                        var vrHashedLink = vrPerLink.ComputeHashOfOurSystem();

                        if (hsProcessedUrls.Contains(vrHashedLink))
                            continue;

                        var vrResult = context.tblMainUrls.Any(databaseRecord => databaseRecord.UrlHash == vrHashedLink);

                        if (vrResult == false)
                        {
                            crawlingResult newLinkCrawlingResult = new crawlingResult();
                            newLinkCrawlingResult.Url = vrPerLink.normalizeUrl();
                            newLinkCrawlingResult.HostUrl = newLinkCrawlingResult.Url.returnRootUrl();
                            newLinkCrawlingResult.UrlHash = vrPerLink.ComputeHashOfOurSystem();
                            newLinkCrawlingResult.ParentUrlHash = crawlResult.UrlHash;
                            newLinkCrawlingResult.LinkDepthLevel = (short)(crawlResult.LinkDepthLevel + 1);
                            context.tblMainUrls.Add(newLinkCrawlingResult.converToBaseMainUrlClass());
                            hsProcessedUrls.Add(vrHashedLink);
                            Interlocked.Increment(ref irDiscoveredUrlCount);
                        }
                    }

                    context.SaveChanges();
                }
            }
        }

        private static void saveCrawlInDatabase(crawlingResult crawledResult)
        {

            lock (_lockDatabaseAdd)
            {
                using (var context = new DBCrawling())
                {
                    crawledResult.UrlHash = crawledResult.Url.ComputeHashOfOurSystem();
                    crawledResult.HostUrl = crawledResult.Url.returnRootUrl();
                    var vrResult = context.tblMainUrls.SingleOrDefault(b => b.UrlHash == crawledResult.UrlHash);
                    crawledResult.ParentUrlHash = crawledResult.ParentUrlHash.ComputeHashOfOurSystem();

                    if (crawledResult.blcrawlSuccess == true)
                    {
                        crawledResult.IsCrawled = true;
                        if (!string.IsNullOrEmpty(crawledResult.SourceCode))
                        {


                            double dblOriginalSourceCodeLenght = crawledResult.SourceCode.Length;
                            crawledResult.SourceCode = crawledResult.SourceCode.CompressString();
                            crawledResult.CompressionPercent = Convert.ToByte(
                                Math.Floor(
                                    ((crawledResult.SourceCode.Length.ToDouble() / dblOriginalSourceCodeLenght) * 100))
                                );
                        }
                        crawledResult.CrawlTryCounter = 0;
                    }


                    tblMainUrl finalObject = crawledResult.converToBaseMainUrlClass();

                    //this approach brings extra overhead to the server with deleting from server first
                    //therefore will use copy properties of object to another object without changing reference
                    //if (vrResult != null)
                    //{
                    //    context.tblMainUrls.Remove(vrResult);
                    //    context.SaveChanges();
                    //}

                

                    if (vrResult != null)
                    {
                        finalObject.DiscoverDate = vrResult.DiscoverDate;
                        finalObject.LinkDepthLevel = vrResult.LinkDepthLevel;
                        finalObject.CrawlTryCounter = vrResult.CrawlTryCounter;
                        if (crawledResult.blcrawlSuccess == false)
                            finalObject.CrawlTryCounter++;
                        finalObject.CopyProperties(vrResult);
                    }
                    else
                        context.tblMainUrls.Add(finalObject);


                    var gg = context.SaveChanges();
                }
            }
        }

        private static tblMainUrl converToBaseMainUrlClass(this tblMainUrl finalObject)
        {
            return JsonConvert.DeserializeObject<tblMainUrl>(JsonConvert.SerializeObject(finalObject));
        }

        public static void CopyProperties(this object source, object destination)
        {
            // If any this null throw an exception
            if (source == null || destination == null)
                throw new Exception("Source or/and Destination Objects are null");
            // Getting the Types of the objects
            Type typeDest = destination.GetType();
            Type typeSrc = source.GetType();
            // Collect all the valid properties to map
            var results = from srcProp in typeSrc.GetProperties()
                          let targetProperty = typeDest.GetProperty(srcProp.Name)
                          where srcProp.CanRead
                          && targetProperty != null
                          && (targetProperty.GetSetMethod(true) != null && !targetProperty.GetSetMethod(true).IsPrivate)
                          && (targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) == 0
                          && targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType)
                          select new { sourceProperty = srcProp, targetProperty = targetProperty };
            //map the properties
            foreach (var props in results)
            {
                props.targetProperty.SetValue(destination, props.sourceProperty.GetValue(source, null), null);
            }
        }

        private static string decodeUrl(this string srUrl)
        {
            return HtmlEntity.DeEntitize(srUrl);
        }

        private static void extractLinks(crawlingResult myCrawlingResult, HtmlDocument doc)
        {
            var baseUri = new Uri(myCrawlingResult.Url);

            // extracting all links
            var vrNodes = doc.DocumentNode.SelectNodes("//a[@href]");
            if (vrNodes != null)
                foreach (HtmlNode link in vrNodes)//xpath notation
                {
                    HtmlAttribute att = link.Attributes["href"];
                    //this is used to convert from relative path to absolute path
                    var absoluteUri = new Uri(baseUri, att.Value.ToString().decodeUrl());

                    if (!absoluteUri.ToString().StartsWith("http://") && !absoluteUri.ToString().StartsWith("https://"))
                        continue;

                    myCrawlingResult.lstDiscoveredLinks.Add(absoluteUri.ToString().Split('#').FirstOrDefault());
                }

            myCrawlingResult.lstDiscoveredLinks = myCrawlingResult.lstDiscoveredLinks.Distinct().Where(pr => pr.Length < 201).ToList();

            var vrDocTitle = doc.DocumentNode.SelectSingleNode("//title")?.InnerText.ToString().Trim();
            vrDocTitle = System.Net.WebUtility.HtmlDecode(vrDocTitle);

            myCrawlingResult.PageTile = vrDocTitle;
        }

        private static StreamWriter swErrorLogs = new StreamWriter("error_logs.txt", append: true, encoding: Encoding.UTF8);
        private static object _lock_swErrorLogs = new object();

        static csHelperMethods()
        {
            swErrorLogs.AutoFlush = true;
            swLog.AutoFlush = true;
        }

        public static void logError(Exception E, string callingMethodName)
        {
            lock (_lock_swErrorLogs)//i am using lock methodology to synchronize access to a non-thread safe object streamwriter
            {
                swErrorLogs.WriteLine(callingMethodName + "\t" + DateTime.Now);
                swErrorLogs.WriteLine();
                swErrorLogs.WriteLine(E.Message);
                swErrorLogs.WriteLine();
                swErrorLogs.WriteLine(E?.InnerException?.Message);
                swErrorLogs.WriteLine();
                swErrorLogs.WriteLine(E?.StackTrace);
                swErrorLogs.WriteLine();
                swErrorLogs.WriteLine(E?.InnerException?.StackTrace);
                swErrorLogs.WriteLine();
                swErrorLogs.WriteLine("************************************");
                swErrorLogs.WriteLine();
            }
        }

        public static string normalizeUrl(this string srUrl)
        {
            return srUrl.ToLower(new System.Globalization.CultureInfo("en-US")).Trim();
        }

        private static string ComputeSha256Hash(this string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        static string ComputeHashOfOurSystem(this string srUrl)
        {
            return srUrl.normalizeUrl().ComputeSha256Hash();
        }

        public static string returnRootUrl(this string srUrl)
        {
            var uri = new Uri(srUrl);
            return uri.Host;
        }

        private static StreamWriter swLog = new StreamWriter("logs.txt", true, Encoding.UTF8);
        private static object _lock_swLogs = new object();
        public static void logMesssage(string srMsg)
        {
            lock (_lock_swLogs)
            {
                swLog.WriteLine($"{DateTime.Now}\t\t{srMsg}");
            }
        }

    }
}
