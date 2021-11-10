using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Threading;

namespace odtwarzacz
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        ObservableCollection<TrackLista> listaTrakow = new ObservableCollection<TrackLista>();
        CurPlay curPlay = new CurPlay();

       

        AudioPlayerLoadedBehavior loadedBehavior = new AudioPlayerLoadedBehavior();
        int NumerGranegoUtworu=1;
        public MainWindow()
        {
            
            InitializeComponent();
            
            audioPlayer.DataContext = loadedBehavior;
            playlista.ItemsSource = listaTrakow;
        }
        
        public string filename;

        


        private void Start_Click(object sender, RoutedEventArgs e)
        {
            loadedBehavior.LoadedBehavior = MediaState.Play;
            
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            loadedBehavior.LoadedBehavior = MediaState.Pause;
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            loadedBehavior.LoadedBehavior = MediaState.Stop;
        }

        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            if (listaTrakow.Count > 0)
            {
                if (NumerGranegoUtworu == 1)
                {
                    NumerGranegoUtworu = 2;
                }
                filename = listaTrakow[NumerGranegoUtworu - 2].SciezkaPliku;
                NumerGranegoUtworu -= 1;
                audioPlayer.Source = (new Uri(filename));
                loadedBehavior.LoadedBehavior = MediaState.Play;
                CurrentPlay.Text = listaTrakow[NumerGranegoUtworu - 1].Tytul;
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (listaTrakow.Count > 0)
            {
                if (NumerGranegoUtworu == playlista.Items.Count)
                {
                    NumerGranegoUtworu = playlista.Items.Count - 1;
                }
                filename = listaTrakow[NumerGranegoUtworu].SciezkaPliku;
                NumerGranegoUtworu += 1;
                audioPlayer.Source = (new Uri(filename));
                loadedBehavior.LoadedBehavior = MediaState.Play;
                CurrentPlay.Text = listaTrakow[NumerGranegoUtworu - 1].Tytul;
            }
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            int numer = 1;
            ofd.Multiselect = true;
            ofd.Filter = "Mp3 files (.mp3)|*.mp3";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            
            if (ofd.ShowDialog() == true)
            {
                
                foreach(string item in ofd.FileNames)
                {
                    var onlyFileName = System.IO.Path.GetFileNameWithoutExtension(item);
                    listaTrakow.Add(new TrackLista() { Numer = Convert.ToInt32(numer), Tytul = onlyFileName, SciezkaPliku=item });
                    numer++;
                    
                  
                }
                
            }
            NumerGranegoUtworu = 0;
            Next_Click(sender, e);
            
        }

        private void Playlista_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TrackLista tlist = (TrackLista)playlista.SelectedItem;      
            filename = tlist.SciezkaPliku;
            NumerGranegoUtworu = tlist.Numer;
            

            audioPlayer.Source = (new Uri(filename));
            loadedBehavior.LoadedBehavior = MediaState.Play;
            CurrentPlay.Text = listaTrakow[NumerGranegoUtworu - 1].Tytul;


        }

        private void AudioPlayer_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            MessageBox.Show(e.ErrorException.Message);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            audioPlayer.ScrubbingEnabled = true;
            loadedBehavior.LoadedBehavior = MediaState.Stop;
        }

        private void AudioPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            TimerSlider.Maximum = audioPlayer.NaturalDuration.TimeSpan.TotalSeconds;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();

            totalTimeOfAudio.Content = audioPlayer.NaturalDuration.TimeSpan.ToString(@"hh\:mm\:ss");
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            TimerSlider.Value = audioPlayer.Position.TotalSeconds;
        }

        private void TimerSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            audioPlayer.Position = TimeSpan.FromSeconds(TimerSlider.Value);
        }

        private void ProgressCircle_Loaded(object sender, RoutedEventArgs e)
        {

        }

        
    }

        

    
}
