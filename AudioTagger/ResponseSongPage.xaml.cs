using AudioTagger.Bundle;
using AudioTagger.Network.ResponseDataJson;
using AudioTagger.ui.Data;
using AudioTagger.ui.MVP.LastFM;
using AudioTagger.ui.MVVM.ModelView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace AudioTagger
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ResponseSongPage : Page
    {
        private LastFM_MV m_ModelView;

        public ResponseSongPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            BackButton.IsEnabled = true;

            if (e.Parameter is LastFM_MV)
            {
                m_ModelView = e.Parameter as LastFM_MV;
                m_ModelView.Model = new LastFMModel();
                m_ModelView.PropertyChanged += M_ModelView_PropertyChanged;
                LW_Tracks.ItemsSource = m_ModelView.Tracks;
                m_ModelView.OnDisplay = true;
                m_ModelView.SongUpdated = false;
                UpdateCurrentInfo();
                UpdateSearchInfo();
                m_ModelView.SearchTrack();
            }
            base.OnNavigatedTo(e);
        }

        private void UpdateSearchInfo()
        {
            tb_SongTitle.Text = m_ModelView.Title;
            tb_SongArtist.Text = m_ModelView.Artist;
            tb_SongAlbum.Text = m_ModelView.Album;
        }

        private void UpdateCurrentInfo()
        {
            tb_CurrentTitle.Text = m_ModelView.Title;
            tb_CurrentArtist.Text = m_ModelView.Artist;
            tb_CurrentAlbum.Text = m_ModelView.Album ?? "No Album";
            IEX_CurrentCover.Source = m_ModelView.Song.Image;
        }

        private void UpdateNewInfoView()
        {
            tb_NewTitle.Text = m_ModelView.Title;
            tb_NewArtist.Text = m_ModelView.Artist;
            tb_NewAlbum.Text = m_ModelView.Album ?? "No Album";
            IEX_NewCover.Source = m_ModelView.Cover;
        }

        private void M_ModelView_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateNewInfoView();
        }

        private string InfoToString(IList<Track> tracks)
        {
            string info = "";
            foreach (Track track in tracks)
            {
                info += track.name + "\n" + track.artist + "\n";
            }
            return info;
        }

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is TrackBundle)
            {
                TrackBundle trackBundle = e.ClickedItem as TrackBundle;
                m_ModelView.UpdateTrackInfo(trackBundle);
                UpdateNewInfoView();
            }
        }

        private void CleanView()
        {
            tb_SongTitle.Text = string.Empty;
            tb_SongArtist.Text = string.Empty;
            tb_SongAlbum.Text = string.Empty;
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

        private void SearchSongRequested()
        {
            if (!tb_SongTitle.Text.Equals(string.Empty))
            {
                m_ModelView.Title = tb_SongTitle.Text;
                m_ModelView.Artist = tb_SongArtist.Text;
                m_ModelView.Tracks.Clear();
                m_ModelView.SearchTrack();
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            SearchSongRequested();
        }

        private void Grid_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                SearchSongRequested();
            }

        }

        private void Write_Tag(object sender, RoutedEventArgs e)
        {
            SaveTagFile();
        }

        private void SaveTagFile()
        {
            DataFile newTag = m_ModelView.Data();
            if (newTag == null)
            {
                DisplayAsyncDialog("Tag not applied");
                return;
            }
            newTag.FolderUrl = m_ModelView.CurrentFolder;
            newTag.ApplyTag();
            DisplayAsyncDialog("Successefully");
            m_ModelView.SongUpdated = true;
            On_BackRequested();

        }

        private void DisplayAsyncDialog(string result)
        {
            ContentDialog TagPrompDialog = new ContentDialog
            {
                Title = "Apply Tag",
                Content = result,
                CloseButtonText = "OK"
            };
            _ = TagPrompDialog.ShowAsync();
        }

    }
}
