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

namespace lecture_13
{
    public static class csHelperMethods
    {
        public static void clearDatabase()
        {
            using (var context = new DBCrawling())
            {
                var ctx = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext;
                ctx.ExecuteStoreCommand("truncate table tblMainUrls");
            }
        }

        public class crawlingResult
        {
            public bool blcrawlSuccess = true;
            public string srCrawledUrl = null;
            public string srCrawledUrlHashed = null;
            public DateTime dtDiscoverDate;
            public short irLinkDepthLevel = 0;
            public DateTime dtLastCrawlingDate = DateTime.Now;
            public int irCrawlingTimeMS = 0;
            public string srCrawledSourceCode = null;
            public List<string> lstDiscoveredLinks = new List<string>();
            public string srTitleofPage = null;
            public string srParentUrlHash = null;
        }

        public static void crawlPage(string srUrlToCrawl, int irUrlDepthLevel, string _srParentUrl, DateTime _dtDiscoverDate)
        {
            var vrLocalUrl = srUrlToCrawl;
            crawlingResult crawlResult = new crawlingResult();
            crawlResult.srCrawledUrl = vrLocalUrl;
            crawlResult.srParentUrlHash = _srParentUrl;
            crawlResult.dtDiscoverDate = _dtDiscoverDate;

            Stopwatch swTimerCrawling = new Stopwatch();
            swTimerCrawling.Start();

            HtmlWeb wbClient = new HtmlWeb();//you should use httpwebrequest for more control and better performance
            wbClient.AutoDetectEncoding = true;
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            try
            {
                doc = wbClient.Load(crawlResult.srCrawledUrl);
                crawlResult.srCrawledSourceCode = doc.Text;
            }
            catch (Exception E)
            {
                crawlResult.blcrawlSuccess = false;
                logError(E, "crawlPage");
            }

            swTimerCrawling.Stop();
            crawlResult.irCrawlingTimeMS = Convert.ToInt32(swTimerCrawling.ElapsedMilliseconds);

            if (crawlResult.blcrawlSuccess)
            {
                perDocument docResults = extractLinks(crawlResult.srCrawledUrl, doc);
                crawlResult.lstDiscoveredLinks.AddRange(docResults.lstExtractedUrls);
                crawlResult.srTitleofPage = docResults.srDocTitle;
            }

            doc = null;
            saveCrawlInDatabase(crawlResult);
        }

        private static void saveCrawlInDatabase(crawlingResult crawledResult)
        {
            if (crawledResult.blcrawlSuccess == false)
                return;

            using (var context = new DBCrawling())
            {
                tblMainUrl crawledUrl = new tblMainUrl();

                crawledUrl.UrlHash = crawledResult.srCrawledUrl.normalizeUrl().ComputeSha256Hash();

                var vrResult = context.tblMainUrls.SingleOrDefault(b => b.UrlHash == crawledUrl.UrlHash);

                if (vrResult == null)
                {
                    context.tblMainUrls.Add(crawledUrl);
                }
                else
                {
                    crawledUrl = vrResult;
                    context.tblMainUrls.Attach(crawledUrl);
                    context.Entry(crawledUrl).State = EntityState.Modified;
                }

                crawledUrl.DiscoverDate = crawledResult.dtDiscoverDate;
                crawledUrl.FetchTimeMS = crawledResult.irCrawlingTimeMS;
                crawledUrl.LastCrawlingDate = crawledResult.dtLastCrawlingDate;
                crawledUrl.LinkDepthLevel = crawledResult.irLinkDepthLevel;
                crawledUrl.PageTile = crawledResult.srTitleofPage;
                crawledUrl.ParentUrlHash = crawledResult.srParentUrlHash.normalizeUrl().ComputeSha256Hash();
                crawledUrl.SourceCode = crawledResult.srCrawledSourceCode.CompressString();
                crawledUrl.CompressionPercent = Convert.ToByte(
                    Math.Floor(
                        ((crawledUrl.SourceCode.Length.ToDouble() / crawledResult.srCrawledSourceCode.Length.ToDouble()) * 100))
                    );
                crawledUrl.Url = crawledResult.srCrawledUrl;



                var gg = context.SaveChanges();


            }
        }


        public class perDocument
        {
            public string srDocTitle = "";
            public List<string> lstExtractedUrls = new List<string>();
        }

        private static perDocument extractLinks(string srUrl, HtmlDocument doc)
        {
            var baseUri = new Uri(srUrl);

            perDocument myDoc = new perDocument();

            // extracting all links
            var vrNodes = doc.DocumentNode.SelectNodes("//a[@href]");
            if (vrNodes != null)
                foreach (HtmlNode link in vrNodes)//xpath notation
                {
                    HtmlAttribute att = link.Attributes["href"];
                    //this is used to convert from relative path to absolute path
                    var absoluteUri = new Uri(baseUri, att.Value.ToString());

                    if (!absoluteUri.ToString().StartsWith("http://") && !absoluteUri.ToString().StartsWith("https://"))
                        continue;

                    myDoc.lstExtractedUrls.Add(absoluteUri.ToString().Split('#').FirstOrDefault());
                }

            myDoc.lstExtractedUrls = myDoc.lstExtractedUrls.Distinct().ToList();

            var vrDocTitle = doc.DocumentNode.SelectSingleNode("//title")?.InnerText.ToString().Trim();
            vrDocTitle = System.Net.WebUtility.HtmlDecode(vrDocTitle);

            myDoc.srDocTitle = vrDocTitle;
            return myDoc;
        }

        private static StreamWriter swErrorLogs = new StreamWriter("error_logs.txt", append: true, encoding: Encoding.UTF8);
        private static object _lock_swErrorLogs = new object();

        static csHelperMethods()
        {
            swErrorLogs.AutoFlush = true;
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

        static string normalizeUrl(this string srUrl)
        {
            return srUrl.ToLower(new System.Globalization.CultureInfo("en-US")).Trim();
        }

        static string ComputeSha256Hash(this string rawData)
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

    }
}
