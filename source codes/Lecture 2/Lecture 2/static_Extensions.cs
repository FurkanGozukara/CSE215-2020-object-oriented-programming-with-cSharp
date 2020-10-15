using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Lecture_2
{
    public static class static_Extensions
    {
        public static int toInt32(this string srGG)
        {
            int irReturnNumber = 0;
            Int32.TryParse(srGG, out irReturnNumber);
            return irReturnNumber;
        }
    }
}
