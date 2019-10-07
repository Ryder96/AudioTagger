using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AudioTagger.Bundle
{
    class TrackBundle : INotifyPropertyChanged
    {
        private string image;
        private string album;
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Image 
        { 
            get { return image; }
            set { if (!image.Equals(value)) { image = value; NotifyPropertyChanged(); } }
        }

        public string Album
        {
            get { return album; }
            set { if (!album.Equals(value)) { album = value; NotifyPropertyChanged(); } }
        }

        public TrackBundle(Network.ResponseDataJson.TrackInfoResponse.Track track)
        {
            InitTrackInfo(track);
        }

        public TrackBundle() {
            image = string.Empty;
            album = string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void InitTrackInfo(Network.ResponseDataJson.TrackInfoResponse.Track track)
        {
            Title = track.name;
            Artist = track.artist.name;
            Album = track.album?.title ?? string.Empty;
            Image = string.Empty;
        }

        public override string ToString()
        {
            string info = "";
            info += "Title\n";
            info += Title + "\n";
            info += "Artist(s)\n";
            info += Artist + "\n";
            info += "Album name\n";
            info += Album + "\n";
            return info;
        }
    }


}
