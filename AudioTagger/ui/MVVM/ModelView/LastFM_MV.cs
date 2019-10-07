using AudioTagger.Bundle;
using AudioTagger.Network.ResponseDataJson;
using AudioTagger.ui.Data;
using AudioTagger.ui.MVP.LastFM;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AudioTagger.ui.MVVM.ModelView
{
    class LastFM_MV : INotifyPropertyChanged
    {
        private SongFile file;
        private string currentFolder;
        private bool songUpdated;
        private bool onDisplay;
        private TrackBundle bundle;
        private ObservableCollection<TrackBundle> tracks;
        public event PropertyChangedEventHandler PropertyChanged;
        
        public ILastFMModel Model { get; set; }

        public LastFM_MV()
        {
            onDisplay = false;
            currentFolder = string.Empty;
            songUpdated = false;
            tracks = new ObservableCollection<TrackBundle>();
        }

        public SongFile File
        {
            get { return file; }
            set { if (file != value) { file = new SongFile(value); }  }
        }

        public string Folder
        {
            get { return currentFolder; }
            set { if (!currentFolder.Equals(value)) { currentFolder = value; } }
        }

        public bool OnDisplay
        {
            get { return onDisplay; }
            set { if (value != onDisplay) { onDisplay = value; NotifyPropertyChanged(); } }
        }

        public bool SongUpdated
        {
            get { return songUpdated; }
            set { if (value != songUpdated) { songUpdated = value; NotifyPropertyChanged(); } }
        }

        public TrackBundle Bundle
        {
            get { return bundle; }
            set { if (bundle != value) { bundle = value; NotifyPropertyChanged(); } }
        }
        public ObservableCollection<TrackBundle> Tracks
        {
            get { return tracks; }
            set { if (tracks != value) { tracks = value; NotifyPropertyChanged(); } }
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public DataFile GenerateData()
        {
            return new DataFile()
            {
                Title = bundle.Title,
                Artist = bundle.Artist,
                Album = bundle.Album,
                CoverUrl = bundle.Image,
                FileName = File.FileName,
                FolderUrl = Folder
            };
        }

        public async void SearchTrack()
        {
            if (File.Title.Equals(string.Empty) || File.Artist.Equals(string.Empty))
            {
                return;
            }
            var response = await Model.SearchTrack(File.Title, File.Artist);
            ConvertToBundle(response.results.trackmatches.track);

        }

        public async void SearchTrack(string title, string artist)
        {
            var response = await Model.SearchTrack(title, artist);
            Tracks.Clear();
            ConvertToBundle(response.results.trackmatches.track);

        }

        private void ConvertToBundle(IList<Track> tracks)
        {
            foreach (var track in tracks)
            {
                TrackBundle bundle = new TrackBundle()
                {
                    Title = track.name,
                    Artist = track.artist
                };
                if (track.image != null)
                {
                    foreach (var img in track.image)
                    {
                        if (img.size.Equals("extralarge") ||
                            img.size.Equals("large") ||
                            img.size.Equals("medium"))
                        {
                            bundle.Image = img.text;
                        }
                    }
                }
                SearchInfoTrack(bundle);
                bundle.PropertyChanged += Bundle_PropertyChanged;
                Tracks.Add(bundle);
            }
        }

        private void Bundle_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            int index = Tracks.IndexOf(sender as TrackBundle);
            Tracks[index] = sender as TrackBundle;
            /*
            Tracks.RemoveAt(index);
            Tracks.Insert(index,sender as TrackBundle);
            */
        }

        private async void SearchInfoTrack(TrackBundle bundle)
        {
            var response = await Model.RequestInfoTrack(bundle.Title, bundle.Artist);
            if (response.Track.album != null)
            {
                bundle.Album = response.Track.album.title;
                FilterImage(bundle, response.Track.album.image);
            }
            else
            {
                bundle.Album = string.Empty;
            }
            RetrieveCoverAlbum(bundle);
        }

        private async void RetrieveCoverAlbum(TrackBundle track)
        {
            if (!track.Album.Equals(string.Empty))
            {
                var response = await Model.RequestInfoAlbum(track.Album, track.Artist);
                if (response.album != null && response.album.image != null)
                {
                    FilterImage(track, response.album.image);
                }
            }

        }

        private void FilterImage(TrackBundle track, IList<Image> images)
        {
            if (images != null)
            {
                foreach (var img in images)
                {
                    if (img.size.Equals("extralarge") ||
                        img.size.Equals("large") ||
                        img.size.Equals("medium"))
                    {
                        track.Image = img.text;
                    }
                }
            }
        }

        public void UpdateTrackInfo(TrackBundle bundleClicked)
        {
            bundle = bundleClicked;
        }
        public void Clear()
        {
            Tracks?.Clear();
            OnDisplay = false;
        }
    }
}

/*
 private bool m_Display;
        private ObservableCollection<TrackBundle> m_TrackCollection;
        private string m_Title;
        private string m_Artist;
        private string m_Album;
        private string m_Info;
        private string m_Cover;
        private SongFile m_Song;
        private string m_CurrentFolder;
        private bool m_SongUpdated;

        public event PropertyChangedEventHandler PropertyChanged;

        public LastFM_MV()
        {
            m_Title = string.Empty;
            m_Artist = string.Empty;
            m_Album = string.Empty;
            m_Info = string.Empty;
            m_Cover = string.Empty;
            m_Display = false;
            m_TrackCollection = new ObservableCollection<TrackBundle>();
            m_CurrentFolder = string.Empty;
            m_SongUpdated = false;
        }


        public ILastFMModel Model { get; set; }

        public string Title
        {
            get
            {
                return m_Title;
            }
            set
            {
                if (!m_Title.Equals(value))
                {
                    m_Title = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public DataFile Data()
        {
            if(m_Title.Equals(string.Empty))
            {
                return null;
            }

            return new DataFile()
            {
                Title = this.Title,
                Artist = this.Artist,
                Album = this.Album,
                CoverUrl = this.Cover,
                FileName = Song.FileName
            };

        }

        public string Album
        {
            get
            {
                return m_Album;
            }
            set
            {
                if (!m_Album.Equals(value))
                {
                    m_Album = value ?? string.Empty;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Info
        {
            get
            {
                return m_Info;
            }
            set
            {
                if (!m_Info.Equals(value))
                {
                    m_Info = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Artist
        {
            get
            {
                return m_Artist;
            }
            set
            {
                if (!m_Artist.Equals(value))
                {
                    m_Artist = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Cover
        {
            get { return m_Cover; }
            set
            {
                if (!m_Cover.Equals(value))
                {
                    m_Cover = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ObservableCollection<TrackBundle> Tracks
        {
            set
            {
                if (m_TrackCollection != value)
                {
                    m_TrackCollection = value;
                    NotifyPropertyChanged();
                }
            }
            get
            {
                return m_TrackCollection;
            }
        }

        public bool OnDisplay
        {
            get { return m_Display; }
            set
            {
                if (value != m_Display)
                {
                    m_Display = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool SongUpdated
        {
            get { return m_SongUpdated; }
            set
            {
                if (value != m_SongUpdated)
                {
                    m_SongUpdated = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string CurrentFolder
        {
            get { return m_CurrentFolder; }
            set
            {
                if(!m_CurrentFolder.Equals(value))
                {
                    m_CurrentFolder = value;
                    NotifyPropertyChanged();
                }
            }
        }


        public SongFile Song
        {
            get
            {
                return m_Song;
            }

            set
            {
                m_Song = value;
                Title = Song.Tag.Title ?? "No Title";
                Artist = Song.Artist ?? "No Artist";
                Album = Song.Album ?? "No Album";
            }
        }



        public void Clear()
        {
            m_Title = string.Empty;
            m_Artist = string.Empty;
            m_Album = string.Empty;
            m_Info = string.Empty;
            m_Cover = string.Empty;
            m_TrackCollection?.Clear();
            OnDisplay = false;
        }


        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async void SearchTrack()
        {
            UpdateTrackSearchList(await Model.SearchTrack(Title, Artist));
        }

        public async void SearchTrackInfo(string mbid)
        {
            //UpdateTrackInfo(await m_Model.RequestInfoTrack(mbid));
        }

        public async void SearchTrackInfo(string title, string artist)
        {
            TrackInfo info = await Model.RequestInfoTrack(title, artist);
        }

        private async void CreateBundle(string mbid, string image)
        {
            TrackBundle bundle = null;
            TrackInfo info = await Model.RequestInfoTrack(mbid);
            if (info.Track != null)
            {
                bundle = new TrackBundle(info.Track);
                await SearchAlbumCover(bundle, mbid, string.Empty, string.Empty);
                if (bundle.Image.Equals(string.Empty))
                {
                    bundle.Image = image;
                }
            }

            if (bundle != null)
            {
                Tracks.Add(bundle);
            }

        }


        private async void CreateBundle(string artist, string name, string image)
        {
            TrackBundle bundle = null;
            TrackInfo info = await Model.RequestInfoTrack(name, artist);
            if (info.Track != null)
            {
                bundle = new TrackBundle(info.Track);
                await SearchAlbumCover(bundle, string.Empty, artist, name);
                if (bundle.Image.Equals(string.Empty))
                {
                    bundle.Image = image;
                }
            }

            if (bundle != null)
            {
                Tracks.Add(bundle);
                if (Tracks.Count == 1)
                {
                    UpdateTrackInfo(bundle);
                }
            }
        }

        private string FillCoverImage(IList<Network.ResponseDataJson.Image> list)
        {

            if (list != null && list.Count != 0)
            {
                foreach (Network.ResponseDataJson.Image image in list)
                {
                    if (image.size.Equals("large"))
                    {
                        return image.text ?? string.Empty;
                    }
                }
            }
            return string.Empty;
        }


        private async Task SearchAlbumCover(TrackBundle bundle, string mbid, string artist, string albumName)
        {
            AlbumInfoResponse albumInfo;
            if (!mbid.Equals(string.Empty))
            {
                albumInfo = await Model.QueryInfoAlbum(mbid);
            }
            else
            {
                albumInfo = await Model.QueryInfoAlbum(artist, albumName);
            }
            if (albumInfo != null)
            {
                if (albumInfo.album != null)
                {
                    foreach (Network.ResponseDataJson.AlbumInfo.Image img in albumInfo.album.image)
                    {
                        if (img.size.Equals("mega") || img.size.Equals("large"))
                        {
                            bundle.Image = img.text;
                        }
                    }
                }
            }
        }

        private void UpdateTrackSearchList(TrackSearch response)
        {
            string tempImage;
            if (response.results != null)
            {
                foreach (Network.ResponseDataJson.Track track in response.results.trackmatches.track)
                {
                    tempImage = FillCoverImage(track.image);
                    if (!track.mbid.Equals(string.Empty))
                    {
                        CreateBundle(track.mbid, tempImage);
                    }
                    else
                    {
                        CreateBundle(track.artist, track.name, tempImage);
                    }
                }
            }

        }


        public void UpdateTrackInfo(TrackBundle bundle)
        {
            Title = bundle.Title;
            Artist = bundle.Artist;
            Album = bundle.Album ?? "";
            Info = bundle.ToString();
            Cover = bundle.Image;

        }*/
