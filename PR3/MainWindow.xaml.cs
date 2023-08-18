using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PR3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MediaPlayer Player = new MediaPlayer();
        public DispatcherTimer Timer = new DispatcherTimer();
        int currentTrack = 0;

        string[] files;
        int mode = 0;
        string[] archiveFiles;

        bool ordered= false;
        

        public MainWindow()
        {
            InitializeComponent();

            Player.MediaEnded += IFComplete;
            Player.MediaOpened += MediaOpened;
            Timer.Tick += TrackPosition;
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();
        }

        public void MediaOpened(object sender, EventArgs e)
        {
            TrackTime.Maximum = Player.NaturalDuration.TimeSpan.Seconds;
        }
        public void TrackPosition(object sender, EventArgs e)
        {
            Label1.Content = Player.Position.ToString(@"mm\:ss");
            if(Player.HasAudio)
                TimeLeft.Content = TimeSpan.FromSeconds(Player.NaturalDuration.TimeSpan.Seconds - Player.Position.TotalSeconds).ToString(@"mm\:ss");
            TrackTime.Value = Player.Position.TotalSeconds;
        }

        public void IFComplete(object sender, EventArgs e)
        {
            if (mode == 1)
            {
                Player.Position = TimeSpan.Zero;
                Player.Play();
            }
            else
            {
                currentTrack++;
                if (currentTrack >= files.Length)
                    currentTrack = 0;

                Player.Open(new Uri(files[currentTrack]));
                Player.Play();
            }

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (mode == 0)
            {
                mode= 1;
                Repeat.Content = "Отключить повтор";
            }
            else
            {
                mode = 0;
                Repeat.Content = "Повтор";
            }
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {


            if (Start.Content == "Start")
            {
                Player.Play();
                Start.Content = "Stop";
            }
            else
            {
                Player.Pause();
                Start.Content = "Start";
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.ShowDialog();

            files = Directory.GetFiles(fbd.SelectedPath);
            archiveFiles = Directory.GetFiles(fbd.SelectedPath);

            Player.Open(new Uri(files[0]));
            Player.Play();
            Start.Content = "Stop";

           
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        { 
            currentTrack++;
            if(currentTrack >= files.Length)
                currentTrack= 0;
           
            Player.Open(new Uri(files[currentTrack]));
            Player.Play();

        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            currentTrack--;
            if (currentTrack < 0)
                currentTrack = files.Length - 1;

            Player.Open(new Uri(files[currentTrack]));
            Player.Play();

        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            if (ordered == false)
            {
                Random rnd = new Random();
                files = files.OrderBy(x => rnd.Next()).ToArray();
                ordered = true;
            }
            else
            {
                files = archiveFiles;
                ordered = false;
            }

            currentTrack = 0;
            Player.Open(new Uri(files[currentTrack]));
            Player.Play();

        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //TrackTime.Value
            Player.Position = TimeSpan.FromSeconds(e.NewValue);
        }
    }
}
