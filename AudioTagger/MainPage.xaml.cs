using System;
using AudioTagger.ui.Data;
using AudioTagger.ui.MVVM.ModelView;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AudioTagger
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    ///


    public sealed partial class MainPage : Page
    {

        private FolderPicker_MV m_FolderPickerMV;
        private LastFM_MV m_LastFMMV;
        private Lyrics_MV m_LyricsMV;
        private SongFile m_SongSelected;
        private string m_CurrenFolder;
        private bool m_FindTagFrameOpened;

        public MainPage()
        {
            m_SongSelected = null;
            m_FolderPickerMV = new FolderPicker_MV();
            m_LastFMMV = new LastFM_MV();
            m_LyricsMV = new Lyrics_MV();
            SubscribeToEvents();
            this.InitializeComponent();
        }

        private void SubscribeToEvents()
        {
            m_FolderPickerMV.PropertyChanged += M_FolderPickerMV_PropertyChanged;
            m_LastFMMV.PropertyChanged += M_LastFMMV_PropertyChanged;
            m_LyricsMV.PropertyChanged += M_LyricsMV_PropertyChanged;
        }

        private void M_LyricsMV_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            
        }

        private void M_LastFMMV_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName.Equals("OnDisplay"))
            {
                m_FindTagFrameOpened = m_LastFMMV.OnDisplay;
            }
            if(e.PropertyName.Equals("SongUpdated"))
            {
                if (m_LastFMMV.SongUpdated)
                {
                    m_FolderPickerMV.RefreshFolder();
                }
            }
        }

        private void M_FolderPickerMV_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName.Equals("Song"))
            {
                m_SongSelected = m_FolderPickerMV.Song;
                m_LastFMMV.Song = m_FolderPickerMV.Song;

                m_LyricsMV.Title = m_SongSelected?.Title ?? "No Title";
                m_LyricsMV.Artist = m_SongSelected?.Artist ?? "No title";
            }
            if(e.PropertyName.Equals("CWD"))
            {
                m_CurrenFolder = m_FolderPickerMV.CWD;
                m_LastFMMV.CurrentFolder = m_CurrenFolder;
            }
        }

        private void PickFolderOnClick(object sender, RoutedEventArgs e)
        {
            m_FolderPickerMV.FolderOpened = false;
            this.ResponseFrame.Navigate(typeof(ListSongPage), m_FolderPickerMV);
        }

        private void FindTagOnClick(object sender, RoutedEventArgs e)
        {
            if (!m_FindTagFrameOpened && m_SongSelected != null)
            {
                this.ResponseFrame.Navigate(typeof(ResponseSongPage), m_LastFMMV);
            }
        }

        private void FindLyricsOnClick(object sender, RoutedEventArgs e)
        {
            if (m_SongSelected != null)
            {
                this.ResponseFrame.Navigate(typeof(LyricsPage), m_LyricsMV);
            }
        }

    }





}


/*
 *   TagLib.Id3v2.Tag.DefaultVersion = 3
        TagLib.Id3v2.Tag.ForceDefaultVersion = True

        'get the mp3 file
        Dim mp3 As TagLib.File = TagLib.File.Create("D:\Towers Of Dub.mp3")
        'create the picture for the album cover
        Dim picture As TagLib.Picture = TagLib.Picture.CreateFromPath("D:\UfOrb.jpg")
        'create Id3v2 Picture Frame
        Dim albumCoverPictFrame As New TagLib.Id3v2.AttachedPictureFrame(picture)
        albumCoverPictFrame.MimeType = System.Net.Mime.MediaTypeNames.Image.Jpeg
        'set the type of picture (front cover)
        albumCoverPictFrame.Type = TagLib.PictureType.FrontCover

        'Id3v2 allows more than one type of image, just one needed
        Dim pictFrames() As TagLib.IPicture = {albumCoverPictFrame}
        mp3.Tag.Pictures = pictFrames 'set the pictures in the tag
        mp3.Save()
 * 
 *
 */
