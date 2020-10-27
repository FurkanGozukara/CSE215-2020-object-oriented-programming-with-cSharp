using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
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

namespace Lecture_4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //very good article about reference types value types
        //heap stack garbage collector
        //https://www.c-sharpcorner.com/article/C-Sharp-heaping-vs-stacking-in-net-part-i/
        public MainWindow()
        {
            InitializeComponent();
        }

        //random is class therefore it is a reference type
        static Random myRand = new Random();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (GC.TryStartNoGCRegion(199999999, true))
            {
                try
                {
                    for (int i = 0; i < 100000000; i++)
                    {
                        //double is a struct therefore it is a value type
                        double dblVal = myRand.NextDouble();
                    }
                }
                finally
                {

                }
            }

           
        }

        public class myDoubleClas
        {
            public double dblVal;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (GC.TryStartNoGCRegion(199999999, true))
            {
                try
                {
                    for (int i = 0; i < 100000000; i++)
                    {
                        testReferenceTypeCall();
                    }
                }
                finally
                {
                   
                }
            }

          
        }

        private void testReferenceTypeCall()
        {
            myDoubleClas dblVal = new myDoubleClas { dblVal = myRand.NextDouble() };
        }
    }
}
