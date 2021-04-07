using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;


namespace youtube_audio_download
{
    public partial class MainWindow : Window
    {

        private string link = "";
        private string arg1 = "";
        private string arg2 = "";
        private string arg3 = "";
        private string arg4 = "";


        private readonly string[] videoOutputArgs = {"Audio", "Video"};
        private readonly string[] audioOutputArgs= {"none", "best", "mp3", "aac"};
        private readonly string[] audioQualityArgs = {"320K", "256K", "192K", "128K", "96K", ""};



        public MainWindow()
        {
            InitializeComponent();

            formatSelect.ItemsSource = videoOutputArgs;
            formatSelect.SelectedIndex = 0;
            outputSelect.ItemsSource = audioOutputArgs;
            outputSelect.SelectedIndex = 2;
            qualitySelect.ItemsSource = audioQualityArgs;
            qualitySelect.SelectedIndex = 0;
            //arg1 = "-f " + formatSelect.SelectedItem;
            arg1 = " -f bestaudio";
            //arg2 = outputSelect.SelectedItem.ToString();
            outputTBox.IsReadOnly = true;
            path_textbox.Text = "";
        }

        private void download()
        {
            Process process = new Process
            {
                StartInfo =
                {
                    FileName = "youtube-dl.exe",
                    Arguments = "-o " + path_textbox.Text + " " + arg1 + arg2 + arg3 + " " + link,
                    //Arguments = "-o " + path_textbox.Text + "-f bestaudio -x --audio-format mp3 --audio-quality 9 " + link,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };


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
            this.Dispatcher.Invoke((Action)(() =>
            {
                outputTBox.AppendText(e.Data + "\n");
                outputTBox.ScrollToEnd();
            }));

        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            download();
        }

        private void linkTBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            link = linkTBox.Text;
        }


        private void formatSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //arg1 = "-i -v -f " + formatSelect.SelectedItem;
            if (formatSelect.SelectedIndex is 0)
                arg1 = " -f bestaudio";
            if (formatSelect.SelectedIndex is 1)
            {
                arg1 = " -f bestvideo";
                outputSelect.SelectedIndex = 0;
                qualitySelect.SelectedIndex = 5;
            }
        }

        private void outputSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (formatSelect.SelectedIndex is 0)
            {
                arg2 = " -x --audio-format " + outputSelect.SelectedItem;
                arg3 = " --audio-quality " + qualitySelect.SelectedItem;
            }

            if (outputSelect.SelectedIndex != 0)
            {
                arg3 = "";
                arg2 = "";
            }
        }

        private void qualitySelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (outputSelect.SelectedIndex != 0 || outputSelect.SelectedIndex != 1)
                arg3 = " --audio-quality " + qualitySelect.SelectedItem;
        }


        private void playlistCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            arg4 = " --no-playlist";
        }

        private void playlistCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            arg4 = " --yes-playlist";
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                DragMove();
        }

        private void X_Click(object sender, RoutedEventArgs e) => Close();

        private void browse_btn_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\Users";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                path_textbox.Text = dialog.FileName + "\\%%(title)s.%%(ext)s";
            }
        }
    }
}
