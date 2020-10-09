using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Lecture_1___dll_test;

namespace Lecture_1
{
    class mainProgramClass
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            printToScreenRandomNumber myCustomPrinter = new printToScreenRandomNumber();
            myCustomPrinter.printToScreen();
            myCustomPrinter.printPrivately();
            myCustomPrinter.printScreenInternal();

            printScreenV2 myCustomPrinter_v2 = new printScreenV2();
            myCustomPrinter_v2.printProtected();
            myCustomPrinter_v2.printScreenInternal();

            dll_printToScreenRandomNumber myDllObcect = new dll_printToScreenRandomNumber();
            myDllObcect.printPrivately();
            myDllObcect.printToScreen();
            //myDllObcect.printScreenInternal(); this one fails because that method is in another DLL file

            print_screen_static.printScreenStatic();
            Console.Clear();

            car myCar = new car();
            Console.WriteLine($"my car default model is : { myCar.carModel}");
            myCar.carModel = "my second model car";
            Console.WriteLine($"my changed model is : { myCar.carModel}");
            Console.WriteLine($"my car default price is : { myCar.irPrice}");
            myCar.irPrice = 351;
            Console.WriteLine($"my changed price is : { myCar.irPrice}");
            Console.WriteLine($"my car default brand is : { myCar.srBrand}");

            Console.WriteLine($"my car default year is : { myCar.irYear}");
            myCar.irYear = 2133;
            Console.WriteLine($"my car new year is : { myCar.irYear}");
            myCar.irYear = 123;
            Console.WriteLine($"my car new year is : { myCar.irYear}");
            myCar.irYear = 3123;
            Console.WriteLine($"my car new year is : { myCar.irYear}");

            //randomNumberTest_incorrect_way();
            randomNumberTest_proper_way();
            random_bigger_dic_test();
        }

        private static void randomNumberTest_incorrect_way()
        {
            Random myRandGen = new Random();
            Dictionary<int, int> dicNumbers = new Dictionary<int, int>();
            Stopwatch timer = new Stopwatch();
            timer.Start();
            for (int i = 0; i < 100000; i++)
            {
                var vrNumber = myRandGen.Next(1, 101);
                try
                {
                    dicNumbers.Add(vrNumber, 1);
                }
                catch (Exception E)
                {
                    dicNumbers[vrNumber]++;
                    //dicNumbers[vrNumber] = dicNumbers[vrNumber] + 1; above line is equal to this line
                }
            }
            timer.Stop();
            Console.WriteLine($"elapsed total ms with try catch errors {timer.Elapsed.TotalMilliseconds.ToString("N0")}");         
        }
        
        private static void writeDictionaryToFileProper(Dictionary<int, int> dicNumbs, string srFileName )
        {
            Stopwatch swWatch = new Stopwatch();
            swWatch.Start();
            StringBuilder srBuild = new StringBuilder();
            foreach (var vrItem in dicNumbs)
            {
                srBuild.AppendLine($"number {vrItem.Key} has been randomly generated {vrItem.Value.ToString("N0")} times");
            }
            File.WriteAllText(srFileName, srBuild.ToString());
            swWatch.Stop();
            Console.WriteLine($"elapsed total ms writeDictionaryToFileProper {swWatch.Elapsed.TotalMilliseconds.ToString("N0")}");
        }

        private static void writeDictionaryToFile_incorrect_way(Dictionary<int, int> dicNumbs, string srFileName)
        {
            Stopwatch swWatch = new Stopwatch();
            swWatch.Start();
            string srTemp = "";
            int irCounter = 0;
            foreach (var vrItem in dicNumbs)
            {
                //this is string concatenation and it is extremely slow
                srTemp = srTemp+$"number {vrItem.Key} has been randomly generated {vrItem.Value.ToString("N0")} times{Environment.NewLine}";

                if (irCounter % 1000 == 0)
                {
                    Console.Write($"{irCounter.ToString("N0")}/{dicNumbs.Count.ToString("N0")}\t");
                }
                irCounter++;
            }
            File.WriteAllText(srFileName, srTemp);
            swWatch.Stop();
            Console.WriteLine($"elapsed total ms writeDictionaryToFile_incorrect_way {swWatch.Elapsed.TotalMilliseconds.ToString("N0")}");
        }

        private static void randomNumberTest_proper_way()
        {
            Random myRandGen = new Random();
            Dictionary<int, int> dicNumbers = new Dictionary<int, int>();
            Stopwatch timer = new Stopwatch();
            timer.Start();
            for (int i = 0; i < 100000; i++)
            {
                var vrNumber = myRandGen.Next(1, 101);
                if (dicNumbers.ContainsKey(vrNumber))
                {
                    dicNumbers[vrNumber]++;
                }
                else
                    dicNumbers.Add(vrNumber, 1);
            }
            timer.Stop();
            Console.WriteLine($"elapsed total ms with proper way {timer.Elapsed.TotalMilliseconds.ToString("N0")}");
        }

        private static void random_bigger_dic_test()
        {
            Random myRandGen = new Random();
            Dictionary<int, int> dicNumbers = new Dictionary<int, int>();
            Stopwatch timer = new Stopwatch();
            timer.Start();
            for (int i = 0; i < 100000; i++)
            {
                var vrNumber = myRandGen.Next();
                if (dicNumbers.ContainsKey(vrNumber))
                {
                    dicNumbers[vrNumber]++;
                }
                else
                    dicNumbers.Add(vrNumber, 1);
            }
            timer.Stop();
            Console.WriteLine($"elapsed total ms random_bigger_dic_test {timer.Elapsed.TotalMilliseconds.ToString("N0")}");

            writeDictionaryToFileProper(dicNumbers, "writeDictionaryToFileProper.txt");
            writeDictionaryToFile_incorrect_way(dicNumbers, "writeDictionaryToFile_incorrect_way.txt");
        }

        static class print_screen_static
        {
            static Random myRandGen = new Random();
            public static void printScreenStatic()
            {
                for (int startnumber = 0; startnumber < 10; startnumber++)
                {
                    Console.WriteLine("printScreenStatic: " + myRandGen.Next(0, 2).ToString("N0"));
                }
            }
        }

        public class printToScreenRandomNumber
        {
            Random myRandGen = new Random();
            public void printToScreen()
            {
                Console.WriteLine("printToScreen: " + myRandGen.Next().ToString("N0"));
            }

            private void printToScreenPrivate()
            {
                Console.WriteLine("printToScreenPrivate: " + myRandGen.Next().ToString("N0"));
            }

            protected void printToScreenProtected()
            {
                Console.WriteLine("printToScreenProtected: " + myRandGen.Next().ToString("N0"));
            }

            public void printPrivately()
            {
                printToScreenPrivate();
            }

            internal void printScreenInternal()
            {
                Console.WriteLine("printScreenInternal: " + myRandGen.Next().ToString("N0"));
            }
        }

        class printScreenV2 : printToScreenRandomNumber // simple example of inheritance
        {
            public void printProtected()
            {
                printToScreenProtected();
                printToScreen();
            }
        }

        class car
        {
            public string carModel = "Default Car";
            public string srBrand;
            public int irPrice { get; set; }

            private int _irYear = 1999;
            public int irYear
            {
                get
                {
                    if (_irYear < 2000)
                        return 2000;
                    return _irYear;
                }
                set
                {
                    _irYear = value - 10;
                    if (_irYear > 3000)
                        _irYear = 3000;
                }
            }
        }
    }
}
