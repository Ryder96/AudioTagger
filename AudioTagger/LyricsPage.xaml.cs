using AudioTagger.Network.ResponseDataXML;
using AudioTagger.ui.MVVM.Models;
using AudioTagger.ui.MVVM.ModelView;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AudioTagger
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LyricsPage : Page
    {
        private Lyrics_MV m_ModelView;
        public LyricsPage()
        {
            this.NavigationCacheMode = NavigationCacheMode.Disabled;
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter is Lyrics_MV)
            {
                this.m_ModelView = e.Parameter as Lyrics_MV;
                m_ModelView.Model = new LyricsModel();
                LW_Lyrics.ItemsSource = m_ModelView.LyricList;
                SubscribeToEvents();
                m_ModelView.SearchLyrics();
            }

            base.OnNavigatedTo(e);

        }

        private void SubscribeToEvents()
        {
            m_ModelView.PropertyChanged += M_ModelView_PropertyChanged;
        }

        private void M_ModelView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName.Equals("Lyrics"))
            {
                LyricsTB.Text = m_ModelView.Lyrics;
            }
        }

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            if(e.ClickedItem is SearchLyricResult)
            {
                var result = e.ClickedItem as SearchLyricResult;
                m_ModelView.GetLyric(result.LyricId, result.LyricChecksum);
            }
        }

        private void BackButtonOnClick(object sender, RoutedEventArgs e)
        {
            On_BackRequested();
        }

        private bool On_BackRequested()
        {
            if (this.Frame.CanGoBack)
            {
                m_ModelView.Clear();
                this.Frame.GoBack();
                return true;
            }
            return false;
        }
    }
}
