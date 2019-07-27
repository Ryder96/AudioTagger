using AudioTagger.Bundle;
using AudioTagger.Network.ResponseDataJson;
using AudioTagger.Network.ResponseDataJson.AlbumInfo;
using AudioTagger.ui.Data;
using AudioTagger.ui.MVP.LastFM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AudioTagger.ui.MVVM.ModelView
{
    class LastFM_MV : INotifyPropertyChanged
    {
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


        public ILastFMModel Model { get; set; }

        public SongFile Song
        {
            get
            {
                return m_Song;
            }

            set
            {
                m_Song = value;
                Title = Song.Tag.Title ?? string.Empty;
                Artist = Song.Artist ?? string.Empty;
                Album = Song.Album ?? string.Empty;
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

        }

    }
}
