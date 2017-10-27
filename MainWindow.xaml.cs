using System;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace WatchDog
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

        private string AppToWatch
        { get; set; }

        private string AppToStart
        { get; set; }

        private int notrespondingcount
        { get; set; }
        
        private System.Timers.Timer aTimer
        { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AppToWatch = process.Text;
            AppToStart = path.Text;

            aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            aTimer.Interval = 1000;
            aTimer.Enabled = true;

            (sender as Button).IsEnabled = false;
        }

        /// <summary>
        /// Event for timer
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                bool find = false;
                Process[] processlist = Process.GetProcesses();
                foreach (Process theprocess in processlist)
                {
                    if (theprocess.ProcessName == AppToWatch)
                    {
                        find = true;
                        if (theprocess.Responding)
                        {
                            notrespondingcount = 0;
                        }
                        else
                        {
                            notrespondingcount++;
                            if (notrespondingcount == 5)
                            {
                                theprocess.Kill();
                                Process newProcess = new Process();
                                newProcess.StartInfo.FileName = AppToStart;
                                newProcess.Start();

                            }
                        }
                    }

                }
                if (find == false)
                {
                    Process newProcess = new Process();
                    newProcess.StartInfo.FileName = AppToStart;
                    newProcess.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error" + ex.Message, "Error");
            }

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this, "Written by kan", "About");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
