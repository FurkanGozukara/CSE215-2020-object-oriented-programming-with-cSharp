using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;

namespace Lecture_2
{
    class Program
    {
        private static int irDictionarySize = 1000000;

        static void Main(string[] args)
        {
            Console.WriteLine("constructing the real dictionary without try catch...");
            Stopwatch mySw = new Stopwatch();
            mySw.Start();
            dictionaryGenerator(false);
            mySw.Stop();

            Console.WriteLine("dictionary building took with proper way: " + mySw.ElapsedMilliseconds + " ms");

            Console.WriteLine("constructing the real dictionary with try catch...");
            mySw = new Stopwatch();
            mySw.Start();
            dictionaryGenerator(true);
            mySw.Stop();

            Console.WriteLine("dictionary building took with try catch: " + mySw.ElapsedMilliseconds + " ms");

            Console.WriteLine($"please type a number between 0 and {irDictionarySize.ToString()} to see its randomly assigned character");

            string srReadLine = Console.ReadLine();

            mySw.Restart();
            var vrRandomChar = dicRandomNumbersAndChars[srReadLine];
            mySw.Stop();

            Console.WriteLine($"your entered number is: {srReadLine.toInt32().ToString("N0")} - its randomly assigned character is '{vrRandomChar}' \r\n time passed to find the random char : {mySw.ElapsedMilliseconds} ms");

            Console.WriteLine();
            Console.WriteLine("constructing the fake dictionary...");
            mySw.Restart();
            fake_dictionary_init();
            mySw.Stop();

            Console.WriteLine("fake dictionary building took: " + mySw.ElapsedMilliseconds + " ms");

            Console.WriteLine($"please type a number between 0 and {irDictionarySize.ToString()} to see its randomly assigned character");

            srReadLine = Console.ReadLine();

            for (int i = 0; i < 5; i++)
            {

                mySw.Restart();
                vrRandomChar = find_string_in_fake_dictionary_brute_force(srReadLine);
                mySw.Stop();

                Console.WriteLine($"your entered number is: {srReadLine.toInt32().ToString("N0")} - its randomly assigned character is '{vrRandomChar}' {Environment.NewLine} time passed to find the random char with brute force fake dictionary research : {mySw.ElapsedMilliseconds} ms");


                mySw.Restart();
                vrRandomChar = find_string_in_fake_dictionary_brute_force(srReadLine, true);
                mySw.Stop();

                Console.WriteLine($"your entered number is: {srReadLine.toInt32().ToString("N0")} - its randomly assigned character is '{vrRandomChar}' {Environment.NewLine} time passed to find the random char with linq dictionary research : {mySw.ElapsedMilliseconds} ms");
            }
        }

        static char find_string_in_fake_dictionary_brute_force(string srKey, bool blUseLinq = false)
        {
            if (!blUseLinq) // if(blUseLinq==false)
                foreach (var vrPerKey in lstFakeDictionary)
                {
                    if (vrPerKey.Item1 == srKey)
                        return vrPerKey.Item2;
                }

            if (blUseLinq) // if(blUseLinq==true)
                return lstFakeDictionary.Where(pr => pr.Item1 == srKey).Select(pr => pr.Item2).FirstOrDefault();

            return '0';
        }

        private static Dictionary<string, char> dicRandomNumbersAndChars = new Dictionary<string, char>();

        static void dictionaryGenerator(bool blWithTryCatch = false)
        {
            dicRandomNumbersAndChars.Clear();

            Random myRand = new Random();
            List<char> myCharList = Enumerable.Range('a', 'z' - 'a' + 1).Select(i => (Char)i).ToList();
            for (int i = 0; i < irDictionarySize; i++)
            {
                dicRandomNumbersAndChars.Add(i.ToString(),
                    myCharList[myRand.Next(0, myCharList.Count - 1)]);
            }


            if (blWithTryCatch == true)
            {
                for (int i = 0; i < irDictionarySize; i++)
                {
                    try
                    {
                       if( myRand.Next(0,12)==11)
                        {
                            var tempVal = dicRandomNumbersAndChars["non existing key"];
                        }

                        if (myRand.Next(0, 12) == 10)
                        {
                            throw new SystemException("custom exception");
                        }

                        dicRandomNumbersAndChars.Add(i.ToString(),
                            myCharList[myRand.Next(0, myCharList.Count - 1)]);
                    }
                    catch(KeyNotFoundException KNFE)
                    {
                        Console.WriteLine("key not found exception happened - msg : " + KNFE.Message.ToString());
                    }
                    catch (ArgumentException AE)
                    {
                        Console.WriteLine("key already exists exception happened - msg : " + AE.Message.ToString());
                    }
                    catch (Exception E)
                    {
                        Console.WriteLine("some other exception happened - msg : " + E.Message.ToString());
                    }
                    System.Threading.Thread.Sleep(100);
                }
            }

            if (blWithTryCatch == false)
            {
                for (int i = 0; i < irDictionarySize; i++)
                {
                    if (dicRandomNumbersAndChars.ContainsKey(i.ToString()) == false)
                        dicRandomNumbersAndChars.Add(i.ToString(),
                            myCharList[myRand.Next(0, myCharList.Count - 1)]);
                }
            }

        }

        static List<Tuple<string, char>> lstFakeDictionary = new List<Tuple<string, char>>();
        static void fake_dictionary_init()
        {
            Random myRand = new Random();
            List<char> myCharList = Enumerable.Range('a', 'z' - 'a' + 1).Select(i => (Char)i).ToList();
            for (int i = 0; i < irDictionarySize; i++)
            {
                lstFakeDictionary.Add(new Tuple<string, char>(i.ToString(), myCharList[myRand.Next(0, myCharList.Count - 1)]));
            }
        }
    }
}
