using AudioTagger.ui.Data;
using AudioTagger.ui.MVVM.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TagLib;
using Windows.Storage;

namespace AudioTagger.ui.MVVM.ModelView
{
    public class FolderPicker_MV : INotifyPropertyChanged
    {
        private bool m_FolderOpened = false;
        private IList<StorageFile> m_List;
        private string m_CWD;
        private SongFile m_Song;
        private ObservableCollection<SongFile> m_SongList;

        public event PropertyChangedEventHandler PropertyChanged;

        public FolderPicker_MV(IFolderModel model)
        {
            FolderModel = model;
            m_SongList = new ObservableCollection<SongFile>();
            CWD = string.Empty;
        }

        public FolderPicker_MV()
        {
            m_SongList = new ObservableCollection<SongFile>();
            m_CWD = string.Empty;
        }

        public bool FolderOpened
        {
            set
            {
                if (m_FolderOpened != value)
                {
                    m_FolderOpened = value;
                    NotifyPropertyChanged();
                }
            }
            get
            {
                return m_FolderOpened;
            }
        }

        public IList<StorageFile> FileList
        {
            set
            {
                m_List = value;
                NotifyPropertyChanged();
            }
            get
            {
                return m_List;
            }
        }

        public string CWD
        {
            get
            {
                return m_CWD;
            }
            set
            {
                m_CWD = value;
                NotifyPropertyChanged();
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
                if (m_Song != value)
                {
                    m_Song = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ObservableCollection<SongFile> SongList
        {
            set
            {
                m_SongList = value;
                NotifyPropertyChanged();
            }
            get
            {
                return m_SongList;
            }
        }

        public IFolderModel FolderModel { get; set; }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async void OpenMusicFolder()
        {
            try
            {
                m_List = await FolderModel.OpenFolder();
                CWD = FolderModel.PathFolder();
                FolderOpened = true;
                UpdateSongList(m_List);
            }
            catch (Exception) { }
        }

        private async void UpdateSongList(IList<StorageFile> list)
        {
            m_SongList.Clear();
            foreach (StorageFile file in list)
            {
                try
                {
                    SongFile song = await ReadTagFromFile(file);
                    m_SongList.Add(song);
                }
                catch (CorruptFileException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }

            }
        }

        private async Task<SongFile> ReadTagFromFile(StorageFile file)
        {
            Stream fileStream = await file.OpenStreamForReadAsync();
            TagLib.File tagFile = TagLib.File.Create(new StreamFileAbstraction(file.Name, fileStream, fileStream));
            Tag tag = null;
            if (tagFile.MimeType == "taglib/mp3")
            {
                tag = tagFile.GetTag(TagTypes.Id3v2);
            }
            if (tagFile.MimeType == "taglib/flac")
            {
                tag = tagFile.GetTag(TagTypes.FlacMetadata);
            }
            string performers = "";
            foreach (string performer in tag.Performers)
            {
                performers += performer;
            }
            tagFile.Dispose();
            fileStream.Close();
            return new SongFile
            {
                FileName = file.Name,
                Title = tag.Title,
                Album = tag.Album,
                Tag = tag,
                Artist = performers,
                Path = file.Path
            };
        }


        public async void RefreshFolder()
        {
            try
            {
                m_List.Clear();
                m_SongList.Clear();
                m_List = await FolderModel.OpenFolder(m_CWD);
                UpdateSongList(m_List);
            }
            catch (Exception) { }
        }
    }
}
