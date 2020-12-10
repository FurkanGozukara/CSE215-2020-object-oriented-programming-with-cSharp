using System;
using System.Collections.Generic;
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

namespace lecture_9
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

        public abstract class DataSource
        {
            public string srPublicField = "good code";

            protected string dataSourceName;
            private string environment;

            //default contructor without any parameters
            public DataSource()
            {

            }

            //contructor overloading
            protected DataSource(string environment, string dsName)
            {
                this.environment = environment;
                this.dataSourceName = dsName;
                GetDataSourceCredentials();
            }
            private void GetDataSourceCredentials()
            {
                MessageBox.Show(string.Format("Get {0}'s connection setting for {1} environment from config file", dataSourceName, environment));
            }
            public abstract void OpenAndReturnConnection();

            public abstract class gg { };

            public void methodGG()
            {

            }
        }

        public abstract class AbsClassV2
        {

        }

        public class MsSqlDataSource : DataSource
        {
            public MsSqlDataSource(string environment) : base(environment, "MsSQL")
            {
                MessageBox.Show("the constructor of MsSqlDataSource class executed");
            }
            public override void OpenAndReturnConnection()
            {
                MessageBox.Show(string.Format("Create and return Connection for {0} dataSource", dataSourceName));
            }
        }

        public class OracleDataSource : DataSource
        {
            //this one defines the overloaded contructor of base class
            public OracleDataSource(string environment) : base(environment, "Oracle")
            {
                MessageBox.Show("the constructor of OracleDataSource class executed");
            }
            public override void OpenAndReturnConnection()
            {
                MessageBox.Show(string.Format("Create and return Connection for {0} dataSource", dataSourceName));
            }
        }

        public class MYSQLDataSource : DataSource
        {
            public override void OpenAndReturnConnection()
            { }
            //this one defines the overloaded contructor of base class
            public MYSQLDataSource() : base("1", "2")
            {

            }
        }

        public class MYSQLDataSource2 : DataSource
        {
            //this one uses the default  contructor of base class
            public override void OpenAndReturnConnection()
            { }
        }

        private void btnAbstractExample_Click(object sender, RoutedEventArgs e)
        {
            MsSqlDataSource msDataSource = new MsSqlDataSource("ms sql environment");
            OracleDataSource oracleDataSource = new OracleDataSource("oracle sql environment");

            msDataSource.OpenAndReturnConnection();
            oracleDataSource.OpenAndReturnConnection();
        }


        //interface or abstract cannot be static

        //static interface IStaticInterface
        //{

        //}

        //static abstract class absClass
        //{

        //}


        //interface cannot have contructor but abstract classes can have

        //by default all members of interface is public and abstract 
        interface IFirstInterface
        {
            string srGoodCode { get; set; }
            void myMethod(); // interface method
        }

        interface ISecondInterface
        {
            int irMoneyPrize { get; set; }
            void myOtherMethod(); // interface method
        }

        //interface can not have non public members such as protected or private

        // Implement multiple interfaces
        //a class can implement multiple interfaces but not multiple abstract or real classes
        class DemoClass : IFirstInterface, ISecondInterface
        {
            public string srGoodCode { get; set; }//by default it is null
            public int irMoneyPrize { get; set; }

            public void myMethod()
            {
                MessageBox.Show("Some text..");
            }
            public void myOtherMethod()
            {
                Console.WriteLine("Some other text...");
            }
        }


        //so a concrete class can only have 1 base class (it can be a concrete class or abstract class) and have as many as interfaces
        //DataSource is the base class and it cannnot inherit more base classes
        //ISecondInterface and IFirstInterface are interfaces
        public class TestClass : DataSource, ISecondInterface, IFirstInterface
        {
            public TestClass(string environment) : base(environment, "MsSQL")
            {
                MessageBox.Show("the constructor of MsSqlDataSource class executed");
            }
            public override void OpenAndReturnConnection()
            {
                MessageBox.Show(string.Format("Create and return Connection for {0} dataSource", dataSourceName));
            }

            public string srGoodCode { get; set; }//by default it is null
            public int irMoneyPrize { get; set; }

            public void myMethod()
            {
                MessageBox.Show("Some text..");
            }
            public void myOtherMethod()
            {
                Console.WriteLine("Some other text...");
            }
        }

        private void btnPolimorphismVSinheriance_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in InhertianceVSPolymorphism.Test.Main())
            {
                listboxResults.Items.Add(item);
            }
        }

        private void btnMethodOverloading_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("first method overloading (static polimorphism) call with signature double + double: " + MethodOverLoading.Example(20, 20));

            MessageBox.Show("second method overloading call (static polimorphism) with signature double + double + integer: " + MethodOverLoading.Example(20, 20, 20));

            MethodOverriding.BaseMultiply baseMultiply = new MethodOverriding.BaseMultiply();
            double finalamount = baseMultiply.ExecuteExample(5000.00, 0.1);
            MessageBox.Show("base multiply execute result: " + finalamount);

            baseMultiply = new MethodOverriding.OverMultiply();
            finalamount = baseMultiply.ExecuteExample(5000.00, 0.1);
            MessageBox.Show("OverMultiply multiply execute result: " + finalamount);

            baseMultiply = new MethodOverriding.PlusMultiply();
            finalamount = baseMultiply.ExecuteExample(5000.00, 0.1);
            MessageBox.Show("PlusMultiply multiply execute result: " + finalamount);



            baseMultiply = new MethodOverriding.PlusMultiply();
            finalamount = baseMultiply.NoOverRide(100, 100);
            MessageBox.Show("PlusMultiply from base type NoOverRide execute result: " + finalamount);

            baseMultiply = new MethodOverriding.OverMultiply();
            finalamount = baseMultiply.NoOverRide(100, 100);
            MessageBox.Show("OverMultiply from base type NoOverRide execute result: " + finalamount);

            baseMultiply = new MethodOverriding.PlusMultiply();
            finalamount = baseMultiply.NoOverRide(100, 100);
            MessageBox.Show("PlusMultiply from base type NoOverRide execute result: " + finalamount);



            baseMultiply = new MethodOverriding.PlusMultiply();
            finalamount = baseMultiply.NewOverride(100, 100);
            MessageBox.Show("PlusMultiply NewOverride execute result: " + finalamount);

            baseMultiply = new MethodOverriding.OverMultiply();
            finalamount = baseMultiply.NewOverride(100, 100);
            MessageBox.Show("OverMultiply NewOverride execute result: " + finalamount);

            baseMultiply = new MethodOverriding.PlusMultiply();
            finalamount = baseMultiply.NewOverride(100, 100);
            MessageBox.Show("PlusMultiply NewOverride execute result: " + finalamount);



            MethodOverriding.OverMultiply OverMultiply = new MethodOverriding.OverMultiply();
            finalamount = OverMultiply.NoOverRide(100, 100);
            MessageBox.Show("OverMultiply from OverMultiply type NoOverRide execute result: " + finalamount);

            MethodOverriding.PlusMultiply PlusMultiply = new MethodOverriding.PlusMultiply();
            finalamount = PlusMultiply.NoOverRide(100, 100);
            MessageBox.Show("PlusMultiply from PlusMultiply type NoOverRide execute result: " + finalamount);

        }
    }
}
