using System;

namespace lecture_12_console
{


    class Program
    {
        class Class1
        {
        }
        class Class2
        {
        }

        public static void Test(object o)
        {
            Class1 a;
            Class2 b;
            if (o is Class1)
            {
                Console.WriteLine("o is Class1");
                a = (Class1)o;
            }
            else if (o is Class2)
            {
                Console.WriteLine("o is Class2");
                b = (Class2)o;
            }
            else
            {
                Console.WriteLine("o is neither Class1 nor Class2.");
            }
        }

        static void Main(string[] args)
        {
            Class1 c1 = new Class1();
            Class2 c2 = new Class2();
            Test(c1);
            Test(c2);
            Test("Passing String Value instead of class");

            object c0 = c2;
            Class1 c3;
            //c3 = (Class1)c0;//it doesnt throw any compiler error but will throw runtime error
            //System.InvalidCastException: 'Unable to cast object of type 'Class2' to type 'Class1'.'

            if (c0 is Class1)//so only cast if they are compatible
                c3 = (Class1)c0;


            object[] myObjects = new object[6];
            myObjects[0] = new Class1();
            myObjects[1] = new Class2();
            myObjects[2] = "string";
            myObjects[3] = 32;
            myObjects[4] = null;
            for (int i = 0; i < myObjects.Length; ++i)
            {
                string s = myObjects[i] as string;
                //string s2 = (string)myObjects[i];//throws error when we do explicit casting without compatibility check with is operator
                //System.InvalidCastException: 'Unable to cast object of type 'Class1' to type 'System.String'.'
                //so with using as operator we handle null case casting
                Console.Write("{0}:", i);
                if (s != null)
                    Console.WriteLine("'" + s + "'");
                else
                    Console.WriteLine("not a string");
            }

            Console.ReadKey();
        }
    }
}
