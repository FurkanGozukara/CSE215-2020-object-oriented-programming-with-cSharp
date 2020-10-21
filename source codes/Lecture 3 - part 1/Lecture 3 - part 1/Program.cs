using System;

namespace Lecture_3___part_1
{
    class Program
    {
        static void Main(string[] args)
        {
            int irCounter = 1;
            foreach (var vrArg in args)
            {
                Console.WriteLine($"arg {irCounter} = {vrArg}");
                irCounter++;
            }
        }
    }
}
