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
using static lecture_6.staticMethods;
using System.IO;
using Newtonsoft.Json;

namespace lecture_6
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

        //field versus properties : https://stackoverflow.com/questions/295104/what-is-the-difference-between-a-field-and-a-property

        private void btnComposeCarObjects_Click(object sender, RoutedEventArgs e)
        {
            cars car_1 = new cars();

            cars car_2 = new cars { irCarPrice = 250, irCarWeight = 250, srCarBrand = "BMW", srCarColor = "Red", srCarModel = "BMW 228", irProductionYear = 1825, lstRepairYears = new List<int> { 1775, 1900, 1645, 1925, 1999 } };

            car_2.saveToAFile("car2.txt");

            //MessageBox.Show("production year " + car_2.irProductionYear);

            cars car_3 = new cars();

            car_3.irCarPrice = 5000;
            car_3.irCarWeight = 2500;
            car_3.irProductionYear = 2000;
            car_3.srCarBrand = "Ford";
            car_3.srCarColor = "White";
            car_3.srCarModel = "Ford Escape Hybrid";
            car_3.lstRepairYears = new List<int>();
            car_3.lstRepairYears.Add(1950);
            car_3.lstRepairYears.Add(2005);


            List<cars> lstMyCars = new List<cars> { car_1, car_2, car_3 };

            lstMyCars.saveToFileCarsList("cars_list.txt");

            File.WriteAllText(@"cars_list_json.txt", JsonConvert.SerializeObject(lstMyCars));
        }

        private void btnTestExceptions_Click(object sender, RoutedEventArgs e)
        {
            int ir1 = "sadasd1".toInt();
            int ir2 = "2".toInt();
            int ir3 = "2453465467546356563".toInt();
        }

        private void btnLoadCarFromFile_Click(object sender, RoutedEventArgs e)
        {
            cars myCar2 = new cars();
            myCar2.loadFromFile("car2.txt");

            List<cars> lstMyCars = new List<cars>();

            lstMyCars.loadCarsFromFile("cars_list.txt");

            List<cars> lstMyCars_json = new List<cars>();

            lstMyCars_json = JsonConvert.DeserializeObject<List<cars>>(File.ReadAllText("cars_list_json.txt"));
        }

        private void btnTestReadOnlyandConstant_Click(object sender, RoutedEventArgs e)
        {
            //this below would throw error because you cant change value of readonly object after intilization or constructor of static class
            //srSplitValueSeperator = "custom stuff";

            //this below would throw error because you cant change value of constant object after first definition
            //irConstNum = 200;

            //cars.srCarManufacturer this would throw error
            cars myTempCar = new cars();
            var carm = myTempCar.srCarManufacturer;

            var carm2 = cars.srCarManufacturer_v2;

            var carm3 = cars.srCarManufacturer_v3;

            cars.srCarManufacturer_v3 = "Global v2";

            //constant vs readonly https://www.pluralsight.com/guides/const-vs-readonly-vs-static-csharp
        }
    }
}
