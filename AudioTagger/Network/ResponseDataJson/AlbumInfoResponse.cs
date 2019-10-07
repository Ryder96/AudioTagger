using Newtonsoft.Json;
using System.Collections.Generic;

namespace AudioTagger.Network.ResponseDataJson.AlbumInfo
{
   /* public class Image
    {
        [JsonProperty("#text")]
        public string text { get; set; }
        public string size { get; set; }
    }*/

    public class Attr
    {
        public string rank { get; set; }
    }

    public class Streamable
    {
        [JsonProperty("#text")]
        public string text { get; set; }
        public string fulltrack { get; set; }
    }

    public class Artist
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Track
    {
        public string name { get; set; }
        public string url { get; set; }
        public string duration { get; set; }
        public Attr @attr { get; set; }
        public Streamable streamable { get; set; }
        public Artist artist { get; set; }
    }

    public class Tracks
    {
        public IList<Track> track { get; set; }
    }

    public class Tag
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Tags
    {
        public IList<Tag> tag { get; set; }
    }

    public class Wiki
    {
        public string published { get; set; }
        public string summary { get; set; }
        public string content { get; set; }
    }

    public class Album
    {
        public string name { get; set; }
        public string artist { get; set; }
        public string url { get; set; }
        public IList<Image> image { get; set; }
        public string listeners { get; set; }
        public string playcount { get; set; }
        public Tracks tracks { get; set; }
        public Tags tags { get; set; }
        public Wiki wiki { get; set; }
    }

    public class AlbumInfoResponse
    {
        public Album album { get; set; }
    }
}
