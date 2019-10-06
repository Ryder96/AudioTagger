
using AudioTagger.Network.ResponseDataXML;
using AudioTagger.ui.MVVM.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AudioTagger.ui.MVVM.ModelView
{
    class Lyrics_MV : INotifyPropertyChanged
    {
        private string title;
        private string artist;
        private string lyrics;
        private ObservableCollection<SearchLyricResult> lyricsList;
      
        public event PropertyChangedEventHandler PropertyChanged;

        public Lyrics_MV()
        {
            title = string.Empty;
            artist = string.Empty;
            lyrics = string.Empty;
            lyricsList = new ObservableCollection<SearchLyricResult>();
        }

        public ILyricsModel Model
        { get; set; }

        public string Title
        {
            get { return title; }
            set { if (!title.Equals(value)) { title = value; NotifyPropertyChanged(); } }
        }

        public string Artist
        {
            get { return artist; }
            set { if (!artist.Equals(value)) { artist = value; NotifyPropertyChanged(); } }
        }

        public string Lyrics
        {
            get { return lyrics; }
            set { if (!lyrics.Equals(value)) { lyrics = value; NotifyPropertyChanged(); } }
        }

        public ObservableCollection<SearchLyricResult> LyricList
        {
            get { return lyricsList; }
            set { if (lyricsList != value) { lyricsList = value; NotifyPropertyChanged(); } }
        }


        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async void SearchLyrics()
        {
            if (!artist.Equals(string.Empty) && !title.Equals(string.Empty))
            {
                var response = await Model.SearchLyrics(artist, title);
                foreach (var lyric in response.SearchLyricResult)
                {
                    LyricList.Add(lyric);
                }
            }
        }

        public async void GetLyric(int lyricID, string lyricChecksum)
        {
            if (lyricID <= 0 || lyricChecksum.Equals(string.Empty))
            {
                Lyrics = "Lyrics not available for this option";
            }
            else
            {
                var lyric = await Model.RetrieveLyrics(lyricID, lyricChecksum);
                Lyrics = lyric.Lyric;
            }
        }


        public void Clear()
        {
            Title = string.Empty;
            Artist = string.Empty;
            Lyrics = string.Empty;
            lyricsList.Clear();
        }

    }
}
