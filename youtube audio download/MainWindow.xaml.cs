using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.IO;
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

namespace youtube_audio_download
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private string link = "";
        private string arg1 = "";
        private string arg2 = "";

        private readonly string[] argsList1 = {"bestaudio", "bestvideo"};
        private readonly string[] argsList2 = {"none", "best", "mp3", "aac"};

        public MainWindow()
        {
            InitializeComponent();

            formatSelect.ItemsSource = argsList1;
            formatSelect.SelectedIndex = 0;
            outputSelect.ItemsSource = argsList2;
            outputSelect.SelectedIndex = 0;
            arg1 = "-f " + formatSelect.SelectedItem;
            //arg2 = outputSelect.SelectedItem.ToString();
            outputTBox.IsReadOnly = true;
        }

        private void download(string arg1, string arg2)
        {
            Process process = new Process();

            process.StartInfo.FileName = Environment.CurrentDirectory + "\\thingythings\\youtube-dl.exe";
            process.StartInfo.Arguments = arg1 + arg2 + " " + link;

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.OutputDataReceived += process_OutputDataReceived;
            process.EnableRaisingEvents = true;
                
            process.Start();

            process.BeginOutputReadLine();

            //process.WaitForExit();
            //process.Close();

            process.Exited += process_HasExited;
        }

        private void process_HasExited(object sender, EventArgs e)
        {
            MessageBox.Show("DONE!", "Done", MessageBoxButton.OK);
        }

        private void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            this.Dispatcher.Invoke((Action) (() =>
            {
                outputTBox.AppendText(e.Data + "\n");
                outputTBox.ScrollToEnd();
            }));
            
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            download(arg1, arg2);
        }

        private void linkTBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            link = linkTBox.Text;
        }


        private void formatSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            arg1 = "-f " + formatSelect.SelectedItem;
        }

        private void outputSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (outputSelect.SelectedIndex != 0)
                arg2 = " -x --audio-format " + outputSelect.SelectedItem;
        }

    }
}
