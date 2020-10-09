using System;

namespace Lecture_1___dll_test
{
    public class dll_printToScreenRandomNumber
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
}
