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
            Random randobj = new Random();

            for (int i = 0; i < 100000; i++)
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
            lstBoxStatiscs.Items.Clear();
            lstBoxStatiscs.Items.Add($"number of elements in the dictionary: {dicHoldNumbers.Count.ToString("N0")}");
            lstBoxStatiscs.Items.Add($"first key in the dictionary: {dicHoldNumbers.Keys.FirstOrDefault().ToString("N0")}");
            lstBoxStatiscs.Items.Add($"last key in the dictionary: {dicHoldNumbers.Keys.LastOrDefault().ToString("N0")}");
            lstBoxStatiscs.Items.Add($"biggest key in the dictionary: {dicHoldNumbers.Keys.OrderByDescending(pr => pr).FirstOrDefault().ToString("N0")}");
            lstBoxStatiscs.Items.Add($"smallest key in the dictionary: {dicHoldNumbers.Keys.OrderBy(pr => pr).FirstOrDefault().ToString("N0")}");
            lstBoxStatiscs.Items.Add($"biggest value in the dictionary: {dicHoldNumbers.Select(pr => pr.Value).OrderByDescending(pr => pr).FirstOrDefault().ToString("N0")}");
            lstBoxStatiscs.Items.Add($"smallest value in the dictionary: {dicHoldNumbers.Select(pr => pr.Value).OrderBy(pr => pr).FirstOrDefault().ToString("N0")}");
            lstBoxStatiscs.Items.Add($"sum of the values in the dictionary: {dicHoldNumbers.Select(pr => pr.Value).Sum().ToString("N0")}");
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
                    writeFileWithAllLines();
                    break;
                case 2:
                    writeFileWithStreamWriter_no_auto_flush();
                    writeFileWithStreamWriter_auto_flush();
                    break;
                case 3:
                    writeStringBuilder();
                    break;
                case 4:

                    break;
            }

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
            lstBoxStatiscs.Items.Insert(0, "writeFileWithAllLines took : " + swTimer.ElapsedMilliseconds.ToString("N0") + " ms");
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
            lstBoxStatiscs.Items.Insert(0, "writeFileWithStreamWriter_no_auto_flush took : " + swTimer.ElapsedMilliseconds.ToString("N0") + " ms");
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
            lstBoxStatiscs.Items.Insert(0, "writeFileWithStreamWriter_auto_flush took : " + swTimer.ElapsedMilliseconds.ToString("N0") + " ms");
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
            lstBoxStatiscs.Items.Insert(0, "writeStringBuilder took : " + swTimer.ElapsedMilliseconds.ToString("N0") + " ms");
        }
    }
}
