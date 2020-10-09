using System;
using System.Collections.Generic;
using System.Text;
using Lecture_1;

namespace Lecture_2
{
    class InternalTest
    {
        public void internalTest()
        {
            mainProgramClass myProgram = new mainProgramClass();
            mainProgramClass.printToScreenRandomNumber anotherPrinter = new mainProgramClass.printToScreenRandomNumber();
            anotherPrinter.printScreenInternal();
        }
    }
}
