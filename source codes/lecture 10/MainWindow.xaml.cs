using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Diagnostics;

namespace lecture_10
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string srLetters = "ABCSŞÜĞİÇOÖCI";

            //en-us
            var vrCurrentCulture = CultureInfo.CurrentCulture;
            //en-us
            var vrCurrentCultureUI = CultureInfo.CurrentUICulture;

            //abcsşüğİçoöci
            string srLowerDefault = srLetters.ToLower();
            //abcsşüğİçoöci
            string srLowerInvariant = srLetters.ToLowerInvariant();
            //abcsşüğiçoöcı
            string srLowerTR = srLetters.ToLower(new CultureInfo("tr-TR"));
            //3.421
            double dbl1 = Convert.ToDouble("3.421");
            //3421
            double dbl2 = Convert.ToDouble("3,421");

            CultureInfo.CurrentCulture = new CultureInfo("tr-TR");

            Thread thread = new Thread(new ThreadStart(testThread));
            thread.Start();

            Task.Factory.StartNew(() => { testTask(); });

            vrCurrentCulture = CultureInfo.CurrentCulture;
            //en-us
            vrCurrentCultureUI = CultureInfo.CurrentUICulture;

            //abcsşüğiçoöcı
            srLowerDefault = srLetters.ToLower();
            //abcsşüğİçoöci
            srLowerInvariant = srLetters.ToLowerInvariant();
            //abcsşüğiçoöcı
            srLowerTR = srLetters.ToLower(new CultureInfo("tr-TR"));
            //3421
            dbl1 = Convert.ToDouble("3.421");
            //3.421
            dbl2 = Convert.ToDouble("3,421");


        }

        private void testThread()
        {
            while (true)
            {
                //tr-TR // same culture as the thread starting sub thread
                var vrCurrentCulture = CultureInfo.CurrentCulture;
                //en-US
                var vrCurrentCultureUI = CultureInfo.CurrentUICulture;
                System.Threading.Thread.Sleep(100);
            }
        }

        private void testTask()
        {
            while (true)
            {
                //tr-TR // same culture as the thread starting sub thread
                var vrCurrentCulture = CultureInfo.CurrentCulture;
                //en-US
                var vrCurrentCultureUI = CultureInfo.CurrentUICulture;
                System.Threading.Thread.Sleep(100);
            }
        }

        private void btnSealedClass_Click(object sender, RoutedEventArgs e)
        {
            var cultureCurrent = CultureInfo.CurrentCulture;
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            SealedClass myTestclass = new SealedClass();
            int irTest = myTestclass.Add(32, 123);
            string srTest = "something";

            srTest = "something2";//this composes a new object of string. we dont use the previous composed. therefore it gets garbage collected. because this is immutable type
            irTest = 321;//because this is not a reference type

            SealedClass myTestclass2 = new SealedClass();

            myTestclass = myTestclass2;//this just copies reference of (we can also say memory address of) myTestclass2 to myTestclass. Because this is reference type

            X myClass1 = new X();
            myClass1.srName = "X";

            Y myClass2 = new Y();
            myClass2.srName = "Y";
            //Y 1 overriden
            Z myClass3 = new Z();
            myClass3.srName = "Z";
        }

        //public class myTestclassStruct : int //this would throw compiler error since you can not inherit a sealed class
        //{

        //}

        // Sealed class  
        sealed class SealedClass
        {
            public int Add(int x, int y)
            {
                return x + y;
            }
        }

        //public class TestClassSealed : SealedClass//this throws compiler error since you cannot inherit from a sealed type object
        //{

        //}

        class X
        {
            public virtual string srName { get; set; }
            protected virtual void F()
            {
                Debug.WriteLine("X.F");
            }
            protected virtual void F2()
            {
                Debug.WriteLine("X.F2");
            }
        }

        class Y : X
        {
            public sealed override string srName
            {
                get
                {
                    return base.srName + " overriden";
                }
                set
                {
                    base.srName = value + " 1";
                }
            }

            sealed protected override void F()
            {
                Debug.WriteLine("Y.F");
            }
            protected override void F2()
            {
                Console.WriteLine("X.F3");
            }
        }

        class Z : Y
        {
            //public override string srName // compiler error due to override
            //{
            //    get
            //    {
            //        return base.srName + " overriden";
            //    }
            //    set
            //    {
            //        base.srName = value + " 1";
            //    }
            //}

            // Attempting to override F causes compiler error CS0239.  
            //   
            //protected override void F()//this causes compile error because its parent class has this method as sealed
            //{
            //    Console.WriteLine("C.F");
            //}
            // Overriding F2 is allowed.   
            protected override void F2()
            {
                Console.WriteLine("Z.F2");
            }
        }

        class DataStore<T>
        {
            public T Data { get; set; }//this is property
        }

        class DataStore_v2<T>
        {
            public T data;//this is a generic field
            //you can not initialize it when it is a generic type
        }

        private void genericsExample_Click(object sender, RoutedEventArgs e)
        {
            DataStore<string> store = new DataStore<string>();
            store.Data = "string only";
            //store.Data = 321;//this fails because once you initialized it with a certain data type, you can not change it again to different data type


            DataStore<double> market = new DataStore<double>();
            market.Data = 321.45;

            KeyValuePair<int, string> kvp1 = new KeyValuePair<int, string>();
            kvp1.Key = 100;
            kvp1.Value = "Hundred";

            KeyValuePair<string, string> kvp2 = new KeyValuePair<string, string>();
            kvp2.Key = "IT";
            kvp2.Value = "Information Technology";


            DataStore_v2<int> test = new DataStore_v2<int>();
            test.data = 432432;

            GenericMethods<float> myGenericDataHolder = new GenericMethods<float>();
            myGenericDataHolder.AddOrUpdate(0, 32.32112F);
            myGenericDataHolder.AddOrUpdate(0, 52.32112F);

            var vrDEfault = myGenericDataHolder.GetData(3212);

            GenericMethods<string> genericStringsHolder = new GenericMethods<string>();
            genericStringsHolder.AddOrUpdate(0, 32.32112F.ToString());
            genericStringsHolder.AddOrUpdate(0, 52.32112F.ToString());

            var vrDEfaultString = genericStringsHolder.GetData(3212);

            genericMethodOverloadings<double> vrOverloads = new genericMethodOverloadings<double>();
            vrOverloads.AddOrUpdate(32, 123.0);//number 1
            vrOverloads.AddOrUpdate<int>(32, 123);//number 3
            vrOverloads.AddOrUpdate(32.0, 123.0);//number 2
            vrOverloads.AddOrUpdate(321);//number 4

            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            Printer printClass = new MainWindow.Printer();
            printClass.Print<int, double>("Example", "Student", 23, 87.32);


            Printer printClass2 = new MainWindow.Printer();
            printClass.Print<double, int>("Example 2", "Student", 23.32, 87);


        }

        class Printer
        {
            public void Print<Age, Score>(string vr1,
                string vr2, Age vr3, Score vr4)
            {
                MessageBox.Show($"{vr1} {vr2} is {vr3.ToString()} years old and got {vr4.ToString()} score");
            }
        }

        class genericMethodOverloadings<T>
        {
            public void AddOrUpdate(int index, T data)
            {

                MessageBox.Show("number 1");
            }//number 1
            public void AddOrUpdate(T data1, T data2) { MessageBox.Show("number 2"); }//number 2
            public void AddOrUpdate<U>(T data1, U data2) { MessageBox.Show("number 3"); }//number 3
            public void AddOrUpdate(T data) { MessageBox.Show("number 4"); }// number 4
        }

        class ArrayDataStore<T>
        {
            public T[] data = new T[10];
            public List<T> list = new List<T>();
            public List<string> lstNames = new List<string>();

            public string returnName()
            {
                return this.lstNames.FirstOrDefault();
            }

            public T printDATA()
            {
                return this.data[0];
            }

            public int irSalary { get; set; } = 100;

            public T genericSalary { get; set; }
        }

        class GenericMethods<T>
        {
            private T[] _data = new T[10];

            public void AddOrUpdate(int index, T item)
            {
                if (index >= 0 && index < 10)
                    _data[index] = item;
            }

            public T GetData(int index)
            {
                if (index >= 0 && index < 10)
                    return _data[index];
                else
                    return default(T);
            }
        }





        //        Why Sealed Classes?
        //We just saw how to create and use a sealed class in C#. The main purpose of a sealed class is to take away the inheritance feature from the class users so they cannot derive a class from it. One of the best usage of sealed classes is when you have a class with static members. For example, the Pens and Brushes classes of the System.Drawing namespace.

        //The Pens class represents the pens with standard colors.This class has only static members.For example, Pens.Blue represents a pen with blue color.Similarly, the Brushes class represents standard brushes.The Brushes.Blue represents a brush with blue color.

        //So when you're designing a class library and want to restrict your classes not to be derived by developers, you may want to use sealed classes.
        //source https://www.c-sharpcorner.com/article/sealed-class-in-C-Sharp/


        class KeyValuePair<TKey, TValue>
        {
            public TKey Key { get; set; }
            public TValue Value { get; set; }
        }

        private void classComparison_Click(object sender, RoutedEventArgs e)
        {
            EmployeeNoCompare_v1 v1 = new EmployeeNoCompare_v1 { Name = "Furkan", Salary = 100 };
            MessageBox.Show("version 1: " + v1.ToString());

            EmployeeNoCompare_v2 v2 = new EmployeeNoCompare_v2 { Name = "Furkan", Salary = 100 };
            MessageBox.Show("version 2: " + v2.ToString());

            List<EmployeeNoCompare_v2> lstEmployes2 = new List<EmployeeNoCompare_v2>();

            Random rand = new Random();

            for (int i = 1; i < 101; i++)
            {
                EmployeeNoCompare_v2 ee = new EmployeeNoCompare_v2 { Name = "E" + i, Salary = rand.Next() };
                lstEmployes2.Add(ee);
            }

            //lstEmployes2.Sort();//throws run time error ArgumentException: At least one object must implement IComparable.

            EmployeeNoCompare_v2 v2_2 = new EmployeeNoCompare_v2 { Name = "Furkan", Salary = 100 };

            if (v2 == v2_2)//this will only compare their reference address
            { }
            else
            {
                MessageBox.Show("v2 is not equal v2_2");
            }

            //to check whether 2 custom class objects are same or not we need to use IEquatable

            Employee ev1 = new Employee { Name = "Furkan", Salary = 100 };
            Employee ev2 = new Employee { Name = "Furkan", Salary = 100 };

            if (ev1.Equals(ev2))
            {
                MessageBox.Show("ev1 is equal to ev2");
            }

            ev1.Salary = 101;//they wont be equal anymore

            if (ev1.Equals(ev2) == false)
            {
                MessageBox.Show("ev1 is not anymore equal to ev2");
            }

            List<Employee> lstEmployes = new List<Employee>();

            rand = new Random();

            for (int i = 1; i < 101; i++)
            {
                Employee ee = new Employee { Name = "E" + i, Salary = rand.Next(), Age = rand.Next() };
                lstEmployes.Add(ee);
            }


            lstEmployes.Sort();//in here it is sorted by descending according to salary
            lstEmployes.Reverse();//in here it becomes sorted by ascending since we reverse the list

            //SuperKeyType<string, int, EmployeeNoCompare_v1> testtt = new SuperKeyType<string, int, EmployeeNoCompare_v1>();
        }

        class EmployeeNoCompare_v1
        {
            public string Name { get; set; }
            public int Salary { get; set; }
        }

        class EmployeeNoCompare_v2
        {
            public string Name { get; set; }
            public int Salary { get; set; }

            public override string ToString()//we are overriding a generic extension to properly format our custom class
            {
                return $"{this.Name} {this.Salary}";
            }
        }


        class Employee : IEquatable<Employee>, IComparable<Employee>
        {
            public string Name { get; set; }
            public Int64 Salary { get; set; }
            public Int64 Age;

            public bool Equals(Employee other)//Equals is a special method name that has to be used with IEquatable
            {
                if (this.Salary == other.Salary && this.Name == other.Name)
                    return true;

                return false;
            }

            public int CompareTo(Employee other)//CompareTo is a special method name that has to be used with IComparable
            {
                //this comparison method currently sorts by descending order
                if ((this.Salary * this.Age) < (other.Salary * other.Age))
                {
                    return 1;
                }
                else if ((this.Salary * this.Age) > (other.Salary * other.Age))
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }

            public override string ToString()
            {
                return $"{this.Name} {this.Salary}";
            }
        }

        class SuperKeyType<K, V, U>
            where K : struct  // constraint
    where U : System.IComparable<U> // constraint
    where V : class, new()  // constraint
        { }

        public class testClass<T>
            where T : struct
        {
            public T testMetod(T data)
            {
                return data;
            }
        }


    }
}
