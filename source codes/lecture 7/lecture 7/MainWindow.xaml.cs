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
using System.IO;
using System.Diagnostics;
using static lecture_7.generic_extensions;

namespace lecture_7
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

        private void btnGenerateTextFile_Click(object sender, RoutedEventArgs e)
        {
            StreamWriter swWriteNumbers = new StreamWriter("numbers.txt");
            swWriteNumbers.AutoFlush = true;
            Random myRand = new Random();

            for (int i = 0; i < Int32.MaxValue; i++)
            {
                swWriteNumbers.WriteLine(myRand.Next() + ";" + myRand.Next() + ";" + myRand.Next());
            }

            swWriteNumbers.Close();
        }

        private void btnFirstTimeCallStatic_Click(object sender, RoutedEventArgs e)
        {
            double dblVar = static_variables.dicValues.FirstOrDefault().Value;
        }

        private void btnCallStaticClassMethod_Click(object sender, RoutedEventArgs e)
        {
            var vrDouble = "321312".ToDouble();
        }

        List<non_static_variables> myList = new List<non_static_variables>();

        private void btnNonStaticClassExample_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {

                composeNonStaticObjects();

            });
        }

        private void composeNonStaticObjects()
        {
            Stopwatch sTimer = new Stopwatch();
            sTimer.Start();
            non_static_variables myobj = new non_static_variables();
            sTimer.Stop();

            //this just runs and inside of begin invoke is not any synchronous with the thread
            Dispatcher.BeginInvoke(new Action(delegate ()
            {
                lstResults.Items.Add(DateTime.Now + "\tcompose time 1: " + sTimer.ElapsedMilliseconds.ToString("N0") + "ms");
            }));


            sTimer.Start();
            myobj = new non_static_variables();
            sTimer.Stop();

            var vrTime = sTimer.ElapsedMilliseconds.ToString("N0");

            //this ensure that thread is paused until invoke is completed
            Dispatcher.BeginInvoke(new Action(delegate ()
            {
                Debug.WriteLine(ToDetailedDate() + " before lst result");
                lstResults.Items.Add(DateTime.Now + "\tcompose time 2: " + vrTime + "ms");
                Debug.WriteLine(ToDetailedDate() + " after lst result");
            }));

            Debug.WriteLine(ToDetailedDate() + " before reset");
            sTimer.Reset();
            Debug.WriteLine(ToDetailedDate() + " before after");

            sTimer.Start();
            myobj = new non_static_variables();
            sTimer.Stop();

            Dispatcher.BeginInvoke(new Action(delegate ()
            {
                lstResults.Items.Add(DateTime.Now + "\tcompose time 3: " + sTimer.ElapsedMilliseconds.ToString("N0") + "ms");
            }));


            myList.Add(myobj);
        }

        private void btnCallGarbageCollector_Click(object sender, RoutedEventArgs e)
        {
            GC.Collect();
        }

        private void btnRemoveList_Click(object sender, RoutedEventArgs e)
        {
            myList.Clear();
        }

        private void btnInvoke_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {

                Stopwatch swTime = new Stopwatch();
                swTime.Start();

                for (int i = 0; i < 100000000; i++)
                {
                    Dispatcher.Invoke(new Action(delegate ()
                    {
                        lblInvoke.Content = $"{i.ToString("N0")}\telapsed time: {swTime.ElapsedMilliseconds.ToString("N0")}";
                    }));
                }
            });
        }

        private void btnBeginInvoke_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {

                Stopwatch swTime = new Stopwatch();
                swTime.Start();

                for (int i = 0; i < 100000000; i++)
                {
                    if (i % 1000 == 0)
                        Dispatcher.BeginInvoke(new Action(delegate ()
                        {
                            lblInvoke.Content = $"{i.ToString("N0")}\telapsed time: {swTime.ElapsedMilliseconds.ToString("N0")}";
                        }));
                }
            });
        }
    }
}
