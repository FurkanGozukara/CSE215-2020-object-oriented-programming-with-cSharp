using System;

namespace Lecture_2___Part_2
{
    class Program
    {
        static private int _irMyNumber;
        //property get and set
        static public int irMyNumber
        {
            get
            {
                return _irMyNumber * _irMyNumber * _irMyNumber;
            }
            set
            {
                _irMyNumber = value / 2;
            }
        }
        static void Main(string[] args)
        {
            printRandomNumber();
            printRandomNumber(32);
            printRandomNumber(12, 43);
            printRandomNumber(23, 41L, 23);
            printRandomNumber(32L, 54, 12);

            irMyNumber = 100;
            Console.WriteLine(irMyNumber);
            //what would this print to the screen?
        }

        static void printRandomNumber()//signature of this method is empty. used parameter types: nothing
        {
            Console.WriteLine("printRandomNumber:\t0");
        }

        static void printRandomNumber(int irNumber)//signature of this method is int32 - used parameter types int32
        {
            Console.WriteLine("printRandomNumber int32:\t" + irNumber);
        }

        static void printRandomNumber(int irNumb1, int irNumb2)//signature of this method is int32 + int32 - used parameter types int32 + int32
        {
            Console.WriteLine("printRandomNumber int32 + int32:\t" + irNumb1 * irNumb2);
        }

        static void printRandomNumber(int irNumb1, Int64 irBigNum, int irNumb2)//signature of this method is int32 + int64 + int32 
        {
            Console.WriteLine("printRandomNumber int32 + int64 + int32:\t" + irNumb1 * irBigNum / irNumb2);
        }

        //this works because signature takes consideration of order of the parameters as well
        static void printRandomNumber(Int64 irBigNum, int irNumb1, int irNumb2)//signature of this method is int64 + int32 + int32 
        {
            Console.WriteLine("printRandomNumber int64 + int32 + int32:\t" + irNumb1 * irBigNum / irNumb2);
        }
    }
}
