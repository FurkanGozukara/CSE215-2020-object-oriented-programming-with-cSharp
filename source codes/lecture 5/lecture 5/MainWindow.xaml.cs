using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace lecture_5
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

        List<int> lstMyMainList = new List<int> { 10, 20, 30, 40, 50 };

        [Serializable]
        public class myTestClass
        {
            public int irTestVal = 0;
            public string srName = "furkan";
            public string srLocation = "mersin";
            public List<int> lstCollection = new List<int> { 3, 2, 100 };
            private string srPrivateVal = "private value";
            protected string srProtectedValue = "protected value";

            public myTestClass deepCopy()
            {
                myTestClass tempObject = new myTestClass();//composes a new instance of object from myTestClass
                tempObject.irTestVal = this.irTestVal;//value type
                tempObject.srName = this.srName;//immutable
                tempObject.srLocation = this.srLocation;//immutable
                tempObject.lstCollection = this.lstCollection.ToList();//i am making a deep copy here with ToList method call

                return tempObject;
            }
        }

        private void btnShallowCloning_Click(object sender, RoutedEventArgs e)
        {
            List<int> lstSecondList = lstMyMainList;

            lstSecondList[2] = 400;

            //the below foreaches are equal, therefore they will throw error
            //collection is modified during enumaration error

            //foreach (var vrSecondList in lstSecondList)
            //{
            //    lstMyMainList.Add(vrSecondList);
            //}

            //foreach (var vrSecondList in lstMyMainList)
            //{
            //    lstMyMainList.Add(vrSecondList);
            //}

            //when assign reference type objects it just copies their address
            //any changes on them modified the same data

            myTestClass test = new myTestClass();
            test.irTestVal = 100;

            myTestClass test2 = test;
            test2.irTestVal = 200;

            //now both test and test2 irTestVal is 200

            int ir1 = 100;
            int ir2 = ir1;
            ir1 = 200;
            //now ir1 = 200 and ir2= 100 because these are value types 

            string sr1 = "hello world";
            string sr2 = sr1;
            sr1 = "hello home";
            //now sr1 is hello home and sr2 is hello world
            //string is only exception of shallow copy of class type
            //because
            //Strings in C# are immutable; that means they cannot change. When you say s = "hello home" you are creating a new string and assigning its reference to sr1; this does not affect the reference saved to sr2 which still points to the original string.

            //since string is immutable, whenever you concatenate  a string, it composes a new string. therefore, for long strings you should use stringbuilder
        }

        private void refreshListBox()
        {
            listBoxResults.Items.Clear();
            foreach (var vrSecondList in lstMyMainList)
            {
                listBoxResults.Items.Add(vrSecondList);
            }
        }

        private void btnRefreshListBox_Click(object sender, RoutedEventArgs e)
        {
            refreshListBox();
        }



        private void btnDeepCopy_Click(object sender, RoutedEventArgs e)
        {
            List<int> lstSecondList = lstMyMainList.ToList();

            lstSecondList[2] = 400;

            //this will work since they are different lists now
            foreach (var vrSecondList in lstSecondList)
            {
                lstMyMainList.Add(vrSecondList);
            }

            myTestClass myDefaultTestClass = new myTestClass();
            myTestClass myTest2 = new myTestClass
            {
                irTestVal = 250,
                lstCollection = new List<int> { 500, 1000, 10 },
                srLocation = "Turkey",
                srName = "Toros University"
            };

            //how can i make a deep copy of myTest2 into myDefaultTestClass??

            myDefaultTestClass = myTest2;// how it copies? this is only shallow cloning

            myDefaultTestClass.lstCollection[2] = 1;
            myTest2.lstCollection[1] = 3;
            myDefaultTestClass.srLocation = "default location of myDefaultTestClass";
            myTest2.srLocation = "default location of myTest2";

            myDefaultTestClass = myTest2.deepCopy();

            myDefaultTestClass.lstCollection[2] = 800;
            myTest2.lstCollection[1] = 5000;
            myDefaultTestClass.srLocation = "new location of myDefaultTestClass";
            myTest2.srLocation = "great location of myTest2";

            myTestClass test3 = myTest2.DeepClone();
            myTest2.lstCollection[2] = 1800;
            test3.lstCollection[1] = 15000;
            myTest2.srLocation = "awesome location of myDefaultTestClass";
            myTest2.srLocation = "super location of myTest2";
        }

        private void btnRefParamDifference_Click(object sender, RoutedEventArgs e)
        {
            int irMyVal = 100;
            int irMyVal_ref_example = 100;
            string srFirst = "";
            string sr_string_ref_example = "";
            List<int> lstMyValTest = new List<int>();
            List<int> lstMyValTest_2 = new List<int>();
            List<int> lst_non_ref_listExample = new List<int>();
            List<int> lst_ref_listExample = new List<int>();
            for (int i = 0; i < 100; i++)
            {
                increment(irMyVal, srFirst, lstMyValTest, lst_non_ref_listExample);

                increment_by_ref(ref irMyVal_ref_example, ref sr_string_ref_example, lstMyValTest_2, ref lst_ref_listExample);
            }

            anotherRefExample(lst_non_ref_listExample, ref lst_ref_listExample);
        }

        private void anotherRefExample(List<int> lstTemp,ref List<int> lstnonRefExample)
        {
            List<int> lstMyTempList = new List<int> { 11, 22, 33 };
            lstTemp = lstMyTempList;//lstTemp is also a copy but a copy of reference
            //therefore, after method return, all modifications to this copy of reference
            //is lost
            lstnonRefExample = lstMyTempList;// however this stays because when you use
            //ref word, not anymore copy but original value or reference is used
        }

        private void increment(int irVal, string srmyString, List<int> lstTemp, List<int> lstnonRefExample)
        {
            irVal++;
            srmyString += irVal + "\t";
            lstTemp.Add(irVal);
            lstnonRefExample = new List<int> { };
            lstnonRefExample.AddRange(lstTemp);
            lstnonRefExample.Add(irVal*irVal);
        }

        private void increment_by_ref(ref int irVal, ref string srmyString, List<int> lstTemp, ref List<int> lstRefListExample)
        {
            irVal++;
            srmyString += irVal + "\t";
            lstTemp.Add(irVal);
            lstRefListExample = new List<int> { };
            lstRefListExample.AddRange(lstTemp);
            lstRefListExample.Add(irVal * irVal);
        }

        public class my_test_class_inheritance
        {
            //default constructor is like this if you dont define any
            public my_test_class_inheritance()
            {
             
            }

            //this is the constructor of the my_test_class_inheritance
            public my_test_class_inheritance(int _irTestVal, string _srName)
            {
                irTestVal = _irTestVal;
                srName = _srName;
            }

            public int irTestVal = 0;
            public string srName = "furkan";
            public string srLocation = "mersin";
            public List<int> lstCollection = new List<int> { 3, 2, 100 };
            private string srPrivateVal = "private value";
            protected string srProtectedValue = "protected value";

            public myTestClass deepCopy()
            {
                myTestClass tempObject = new myTestClass();//composes a new instance of object from myTestClass
                tempObject.irTestVal = this.irTestVal;//value type
                tempObject.srName = this.srName;//immutable
                tempObject.srLocation = this.srLocation;//immutable
                tempObject.lstCollection = this.lstCollection.ToList();//i am making a deep copy here with ToList method call

                return tempObject;
            }
        }

     
        public class test_inherited : my_test_class_inheritance
        {
            public test_inherited(int _irTestVal, string _srName, string _location) 
                : base (_irTestVal, _srName)
            {
                irTestVal = _irTestVal;
                srName = _srName;
                srLocation = _location;
            }

            public test_inherited()
            {

            }

            //the child class is test_inherited
            //the parent class is my_test_class_inheritance

            //the base class is my_test_class_inheritance
            //the derived class is test_inherited
            public void setValues()
            {
                this.irTestVal = 10000;
                this.lstCollection = new List<int> { 250, 550, 750 };
            }

            private string srPrivateoftest_inherited = "test test_inherited";
        }

        public class child_of_inherited : test_inherited
        {
            public string srChildofAChild = "";

            public child_of_inherited()
            {

            }

            public child_of_inherited(int _irTestVal, string _srName, string _location)
                : base(_irTestVal, _srName, _location)
            {

            }

            //the child class is child_of_inherited
            //the parent class is test_inherited

            //the base class is my_test_class_inheritance
            //the derived class is child_of_inherited
            public void setNewValues()
            {
                this.setNewValues();
                this.irTestVal = 1000000;
            }

            public void setGreatValues()
            {
       
                this.irTestVal = 10000000;
            }
        }

        public static class static_base_class
        {
          public  static int irA, irB;
            static static_base_class()
            {
                irA = 25;
                irB = 50;
            }
        }

        //this below one throws compile error
        //public static class child_static_class : static_base_class
        //{

        //}

        private void btnSaveClassState_Click(object sender, RoutedEventArgs e)
        {
            my_test_class_inheritance my_base_class = new my_test_class_inheritance();

            my_test_class_inheritance my_base_class_2 =
                new my_test_class_inheritance(56,"custom name");

            test_inherited firstChildClass = new test_inherited();

            test_inherited firstChildClass_v2 = new test_inherited(56, "custom name","custom location");

            child_of_inherited child_of_a_Child = new child_of_inherited();

            child_of_inherited child_of_a_Child_v2 = new child_of_inherited(56, "custom name", "custom location");
        }
    }
}
