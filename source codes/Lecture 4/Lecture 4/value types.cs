using System;
using System.Collections.Generic;
using System.Text;

namespace Lecture_4
{
    class value_types
    {
        bool blValueType;
        byte byteType;// default = 0
        char crType;//'/0' null as character
        decimal dcType;// default = 0
        double dblType;// default = 0
        enum enType // default = empty
        {

        };
        float flType; // default = 0
int irType; // default = 0 default = 0
        long lnType; // default = 0
        sbyte sbyteType; // default = 0
        short srType; // default = 0
        struct strucrtpe // default = empty
        {

        };
        uint untype; // default = 0
        ulong utype;// default = 0
        ushort ushortype;// default = 0
    }

    class referenceTypes
    {
        // by default null without initialization
        class csType 
        {

        };

        // by default null without initialization
        interface Animal
        {
            void animalSound(); // interface method (does not have a body)
            void run(); // interface method (does not have a body)
        }

        // by default null without initialization
        public delegate void MyDelegate(string msg); // declare a delegate

        // by default null without initialization
        object obj;

        // by default null without initialization
        string sr;
    }
}
