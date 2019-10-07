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
                m_ModelView.PropertyChanged += ModelView_PropertyChanged;
                m_ModelView.OnDisplay = true;
                m_ModelView.SongUpdated = false;
                UpdateCurrentInfo();
                UpdateSearchInfo();
                m_ModelView.SearchTrack();
                LW_Tracks.ItemsSource = m_ModelView.Tracks;
            }
            base.OnNavigatedTo(e);
        }

        private void UpdateSearchInfo()
        {
            tb_SongTitle.Text = m_ModelView.File.Title;
            tb_SongArtist.Text = m_ModelView.File.Artist;
            tb_SongAlbum.Text = m_ModelView.File.Album ?? "No Album";
        }

        private void UpdateCurrentInfo()
        {
            tb_CurrentTitle.Text = m_ModelView.File.Title;
            tb_CurrentArtist.Text = m_ModelView.File.Artist;
            tb_CurrentAlbum.Text = m_ModelView.File.Album ?? "No Album";
            IEX_CurrentCover.Source = m_ModelView.File.Image;
        }

        private void UpdateNewInfoView()
        {
            tb_NewTitle.Text = m_ModelView.Bundle.Title;
            tb_NewArtist.Text = m_ModelView.Bundle.Artist;
            tb_NewAlbum.Text = m_ModelView.Bundle.Album ?? "No Album";
            IEX_NewCover.Source = m_ModelView.Bundle.Image;
        }

        private void ModelView_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Bundle"))
            {
                UpdateNewInfoView();
            }
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
                m_ModelView.SearchTrack(tb_SongTitle.Text, tb_SongArtist.Text);
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
            DataFile newTag = m_ModelView.GenerateData();
            if (newTag == null)
            {
                DisplayAsyncDialog("Tag not applied");
                return;
            }
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
