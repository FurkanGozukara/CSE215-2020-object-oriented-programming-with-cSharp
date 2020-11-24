using System;
using System.Collections.Generic;
using System.Text;

namespace lecture_7
{
    public static class generic_extensions
    {
        public static string ToDetailedDate()
        {
            return DateTime.Now.ToString("hh:mm:ss.fff tt");
        }
    }
}
