using AudioTagger.ui.Data;
using AudioTagger.ui.MVVM.Models;
using AudioTagger.ui.MVVM.Models.Interfaces;
using AudioTagger.ui.MVVM.ModelView;
using System;
using System.Collections.ObjectModel;
using System.IO;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace AudioTagger
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ListSongPage : Page
    {
        private ObservableCollection<SongFile> m_SongList;
        private FolderPicker_MV m_ModelView;

        public ListSongPage()
        {
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            this.InitializeComponent();
        }



        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter is FolderPicker_MV)
            {
                m_ModelView = e.Parameter as FolderPicker_MV;
                IFolderModel folderModel = new FolderModel();
                m_ModelView.FolderModel = folderModel;
                m_SongList = m_ModelView.SongList;
            }
            if (!m_ModelView.FolderOpened)
            {
                m_ModelView.OpenMusicFolder();
            }

            base.OnNavigatedTo(e);
        }
        
        private string FormatDataFromSong(SongFile song)
        {
            string info = "";
            info += "Title\n";
            info += song.Tag.Title + "\n";
            info += "Artist(s)\n";
            info += song.Artist + "\n";
            info += "Genre \n";
            foreach (string genre in song.Tag.Genres)
            {
                info += genre;
                info += " ";
            }
            info += "\n";
            info += "Track Number\tTrack count\n";
            info += song.Tag.Track + "\t" + song.Tag.TrackCount + "\n";
            info += "Disco number\tDisc total\n";
            info += song.Tag.Disc + "\t" + song.Tag.DiscCount + "\n";
            info += "Album name\n";
            info += song.Tag.Album + "\n";
            info += "Album Artist\n";
            foreach (string albumArtist in song.Tag.AlbumArtists)
            {
                info += albumArtist;
                info += " ";
            }
            info += "\n";
            info += "Year\n";
            info += song.Tag.Year + "\n";
            info += "Lyrics\n";
            info += song.Tag.Lyrics;
            return info;
        }

        private async void SongListView_SongOnClick(object sender, ItemClickEventArgs e)
        {
            SongFile songClicked = e.ClickedItem as SongFile;
            m_ModelView.Song = songClicked;
            InfoBox.Text = FormatDataFromSong(songClicked);
            if (songClicked.Tag.Pictures.Length == 0)
            {
                IEx_AlbumCover.Source = null;
                return;
            }
            else
            {
                MemoryStream stream = new MemoryStream(songClicked.Tag.Pictures?[0].Data.Data);
                BitmapImage image = new BitmapImage();
                await image.SetSourceAsync(stream.AsRandomAccessStream());
                songClicked.Image = image;
                IEx_AlbumCover.Source = image;
            }

        }
  
    }
}

