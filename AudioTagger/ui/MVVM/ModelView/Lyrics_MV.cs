
using AudioTagger.Network.ResponseDataXML;
using AudioTagger.ui.MVVM.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AudioTagger.ui.MVVM.ModelView
{
    class Lyrics_MV : INotifyPropertyChanged
    {
        private string title;
        private string artist;
        private string lyrics;
        private ObservableCollection<SearchLyricResult> lyricsList;

        private readonly string[] STOPWORDS = { "about", "after", "all", "also", "an", "and", "another", "any", "are", "as",
            "at", "be", "because", "been", "before", "being", "between", "both", "but", "by", "came", "can", "come", "could",
            "did", "do", "does", "each", "else", "for", "from", "get", "got", "had", "has", "have", "he", "her", "here", "him",
            "himself", "his", "how", "if", "in", "into", "is", "it", "its", "just", "like", "make", "many", "me", "might", "more",
            "most", "much", "must", "my", "never", "no", "now", "of", "on", "only", "or", "other", "our", "out", "over", "re", "said",
            "same", "see", "should", "since", "so", "some", "still", "such", "take", "than", "that", "the", "their", "them", "then", "there",
            "these", "they", "this", "those", "through", "to", "too", "under", "up", "use", "very", "want", "was", "way", "we", "well", "were",
            "what", "when", "where", "which", "while", "who", "will", "with", "would", "you", "your",
            "(", ")", "[", "]", "\'", "!", ".", ":", ";", ",", "\"", "|", "~", "?"};


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
                if (ControlStopword())
                {
                    var response = await Model.SearchLyrics(artist, title);

                    foreach (var lyric in response.SearchLyricResult)
                    {
                            LyricList.Add(lyric);
                    }
                    lyricsList.RemoveAt(lyricsList.Count-1);
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


        private bool ControlStopword()
        {
            string lowercaseTitle = Title.ToLower();
            string lowercaseArtist = Artist.ToLower();
            foreach (string stopword in STOPWORDS)
            {
                lowercaseTitle = Regex.Replace(lowercaseTitle, @"\b" + Regex.Escape(stopword) + @"\b", "");
                lowercaseArtist = Regex.Replace(lowercaseArtist, @"\b" + Regex.Escape(stopword) + @"\b", "");
            }
            lowercaseTitle = Regex.Replace(lowercaseTitle, @"(^\s+)", "");
            lowercaseArtist = Regex.Replace(lowercaseArtist, @"(^\s+)", "");

            if (lowercaseArtist.Length <= 0 || lowercaseTitle.Length <= 0)
            {
                return false;
            }
            return true;
        }

        public void Clear()
        {
            Title = string.Empty;
            Artist = string.Empty;
            Lyrics = string.Empty;
            LyricList.Clear();
        }

    }
}
