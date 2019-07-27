using Newtonsoft.Json;
using System.Collections.Generic;

namespace AudioTagger.Network.ResponseDataJson
{
    public class TrackSearch
    {
        public Results results { get; set; }
    }

    public class TrackInfo
    {
        public TrackInfoResponse.Track Track { get; set; }
    }

    public class OpensearchQuery
    {
        public string text { get; set; }
        public string role { get; set; }
        public string startPage { get; set; }
    }

    public class Image
    {
        [JsonProperty ("#text")]
        public string text { get; set; }
        public string size { get; set; }
    }

    public class Track
    {
        //
        public string name { get; set; }
        public string artist { get; set; }
        public string url { get; set; }
        public string streamable { get; set; }
        public string listeners { get; set; }
        public IList<Image> image { get; set; }
        public string mbid { get; set; }
    }

    namespace TrackInfoResponse
    {
        public class Track
        {
            public string name { get; set; }
            public Artist artist { get; set; }
            public string url { get; set; }
            public Streamable streamable { get; set; }
            public string listeners { get; set; }
            public IList<Image> image { get; set; }
            public string mbid { get; set; }

            //Additional data from Track Info query
            public Album album { get; set; }
            public Toptags toptags { get; set; }
            public Wiki wiki { get; set; }
        }
    }


    public class Trackmatches
    {
        public IList<Track> track { get; set; }
    }

    public class Attr
    {
        public string position { get; set; }
    }

    public class Results
    {
        public OpensearchQuery OpensearchQuery { get; set; }
        public string OpenSearchtotalResults { get; set; }
        public string OpensearchStartIndex { get; set; }
        public string OpensearchItemsPerPage { get; set; }
        public Trackmatches trackmatches { get; set; }
        public Attr attr { get; set; }
    }

    //Track Info
    public class Streamable
    {
        public string text { get; set; }
        public string fulltrack { get; set; }
    }

    public class Artist
    {
        public string name { get; set; }
        public string mbid { get; set; }
        public string url { get; set; }
    }

    public class Album
    {
        public string artist { get; set; }
        public string title { get; set; }
        public string mbid { get; set; }
        public string url { get; set; }
        public IList<Image> image { get; set; }
        public Attr Attr { get; set; }
    }
    public class Tag
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Toptags
    {
        public IList<Tag> tag { get; set; }
    }

    public class Wiki
    {
        public string published { get; set; }
        public string summary { get; set; }
        public string content { get; set; }
    }



    // CoverArtArchive Response
    namespace CoverArtArchiveResponse
    {
        public class Thumbnails
        {
            [JsonProperty("250")]
            public string xsmall { get; set; }

            [JsonProperty("500")]
            public string medium { get; set; }

            [JsonProperty("1200")]
            public string big { get; set; }

            public string small { get; set; }
            public string large { get; set; }
        }


        public class Image
        {
            public IList<string> types { get; set; }
            public bool front { get; set; }
            public bool back { get; set; }
            public int edit { get; set; }
            public string image { get; set; }
            public string comment { get; set; }
            public bool approved { get; set; }
            public string id { get; set; }
            public Thumbnails thumbnails { get; set; }
        }

        public class CoverArtResponse
        {
            public IList<Image> images { get; set; }
            public string release { get; set; }
        }
    }

}
