using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace lecture_7
{
    public static class static_variables
    {
        public static Dictionary<int, double> dicValues;

        //the constructor of the static class is executed only once at the initilization when the first time class is called
        static static_variables()
        {
            dicValues = new Dictionary<int, double>();

            foreach (var vrLine in File.ReadLines("numbers.txt"))
            {
                int irFirstVal = vrLine.Split(';')[0].ToInt32();
                double dbl1 = vrLine.Split(';')[1].ToDouble();
                double dbl2 = vrLine.Split(';')[2].ToDouble();

                if (dicValues.ContainsKey(irFirstVal) == false)
                {
                    dicValues.Add(irFirstVal, dbl1 * dbl2);
                }
                else
                {
                    dicValues[irFirstVal] += dbl1 * dbl2;
                }
            }
        }

        public static double ToDouble(this string srVal)
        {
            double dblVal = double.NaN;
            bool blResult = double.TryParse(srVal, out dblVal);
            if (blResult == false) dblVal = double.NaN;
            return dblVal;
        }

        public static int ToInt32(this string srVal)
        {
            int irVal = int.MinValue;
            bool blResult = int.TryParse(srVal, out irVal);
            if (blResult == false) irVal = int.MinValue;
            return irVal;
        }
    }
}
