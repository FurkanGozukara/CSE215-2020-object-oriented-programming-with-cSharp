using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace lecture_7
{
    public class non_static_variables
    {
        public Dictionary<int, double> dicValues;

        //the constructor of the static class is executed only once at the initilization when the first time class is called
        public non_static_variables()
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
    }
}
