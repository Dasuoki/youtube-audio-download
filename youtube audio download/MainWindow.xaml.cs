using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;


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
        private string arg3 = "";

        private readonly string[] videoOutputArgs = {"bestaudio", "bestvideo"};
        private readonly string[] audioOutputArgs= {"none", "best", "mp3", "aac"};
        private readonly string[] audioQualityArgs = {"320K", "256K", "192K", "128K", "96K"};


        public MainWindow()
        {
            InitializeComponent();

            formatSelect.ItemsSource = videoOutputArgs;
            formatSelect.SelectedIndex = 0;
            outputSelect.ItemsSource = audioOutputArgs;
            outputSelect.SelectedIndex = 0;
            qualitySelect.ItemsSource = audioQualityArgs;
            arg1 = "-f " + formatSelect.SelectedItem;
            //arg2 = outputSelect.SelectedItem.ToString();
            outputTBox.IsReadOnly = true;
        }

        private void download(string arg1, string arg2)
        {
            Process process = new Process();

            process.StartInfo.FileName = "youtube-dl.exe";
            process.StartInfo.Arguments = arg1 + arg2 + arg3 + " " + link;

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
            if (outputSelect.SelectedIndex == 0)
                arg3 = "";
        }

        private void qualitySelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (outputSelect.SelectedIndex != 0 || outputSelect.SelectedIndex != 1)
                arg3 = " --audio-quality " + qualitySelect.SelectedItem;
        }
    }
}
