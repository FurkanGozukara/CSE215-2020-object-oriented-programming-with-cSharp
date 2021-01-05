using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using static lecture_13.csHelperMethods;

namespace lecture_13
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

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            using (DBCrawling db = new DBCrawling())
            {
                db.tblMainUrls.RemoveRange(db.tblMainUrls);
                db.SaveChanges();

                db.tblMainUrls.Add(new tblMainUrl { Url = "www.toros.edu.tr", ParentUrlHash = "www.toros.edu.tr", SourceCode = "gg", UrlHash = "ww" });
                db.SaveChanges();
            }
        }


        private void clearDBandStart(object sender, RoutedEventArgs e)
        {
           // clearDatabase();
            crawlPage(txtInputUrl.Text, 0, txtInputUrl.Text,DateTime.Now);
        }
    }
}
