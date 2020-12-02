using System;
using System.Windows.Controls;

namespace exampleDLL
{
    public static class student_operations
    {
        public static double return_square_of_STID(int irStudentId)
        {
            return irStudentId * irStudentId;
        }
    }

    public class student_dll_project : ListBoxItem
    {
        public int irStudentId { get; set; }

        private string _srStudentName;

        public string srStudentName
        {
            get { return _srStudentName; }   // get method

            set//this is constructor customization  
            {
                _srStudentName = value;
                this.Content = _srStudentName;
            }  // set method          
        }
    }
}
