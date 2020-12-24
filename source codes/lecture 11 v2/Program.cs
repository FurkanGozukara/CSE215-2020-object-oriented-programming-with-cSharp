using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace lecture_11_v2
{
    class Program
    {
        delegate int NumberChanger(int n);//signature is taking single integer and returning integer
        static int num = 10;

        public static int AddNum(int p)
        {
            num += p;
            return num;
        }
        public static int MultNum(int q)
        {
            num *= q;
            return num;
        }
        public static int getNum()
        {
            return num;
        }

        static FileStream fs;
        static StreamWriter sw;

        // delegate declaration
        public delegate void printString(string s);

        // this method prints to the console
        public static void WriteToScreen(string str)
        {
            Console.WriteLine("The String is: {0}", str);
        }

        //this method prints to a file
        public static void WriteToFile(string s)
        {
            fs = new FileStream("message.txt",
            FileMode.Append, FileAccess.Write);
            sw = new StreamWriter(fs);
            sw.WriteLine(s);
            sw.Flush();
            sw.Close();
            fs.Close();
        }

        // this method takes the delegate as parameter and uses it to
        // call the methods as required
        public static void sendString(printString ps)
        {
            ps("Hello World");
        }

        public class Name
        {
            private string instanceName;

            public Name(string name)
            {
                this.instanceName = name;
            }

            public void DisplayToConsole()
            {
                Console.WriteLine(this.instanceName);
            }

           
        }

        static void Main(string[] args)
        {
            //create delegate instances
            NumberChanger nc1 = new NumberChanger(AddNum);
            NumberChanger nc2 = new NumberChanger(MultNum);

            //calling the methods using the delegate objects
            nc1(25);// equal to AddNum(25)
            Console.WriteLine("Value of Num: {0}", getNum());
            nc2(5);// equal to MultNum(25)
            Console.WriteLine("Value of Num: {0}", getNum());


            NumberChanger nc = nc1;
            nc += nc2;

            num = 10;
            nc(5);

           

            Console.WriteLine("Value of Num: {0}", getNum());//expected result is (5+10) * 5


            printString ps1 = new printString(WriteToScreen);
            printString ps2 = new printString(WriteToFile);
            sendString(ps1);
            sendString(ps2);

            Name testName = new Name("Koani");
            Action showMethod = testName.DisplayToConsole;
            showMethod();

            Func<int, int, int> sum = delegate (int a, int b) { return a + b; };//anonymous  method
            //https://www.tutorialsteacher.com/csharp/csharp-anonymous-method
            //https://docs.microsoft.com/tr-tr/dotnet/csharp/programming-guide/statements-expressions-operators/anonymous-functions
            //https://docs.microsoft.com/en-us/dotnet/api/system.action-4?view=net-5.0

            Console.WriteLine(sum(3, 4));  // output: 7

            List<String> names = new List<String>();
            names.Add("Bruce");
            names.Add("Alfred");
            names.Add("Tim");
            names.Add("Richard");

            // Display the contents of the list using the Print method.
            names.ForEach(Print);

            // The following demonstrates the anonymous method feature of C#
            // to display the contents of the list to the console.
            names.ForEach(delegate (String name)
            {
                Console.WriteLine(name);
            });

            void Print(string s)
            {
                Console.WriteLine(s);
            }

            /* This code will produce output similar to the following:
            * Bruce
            * Alfred
            * Tim
            * Richard
            * Bruce
            * Alfred
            * Tim
            * Richard
            */


            // Declare a Func variable and assign a lambda expression to the
            // variable. The method takes a string and converts it to uppercase.
            Func<string, string> selector = str => str.ToUpper();
            //https://docs.microsoft.com/en-us/dotnet/api/system.func-2?view=net-5.0
            // Create an array of strings.
            string[] words = { "orange", "apple", "Article", "elephant" };
            // Query the array and select strings according to the selector method.
            IEnumerable<String> aWords = words.Select(selector);

            // Output the results to the console.
            foreach (String word in aWords)
                Console.WriteLine(word);

            /*
            This code example produces the following output:

            ORANGE
            APPLE
            ARTICLE
            ELEPHANT

            */

            Console.ReadKey();
        }
    }
}
