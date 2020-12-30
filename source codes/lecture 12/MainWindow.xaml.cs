using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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
using static lecture_12.customEncryption;


namespace lecture_12
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MainWindow runningWindowRef;

        public MainWindow()
        {
            InitializeComponent();
            runningWindowRef = this;
        }

        private void btnThread_Click(object sender, RoutedEventArgs e)
        {
            Thread mythread = new Thread(new ParameterizedThreadStart(updateClockonScreen));
            mythread.Start(this);
        }

        private static void updateClockonScreen(object _runningWindow)
        {
            MainWindow runningWindow = (MainWindow)_runningWindow;

            while (true)
            {
                runningWindow.lblUpdate.Dispatcher.BeginInvoke(new Action(() =>
                {
                    runningWindow.lblUpdate.Content = DateTime.Now.TimeOfDay;
                }));

                System.Threading.Thread.Sleep(100);
            }
        }

        private void btnDataRaceThreading_Click(object sender, RoutedEventArgs e)
        {
            int irMyNumber = 250432203;
            Task.Factory.StartNew(() => { displayNumber(irMyNumber); });//tasks and threads doesnt block flow of your thread. therefore, this line is immediately executed and then the thread continues
            //however, at a later sometime, the task starts
            irMyNumber = 10;
            //data racing happens and 10 is displayed https://stackoverflow.com/questions/13776846/pass-parameters-through-data
        }

        private static bool blParameterPassed = false;
        private static readonly object objParameterPassLock = new object();

        private static void displayNumber(int irNumber)
        {
            var localVar = irNumber;
            lock (objParameterPassLock)
                blParameterPassed = true;
            runningWindowRef.lblUpdate.Dispatcher.BeginInvoke(new Action(() =>
            {
                runningWindowRef.lblUpdate.Content = localVar.ToString("N0");
            }));
            while (true)
            {
                System.Threading.Thread.Sleep(1);
            }
        }

        private void btnPreventDataRace_Click(object sender, RoutedEventArgs e)
        {
            lock (objParameterPassLock)
                blParameterPassed = false;
            int irMyNumber = 250432203;
            Task vrTask = Task.Factory.StartNew(() =>
            {
                displayNumber(irMyNumber);
            });
            // vrTask.Wait();//this will still block the parent thread until the task completely executed. therefore in this case our application would freeze
            while (true)
            {
                lock (objParameterPassLock)
                    if (blParameterPassed == true)
                        break;
                System.Threading.Thread.Sleep(1);
            }
            irMyNumber = 10;
        }

        // taking a class 
        public class P { }

        // taking a class 
        // derived from P 
        public class P1 : P { }

        // taking a class 
        public class P2 { }
        //https://www.geeksforgeeks.org/is-vs-as-operator-keyword-in-c-sharp/



        private void btnIsOperator_Click(object sender, RoutedEventArgs e)
        {
            // creating an instance 
            // of class P 
            P o1 = new P();

            // creating an instance 
            // of class P1 
            P1 o2 = new P1();

            // checking whether 'o1' 
            // is of type 'P' 
            Debug.WriteLine(o1 is P);

            // checking whether 'o1' is 
            // of type Object class 
            // (Base class for all classes) 
            Debug.WriteLine(o1 is Object);

            // checking whether 'o2' 
            // is of type 'P1' 
            Debug.WriteLine(o2 is P1);

            // checking whether 'o2' is 
            // of type Object class 
            // (Base class for all classes) 
            Debug.WriteLine(o2 is Object);

            // checking whether 'o2' 
            // is of type 'P' 
            // it will return true as P1 
            // is derived from P 
            Debug.WriteLine(o2 is P1);

            // checking whether o1 
            // is of type P2 
            // it will return false 
            Debug.WriteLine(o1 is P2);

            // checking whether o2 
            // is of type P2 
            // it will return false 
            Debug.WriteLine(o2 is P2);

            // checking whether o1 (P) 
            // is of type P1 
            // it will return false 
            Debug.WriteLine(o1 is P1);

            // checking whether o2 (P1)
            // is of type P
            // it will return true 
            Debug.WriteLine(o2 is P);
        }

        class Y { }
        class Z { }

        private void btnAsOperator_Click(object sender, RoutedEventArgs e)
        {
            object[] o = new object[5];
            o[0] = new Y();//Y
            o[1] = new Z();//Z
            o[2] = "Hello";//string
            o[3] = 4759.0;//double
            o[4] = null;

            for (int q = 0; q < o.Length; ++q)
            {
                // using as operator 
                string str1 = o[q] as string;

                Debug.WriteLine(o[q]?.ToString());

                // checking for the result 
                if (str1 != null)
                {
                    Debug.WriteLine("'" + str1 + "'");

                }
                else
                {
                    Debug.WriteLine("Is is not a string");
                }
            }
        }
        //https://www.geeksforgeeks.org/c-sharp-type-casting/
        private void btnTypeCasting_Click(object sender, RoutedEventArgs e)
        {
            int i = Int32.MaxValue;

            // automatic type conversion implict
            long l = i;

            // automatic type conversion 
            float f = l;

            // Display Result 
            Debug.WriteLine("Int value " + i);
            Debug.WriteLine("Long value " + l);
            Debug.WriteLine("Float value " + f);


            l = long.MaxValue;

            f = l;


            Debug.WriteLine("Long value " + l);
            Debug.WriteLine("Float value " + f);

            double d = 765.12;

            // Incompatible Data Type 
            // i = d;//this gives error cannot implicty convert double to int

            double gg = i;

            double ga = 765.99;// equal to ceiling 
            var v1 = Math.Ceiling(ga);
            Debug.WriteLine($"765.99 Math.Ceiling {v1}");
            var v2 = Math.Floor(ga);
            Debug.WriteLine($"765.99 Math.Floor {v2}");
            // Explicit Type Casting 
            int ii = (int)d;

            // Display Result 
            Debug.WriteLine("Value of i is " + ii);

            ii = (int)ga;

            Debug.WriteLine("Value of i is " + ii);

            ga = double.MaxValue;


            ii = (int)ga;

            Debug.WriteLine("Value of i is " + ii);

            int ia = 12;
            double daa = 765.12;
            float faa = 56.123F;

            // Using Built- In Type Conversion 
            // Methods & Displaying Result 
            Debug.WriteLine(Convert.ToString(faa));
            Debug.WriteLine(Convert.ToInt32(daa));
            Debug.WriteLine(Convert.ToUInt32(faa));
            Debug.WriteLine(Convert.ToDouble(ia));

        }

        //https://www.c-sharpcorner.com/article/implement-symmetric-and-asymmetric-cryptography-algorithms-with-c-sharp/
        private readonly static string myCustomKey1 = "test key example";

        private void btnAsymmetricEncryption_Click(object sender, RoutedEventArgs e)
        {

            //Encrypt and export public and private keys
            var rsa1 = new RSACryptoServiceProvider();
            string privateKey = rsa1.ToXmlString(true);   // server side export
            string publicKey = rsa1.ToXmlString(false);   // server side export
            byte[] toEncryptData = Encoding.ASCII.GetBytes("hello world");
            byte[] encryptedRSA = rsa1.Encrypt(toEncryptData, false);
            string EncryptedResult = Encoding.Default.GetString(encryptedRSA);

            //Decrypt using exported keys
            var rsa2 = new RSACryptoServiceProvider();
            rsa2.FromXmlString(privateKey);
            byte[] decryptedRSA = rsa2.Decrypt(encryptedRSA, false);
            string originalResult = Encoding.Default.GetString(decryptedRSA);
        }

        private void btnSymmetricEncryption_Click(object sender, RoutedEventArgs e)
        {
            SymmetricAlgorithm aes = new AesManaged();

            aes.Key = myCustomKey1.toByteArray().to32Bytes();

            string message = "my special text to be sent to school";

            // Call the encryptText method to encrypt the a string and save the result to a file   
            EncryptText(aes, message, "encryptedData.txt");
            SymmetricAlgorithm aes2 = new AesManaged();
            var vrKey = "some other key".toByteArray().to32Bytes();
            aes2.Key = vrKey;

            //var vrDecryptedData = DecryptData(aes2, "encryptedData.txt");//this causes error

            var vrDecryptedData = DecryptData(aes, "encryptedData.txt");



        }
    }
}
