using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace lecture_11
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnEnumerators_Click(object sender, RoutedEventArgs e)
        {
            Employee firstEmployee = new Employee(1, "furkan", 100);
            screenList.Items.Add(firstEmployee.ToString());

            Employees EmpList = new Employees();
            Employee e1 = new Employee(1, "Employee#1", 1250.75);
            Employee e2 = new Employee(2, "Employee#2", 1275.85);
            EmpList.AddEmployee(e1);
            EmpList.AddEmployee(e2);

            foreach (Employee emp in EmpList)
            {
                screenList.Items.Add(emp.ToString());
            }

            IEnumerator EmpEnumerator = EmpList.GetEnumerator();
            EmpEnumerator.Reset();
            while (EmpEnumerator.MoveNext())
            {
                screenList.Items.Add((Employee)EmpEnumerator.Current);
            }

            // Display powers of 2 up to the exponent of 8:
            foreach (int i in Power(2, 8))
            {
                screenList.Items.Add(i);
            }

            Debug.WriteLine(" ");

            Debug.WriteLine(" ");

            ShowGalaxies();
        }

        class Employee
        {
            private int Id;
            private string Name;
            private double Salary;
            public Employee(int id, string name, double salary)//after we compose an instance of object, the id is not setable again
            {
                this.Id = id;
                this.Name = name;
                this.Salary = salary;
            }
            public int ID//this is only get not setable
            {
                get
                {
                    return this.Id;
                }
            }
            public override string ToString()
            {
                return "Id: " + this.Id.ToString() + "\tName: " + Name + "\tSalary: " + Salary.ToString();
            }
        }

        class Employees : IEnumerable, IEnumerator
        {
            ArrayList EmpList = new ArrayList();
            private int Position = -1;
            public void AddEmployee(Employee oEmp)
            {
                EmpList.Add(oEmp);
            }
            /* Needed since Implementing IEnumerable*/
            public IEnumerator GetEnumerator()
            {
                return (IEnumerator)this;
            }
            /* Needed since Implementing IEnumerator*/
            public bool MoveNext()
            {
                if (Position < EmpList.Count - 1)
                {
                    ++Position;
                    return true;
                }
                return false;
            }
            public void Reset()
            {
                Position = -1;
            }
            public object Current
            {
                get
                {
                    return EmpList[Position];
                }
            }
        }

        public static IEnumerable<int> Power(int number, int exponent)
        {
            int result = 1;

            for (int i = 0; i < exponent; i++)
            {
                result = result * number;
                if (result > 100)
                    break;//this will only break for loop and method will continue
                if (result > 100)
                    yield break;//equal to return
                yield return result;
            }

            Debug.WriteLine("  break;");
        }

        public static void ShowGalaxies()
        {
            var theGalaxies = new Galaxies();
            foreach (Galaxy theGalaxy in theGalaxies.NextGalaxy)
            {
                Debug.WriteLine(theGalaxy.Name + " " + theGalaxy.MegaLightYears.ToString());
            }
        }

        public class Galaxies
        {
            public IEnumerable<Galaxy> NextGalaxy//this is IEnumerable property
            {
                get
                {
                    yield return new Galaxy { Name = "Tadpole", MegaLightYears = 400 };
                    yield return new Galaxy { Name = "Pinwheel", MegaLightYears = 25 };
                    yield return new Galaxy { Name = "Milky Way", MegaLightYears = 0 };
                    yield return new Galaxy { Name = "Andromeda", MegaLightYears = 3 };
                }
            }
        }

        public class Galaxy
        {
            public String Name { get; set; }
            public int MegaLightYears { get; set; }
        }

        private void btnRegulerException_Click(object sender, RoutedEventArgs e)
        {
            Convert.ToInt32("123test");
        }

        private void btnTaskException_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() => { Convert.ToInt32("123test"); });
        }

        private void btnSubThreadException_Click(object sender, RoutedEventArgs e)
        {
            //unhandles exceptions in threads causes parent thread crash as well
            Thread thread1 = new Thread(generateError);
            thread1.IsBackground = true;
            thread1.Start();
        }

        private void generateError()
        {
            Convert.ToInt32("123test");
        }

        private void btnDispatcherErrorCheck_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Convert.ToInt32("123test");
            }));
        }

        private void btnStackExample_Click(object sender, RoutedEventArgs e)
        {
            Stack<int> myStack = new Stack<int>();
            myStack.Push(1);
            myStack.Push(2);
            myStack.Push(3);
            myStack.Push(4);

            foreach (var item in myStack)
                screenList.Items.Add(item + ","); //prints 4,3,2,1, 

             myStack = new Stack<int>();
            myStack.Push(1);
            myStack.Push(2);
            myStack.Push(3);
            myStack.Push(4);

            //https://www.tutorialsteacher.com/csharp/csharp-stack

            while (myStack.Count > 0)
                screenList.Items.Add(myStack.Pop() + ",");

            foreach (var item in myStack)
                screenList.Items.Add(item + ","); //prints 4,3,2,1, 
        }
    }
}