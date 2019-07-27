using AudioTagger.Network.ResponseDataJson.AlbumInfo;
using AudioTagger.Network.ResponseDataJson.TrackInfoResponse;
using System.Collections.Generic;

namespace AudioTagger.Bundle
{
    class TrackBundle
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Image { get; set; }

        public TrackBundle(Network.ResponseDataJson.TrackInfoResponse.Track track)
        {
            InitTrackInfo(track);
        }

        public TrackBundle() { }

        private void InitTrackInfo(Network.ResponseDataJson.TrackInfoResponse.Track track)
        {
            Title = track.name;
            Artist = track.artist.name;
            Album = track.album?.title ?? string.Empty;
            Image = string.Empty;
            //Image = InitImage(track.image);
        }

        private string InitImage(IList<Image> imageList)
        {
            if(imageList.Count != 0)
            {
                foreach(Image image in imageList)
                {
                    System.Diagnostics.Debug.WriteLine(image.text);
                    if(image.size.Equals("large"))
                    {
                        return image.text ?? "";
                    }
                }
            }
            return "";
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
