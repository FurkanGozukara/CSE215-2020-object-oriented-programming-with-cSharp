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


namespace Lecture_3___Part_2___WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            txtNewValueOfListBox.Visibility = Visibility.Hidden;
            btnSaveNewValueofListBox.Visibility = Visibility.Hidden;
        }

        private void btnJoinStrings_Click(object sender, RoutedEventArgs e)
        {
            List<string> lstInputs = new List<string> { txt1.Text, txt2.Text };

            var mergedInput = string.Join("\r\n\r\n", lstInputs);

            MessageBox.Show(mergedInput);


        }

        private void splitText_Click(object sender, RoutedEventArgs e)
        {
            int irInsertIndex = -1;

            if (Int32.TryParse(txtStarIndexofListBox.Text, out irInsertIndex) == false)
            {
                irInsertIndex = -1;
            }

            string myMainInput = txtSplitBox.Text;
            List<string> lstSplittedText = myMainInput.Split(txtSplitParameter.Text).ToList();

            foreach (var vrPerSplit in lstSplittedText)
            {
                if (irInsertIndex > -1)
                {
                    lstSplitResults.Items.Insert(irInsertIndex, vrPerSplit);
                }
                else
                    lstSplitResults.Items.Add(vrPerSplit);

            }
        }

        private void clearListBoxButton_Click(object sender, RoutedEventArgs e)
        {
            lstSplitResults.Items.Clear();
        }

        private void lstSplitResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtNewValueOfListBox.Visibility = Visibility.Visible;
            btnSaveNewValueofListBox.Visibility = Visibility.Visible;
        }

        private void btnSaveNewValueofListBox_Click(object sender, RoutedEventArgs e)
        {
            if (lstSplitResults.SelectedIndex > -1)
            {
                lstSplitResults.Items[lstSplitResults.SelectedIndex] = txtNewValueOfListBox.Text;
            }
            lstSplitResults.SelectedIndex = -1;
            txtNewValueOfListBox.Visibility = Visibility.Hidden;
            btnSaveNewValueofListBox.Visibility = Visibility.Hidden;
        }

        private void btnOpenSecondWindow_Click(object sender, RoutedEventArgs e)
        {
            SecondWindow mySecondWindow = new SecondWindow();
            
            mySecondWindow.Show();
        }
    }
}
