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
using System.Diagnostics;

using System.Runtime.InteropServices;

namespace ADB_GUI_Utillity
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string[] coms = { "-es", "-ez", "-ei", "-el", "-ef" };
        string[] names = { "ExtraString", "ExtraBool", "ExtraInt", "ExtraLong", "ExtraFloat" };

        public MainWindow()
        {
            InitializeComponent();

            txtBoxTarget.Clear();

            txtBoxPackage.Text = Properties.Settings.Default.Package;
            txtBoxPackage.Text = Properties.Settings.Default.Activity;
            string action = Properties.Settings.Default.Action;
            if (action.Length > 0)
            {
                txtBoxAction.AppendText(action);
            }
            else
            {
                txtBoxAction.Text = "com.google.android.c2dm.intent.RECEIVE";
            }
 
         
            txtBoxName1.AppendText("");
            txtBoxVal1.AppendText("");

            combo1.ItemsSource = names;
            combo2.ItemsSource = names;
            combo3.ItemsSource = names;

        }

        private void Execute_Click(object sender, RoutedEventArgs e)
        {
            Process adb = new Process();
            // Redirect the output stream of the child process.
            adb.StartInfo.UseShellExecute = false;
            adb.StartInfo.RedirectStandardOutput = true;
            adb.StartInfo.FileName = "adb.exe";
            string args = " shell am startservice -n ";
            args = args + txtBoxPackage.Text + "/" + txtBoxTarget.Text;
            args = args + " -a " + txtBoxAction.Text;
            if (combo1.SelectedIndex > -1)
            {
                args = args + " " + coms[combo1.SelectedIndex] + " " + txtBoxName1.Text + " " + txtBoxVal1.Text;
            }         
            adb.StartInfo.Arguments = args;
            adb.Start();
            MessageBox.Show("Done\nTip: Set export to True\n");
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Package = txtBoxPackage.Text;
            Properties.Settings.Default.Activity = txtBoxPackage.Text;
            Properties.Settings.Default.Action = txtBoxAction.Text;
            Properties.Settings.Default.Save();
        }

    }

    public class Bundle
    {
        public string name;
        public string command;

        public Bundle(string tname, string tcommand)
        {
            name = tname;
            command = tcommand;
        }
    }
}
