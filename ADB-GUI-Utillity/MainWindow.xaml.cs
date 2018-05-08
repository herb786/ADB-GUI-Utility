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
using System.Reflection;
using System.Security.Principal;

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
        public const int OPT_ACTIVITY = 1;
        public const int OPT_SERVICE = 2;
        int adbIntent;
        IList<string> envs = new List<string>();

        public MainWindow()
        {
            InitializeComponent();

            txtBoxTarget.Clear();

            txtBoxPackage.Text = Properties.Settings.Default.Package;
            txtBoxTarget.Text = Properties.Settings.Default.Activity;
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
            radButService.IsChecked = true;

            string userprofile = Environment.ExpandEnvironmentVariables("%USERPROFILE%");
            txtHome.Text = userprofile + "\\AppData\\Local\\Android\\Sdk";
            txtNDK.Text = userprofile + "\\AppData\\Local\\Android\\Sdk\\ndk-bundle";
            txtTools.Text = userprofile + "\\AppData\\Local\\Android\\Sdk\\tools";
            txtPlatform.Text = userprofile + "\\AppData\\Local\\Android\\Sdk\\platform-tools";
            txtBuild.Text = userprofile + "\\AppData\\Local\\Android\\Sdk\\build-tools\\27.0.3";

        }

        private void Set_Paths(object sender, RoutedEventArgs e)
        {
            if (checkPlatform.IsChecked.Value) {
                envs.Add(txtPlatform.Text);
            }
            if (checkTools.IsChecked.Value)
            {
                envs.Add(txtTools.Text);          
            }
            if (checkBuild.IsChecked.Value)
            {
                envs.Add(txtBuild.Text);
            }
            add_env_path(String.Join(";", envs.ToArray()));
            if (checkHome.IsChecked.Value)
            {
                set_env_path("ANDROID_HOME", txtHome.Text);
            }
            if (checkNDK.IsChecked.Value)
            {
                set_env_path("ANDROID_NDK_HOME", txtNDK.Text);
            }
            //Application.Current.Shutdown();
            //System.Windows.Forms.Application.Restart();
          
        }

        private void set_env_path(string key, string value)
        {
            Process sett = new Process();
            sett.StartInfo.Verb = "runas";
            sett.StartInfo.UseShellExecute = false;
            sett.StartInfo.RedirectStandardOutput = true;
            sett.StartInfo.FileName = "setx";
            sett.StartInfo.Arguments = " \"" + key + "\" " + value + " /m";
            sett.Start();
            sett.WaitForExit();
        }

        private void add_env_path(string value)
        {
            string path = Environment.ExpandEnvironmentVariables("%PATH%");
            //string paths = " PATH2 \"%PATH%;" + value + "\" /m";
            string paths = path + ";" + value;
            Console.WriteLine(paths);
            /*
            Process sett = new Process();
            sett.StartInfo.Verb = "runas";
            sett.StartInfo.UseShellExecute = false;
            sett.StartInfo.RedirectStandardOutput = true;
            sett.StartInfo.FileName = "setx";
            sett.StartInfo.Arguments = path;
            sett.Start();
            sett.WaitForExit();
            */
            Environment.SetEnvironmentVariable("Path", paths, EnvironmentVariableTarget.Machine);
        }

        private void Activity_Checked(object sender, RoutedEventArgs e)
        {
            adbIntent = OPT_ACTIVITY;
        }

        private void Service_Checked(object sender, RoutedEventArgs e)
        {
            adbIntent = OPT_SERVICE;
        }

        private void Execute_Click(object sender, RoutedEventArgs e)
        {
            Process adb = new Process();
            // Redirect the output stream of the child process.
            adb.StartInfo.UseShellExecute = false;
            adb.StartInfo.RedirectStandardOutput = true;
            adb.StartInfo.FileName = "adb.exe";
            string args = " shell am startservice -n ";
            if (adbIntent == OPT_ACTIVITY)
            {
                args = " shell am start -n ";
            }
            args = args + txtBoxPackage.Text + "/" + txtBoxTarget.Text;
            args = args + " -a " + txtBoxAction.Text;
            if (combo1.SelectedIndex > -1  && adbIntent == OPT_SERVICE)
            {
                args = args + " " + coms[combo1.SelectedIndex] + " " + txtBoxName1.Text + " " + txtBoxVal1.Text;
            }
            if (combo2.SelectedIndex > -1 && adbIntent == OPT_SERVICE)
            {
                args = args + " " + coms[combo2.SelectedIndex] + " " + txtBoxName2.Text + " " + txtBoxVal2.Text;
            }
            if (combo3.SelectedIndex > -1 && adbIntent == OPT_SERVICE)
            {
                args = args + " " + coms[combo3.SelectedIndex] + " " + txtBoxName3.Text + " " + txtBoxVal3.Text;
            }
            adb.StartInfo.Arguments = args;
            adb.Start();
            MessageBox.Show("Done\nTip: Set export to True\n");
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Package = txtBoxPackage.Text;
            Properties.Settings.Default.Activity = txtBoxTarget.Text;
            Properties.Settings.Default.Action = txtBoxAction.Text;
            Properties.Settings.Default.Save();
        }

        private void Open_Path_Setup(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("control", "sysdm.cpl");
        }

        private void Open_Folder(object sender, RoutedEventArgs e)
        {
            string userprofile = Environment.ExpandEnvironmentVariables("%USERPROFILE%");
            string path = userprofile + "\\AppData\\Local\\Android\\Sdk\\platforms\\android-22\\data\\res";
            Console.WriteLine(path);
            Process.Start("explorer.exe", path);
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
