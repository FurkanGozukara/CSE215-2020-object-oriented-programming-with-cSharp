using System;
using System.Collections.Generic;
using System.Text;

namespace lecture_9
{
    public static class MethodOverLoading
    {

        // interest for 1 year of tenure
        public static double Example(double amount, double rate)//signature is double and double
        {
            return amount + (amount * rate);
        }
        public static double Example(double amount, double rate, int irExtraMultipler) // double and double and int
        {
            return amount + (amount * rate) + 2000 * irExtraMultipler;
        }
    }

    //a good example of overriding vs new 
    // https://stackoverflow.com/questions/1399127/difference-between-new-and-override

    public static class MethodOverriding
    {
        public class BaseMultiply
        {
            //the virtual keyword allow us to override a member in a derived class
            public virtual double ExecuteExample(double amount, double rate)//signature is double + double
            {
                return amount + (amount * rate);
            }

            public double NoOverRide(double amount, double rate)//signature is double + double
            {
                return amount + rate;
            }

            public virtual double NewOverride(double amount, double rate)//signature is double + double
            {
                return ( amount + rate ) * 2;
            }
        }
        // first child class
        public class PlusMultiply : BaseMultiply
        {
            public override double ExecuteExample(double amount, double rate)//signature is double + double
            {
                return amount + (amount * rate) + 1000;
            }

            //if you don't use override keyword, it inherently hides the base class method
            public double NoOverRide(double amount, double rate)//signature is double + double
            {
                return amount + rate + 1000;
            }

            //if you use new keyword, it explicty hides the base class method
            public new double NewOverride(double amount, double rate)//signature is double + double
            {
                return (amount + rate) * 2;
            }

            //hiding of base class members can be done both on virtual and non virtual members
        }
        // second child class
        public class OverMultiply : BaseMultiply
        {
            public override double ExecuteExample(double amount, double rate)//signature is double + double
            {
                return amount + (amount * rate) * 1500;
            }

            public double NoOverRide(double amount, double rate)//signature is double + double
            {
                return amount + rate + 1000000;
            }

            public new double NewOverride(double amount, double rate)//signature is double + double
            {
                return (amount + rate) * 10;
            }
        }
    }
}
