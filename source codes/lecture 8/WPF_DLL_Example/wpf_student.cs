using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace WPF_DLL_Example
{
    public class student : ListBoxItem
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
