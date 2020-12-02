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
using exampleDLL;
using WPF_DLL_Example;

namespace lecture_8
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            listBoxResults.DisplayMemberPath = "srStudentName";
            listBoxResults.SelectedValuePath = "irStudentId";

            txtStudentId.LostFocus += TxtStudentId_LostFocus;

            txtStudentName.GotFocus += TxtStudentName_GotFocus;
            txtStudentName.LostFocus += TxtStudentName_LostFocus;
            //these are event bindings
            listBoxResults.MouseDoubleClick += ListBoxResults_MouseDoubleClick;

            //https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/
        }

        private void ListBoxResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var senderListbox = (ListBox)sender;
            var myStudentObject = (student)senderListbox.SelectedItem;
            MessageBox.Show($"selected student {myStudentObject.srStudentName} has student id: {myStudentObject.irStudentId} - square of student id: {student_operations.return_square_of_STID(myStudentObject.irStudentId)}");


        }

        //Base Class (parent) is ListBoxItem
        //Derived Class(child) is student
        //we have moved this class into our WPF_DLL_Example project
        //public class student : ListBoxItem
        //{
        //    public int irStudentId { get; set; }

        //    private string _srStudentName;

        //    public string srStudentName
        //    {
        //        get { return _srStudentName; }   // get method

        //        set//this is constructor customization  
        //        {
        //            _srStudentName = value;
        //            this.Content = _srStudentName;
        //        }  // set method          
        //    }
        //}

        private void btnAddListbox_Click(object sender, RoutedEventArgs e)
        {
            int irStudentId = 0;

            try
            {
                irStudentId = Convert.ToInt32(txtStudentId.Text);

                if (irStudentId < 1)
                {
                    MessageBox.Show("you can not enter a negative integer as a student id");
                    return;
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("please enter a valid integer value for the student id");
                return;
            }
            catch (OverflowException)
            {
                MessageBox.Show("please enter a lower value. your value is exceeding " + Int32.MaxValue);
                return;
            }

            bool isIntString = txtStudentName.Text.Any(char.IsNumber);

            if (isIntString == true)
            {
                MessageBox.Show("student name cannot contain numbers");
                return;
            }

            student mytestStudent = new student { irStudentId = irStudentId, srStudentName = txtStudentName.Text };
            mytestStudent.Foreground = Brushes.Red;
            mytestStudent.Background = Brushes.Green;
            mytestStudent.FontStyle = FontStyles.Italic;
            mytestStudent.FontSize = 22.1;
            listBoxResults.Items.Add(mytestStudent);
        }

        //this is how to obtain place holder feature with got focus lost focus
        private void TxtStudentName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtStudentName.Text == "student name")
                txtStudentName.Text = "";
        }

        private void TxtStudentName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtStudentName.Text.Trim()))
                txtStudentName.Text = "student name";
        }

        private void TxtStudentId_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtStudentId.Text.Trim()))
                txtStudentId.Text = "student id";
        }

        private void txtStudentId_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtStudentId.Text == "student id")
                txtStudentId.Text = "";
        }
    }
}
