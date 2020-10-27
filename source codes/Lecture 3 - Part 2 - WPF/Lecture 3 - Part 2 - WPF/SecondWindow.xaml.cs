using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;
using System.Threading.Tasks;

namespace Lecture_3___Part_2___WPF
{
    /// <summary>
    /// Interaction logic for SecondWindow.xaml
    /// </summary>
    public partial class SecondWindow : Window
    {
        public SecondWindow()
        {
            InitializeComponent();
            cmSortOrder.Items.Add("select dictionary sorting order");
            cmSortOrder.Items.Add("order by keys ascending");
            cmSortOrder.Items.Add("order by keys descending");
            cmSortOrder.Items.Add("order by values ascending");
            cmSortOrder.Items.Add("order by values descending");
            cmSortOrder.SelectedIndex = 0;

            cmb_WritingMethod.Items.Add("select dictionary writing method");
            cmb_WritingMethod.Items.Add("use write all lines method");
            cmb_WritingMethod.Items.Add("use stream writer method");
            cmb_WritingMethod.Items.Add("use strinbuilder write all text method");
            cmb_WritingMethod.SelectedIndex = 0;
        }

        Dictionary<int, Int64> dicHoldNumbers = new Dictionary<int, long>();

        private void btnGenerateNumbers_Click(object sender, RoutedEventArgs e)
        {
            var vrId = Thread.CurrentThread.ManagedThreadId;
            Debug.WriteLine("CurrentThread id when generate button clicked = "+ vrId);
            Task.Factory.StartNew(() => {
                //whatever run here will be executed on the thread pool
                //therefore, whatever executed here will not block the main thread nor the UI (user interface)
                vrId = Thread.CurrentThread.ManagedThreadId;
                Debug.WriteLine("CurrentThread id in task factory = " + vrId);
                generateRandomNumbersMethod();

            });
           
        }

        private void generateRandomNumbersMethod()
        {
            var vrId = Thread.CurrentThread.ManagedThreadId;
            Debug.WriteLine("CurrentThread id inside generate random numbers method = " + vrId);

            Random randobj = new Random();

            for (int i = 0; i < 10000000; i++)
            {
                int irRand1 = randobj.Next();
                int irRand2 = randobj.Next();

                if (dicHoldNumbers.ContainsKey(irRand1))
                {
                    //these 2 are equal dicHoldNumbers[irRand1] = dicHoldNumbers[irRand1] + irRand2;
                    dicHoldNumbers[irRand1] += irRand2;
                }
                else
                    dicHoldNumbers.Add(irRand1, irRand2);

                if (irRand1 == 43)
                    break;
            }

            updateStatistics();
        }

        void updateStatistics()
        {
            var vrId = Thread.CurrentThread.ManagedThreadId;
            Debug.WriteLine("CurrentThread id when update statistics method called = " + vrId);
           var vrStatistics= calculateStatistics();
            Dispatcher.BeginInvoke(new Action(delegate()
            {
                vrId = Thread.CurrentThread.ManagedThreadId;
                Debug.WriteLine("thread id inside dispatcher = " + vrId);
                lstBoxStatiscs.Items.Clear();

                foreach (var vrPerStatistic in vrStatistics)
                {
                    lstBoxStatiscs.Items.Add(vrPerStatistic);
                }
            }));

         
        }

        private List<string> calculateStatistics()
        {
            List<string> lstStatistics = new List<string>();

            //these below are cpu intensive task and we took them to a sub task inside a different thread rather than the main thread
            //therefore no more user interface freeze / unresponsiveness
            lstStatistics.Add($"number of elements in the dictionary: {dicHoldNumbers.Count.ToString("N0")}");
            lstStatistics.Add($"first key in the dictionary: {dicHoldNumbers.Keys.FirstOrDefault().ToString("N0")}");
            lstStatistics.Add($"last key in the dictionary: {dicHoldNumbers.Keys.LastOrDefault().ToString("N0")}");
            lstStatistics.Add($"biggest key in the dictionary: {dicHoldNumbers.Keys.OrderByDescending(pr => pr).FirstOrDefault().ToString("N0")}");
            lstStatistics.Add($"smallest key in the dictionary: {dicHoldNumbers.Keys.OrderBy(pr => pr).FirstOrDefault().ToString("N0")}");
            lstStatistics.Add($"biggest value in the dictionary: {dicHoldNumbers.Select(pr => pr.Value).OrderByDescending(pr => pr).FirstOrDefault().ToString("N0")}");
            lstStatistics.Add($"smallest value in the dictionary: {dicHoldNumbers.Select(pr => pr.Value).OrderBy(pr => pr).FirstOrDefault().ToString("N0")}");
            lstStatistics.Add($"sum of the values in the dictionary: {dicHoldNumbers.Select(pr => pr.Value).Sum().ToString("N0")}");

            return lstStatistics;
        }



        private void btnSortdictionary_Click(object sender, RoutedEventArgs e)
        {
            switch (cmSortOrder.SelectedIndex)
            {
                case 0:
                    MessageBox.Show("no sorting option selected");
                    break;
                case 1:
                    dicHoldNumbers = dicHoldNumbers.OrderBy(pr => pr.Key)
                        .ToDictionary(pr => pr.Key, pr => pr.Value);
                    break;
                case 2:
                    dicHoldNumbers = dicHoldNumbers.OrderByDescending(pr => pr.Key)
                        .ToDictionary(pr => pr.Key, pr => pr.Value);
                    break;
                case 3:
                    dicHoldNumbers = dicHoldNumbers.OrderBy(pr => pr.Value)
                        .ToDictionary(pr => pr.Key, pr => pr.Value);
                    break;
                case 4:
                    dicHoldNumbers = dicHoldNumbers.OrderByDescending(pr => pr.Value)
                        .ToDictionary(pr => pr.Key, pr => pr.Value);
                    break;
            }
        }

        private void btnWriteDicToFile_Click(object sender, RoutedEventArgs e)
        {
            switch (cmb_WritingMethod.SelectedIndex)
            {
                case 0:
                    MessageBox.Show("no writing method is selected");
                    break;
                case 1:
                    Task.Factory.StartNew(() => { writeFileWithAllLines(); })
                        .ContinueWith(completed => executeFinalTask());
            
                    break;
                case 2:
                    Task.Factory.StartNew(() => { writeFileWithStreamWriter_no_auto_flush(); }).ContinueWith(completed => executeFinalTask()); 
                    Task.Factory.StartNew(() => { writeFileWithStreamWriter_auto_flush(); }).ContinueWith(completed => executeFinalTask());                 
                    break;
                case 3:
                    Task.Factory.StartNew(() => { writeStringBuilder(); }).ContinueWith(completed => executeFinalTask()); 
                
                    break;
                case 4:

                    break;
            }

       
        }

        private void executeFinalTask()
        {
            var vrExePath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Process.Start("explorer.exe", vrExePath);
            GC.Collect();
        }

        private void writeFileWithAllLines()
        {
            string srNameofFile = "dic_write_all_lines.txt";
            File.Delete(srNameofFile);
            Stopwatch swTimer = new Stopwatch();
            swTimer.Start();
            File.WriteAllLines(srNameofFile, dicHoldNumbers.Select(pr => (pr.Key + "\t" + pr.Value).ToString()).ToList());
            swTimer.Stop();

            updateListBox("writeFileWithAllLines took : " + swTimer.ElapsedMilliseconds.ToString("N0") + " ms",0);
        }

        private void updateListBox(string srMessage, int irIndex)
        {
            Dispatcher.BeginInvoke( new Action(delegate ()  {
                lstBoxStatiscs.Items.Insert(irIndex, srMessage);
            } ));
        }

        private void writeFileWithStreamWriter_no_auto_flush()
        {
            string srNameofFile = "dic_write_stream_writer_no_auto_flush.txt";
            File.Delete(srNameofFile);
            StreamWriter swWrite = new StreamWriter(srNameofFile);
            Stopwatch swTimer = new Stopwatch();
            swTimer.Start();
            foreach (var vrPerObject in dicHoldNumbers)
            {
                swWrite.WriteLine($"{vrPerObject.Key}\t{vrPerObject.Value}");
            }
            swWrite.Flush();
            swWrite.Close();
            swTimer.Stop();

            updateListBox("writeFileWithStreamWriter_no_auto_flush took : " + swTimer.ElapsedMilliseconds.ToString("N0") + " ms", 0);
        }

        private void writeFileWithStreamWriter_auto_flush()
        {
            string srNameofFile = "dic_write_stream_writer_auto_flush.txt";
            File.Delete(srNameofFile);
            StreamWriter swWrite = new StreamWriter(srNameofFile);
            swWrite.AutoFlush = true;
            Stopwatch swTimer = new Stopwatch();
            swTimer.Start();
            foreach (var vrPerObject in dicHoldNumbers)
            {
                swWrite.WriteLine($"{vrPerObject.Key}\t{vrPerObject.Value}");
            }
            swWrite.Flush();
            swWrite.Close();
            swTimer.Stop();

            updateListBox("writeFileWithStreamWriter_auto_flush took : " + swTimer.ElapsedMilliseconds.ToString("N0") + " ms", 0);
        }

        private void writeStringBuilder()
        {
            string srNameofFile = "dic_write_string_builder.txt";
            File.Delete(srNameofFile);
            StringBuilder srWritings = new StringBuilder();
            Stopwatch swTimer = new Stopwatch();
            swTimer.Start();
            foreach (var vrPerObject in dicHoldNumbers)
            {
                srWritings.AppendLine($"{vrPerObject.Key}\t{vrPerObject.Value}");
            }
            File.WriteAllText(srNameofFile, srWritings.ToString());
            swTimer.Stop();

            updateListBox("writeStringBuilder took : " + swTimer.ElapsedMilliseconds.ToString("N0") + " ms", 0);
        }
    }
}
