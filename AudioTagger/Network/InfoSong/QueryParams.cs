namespace AudioTagger.Network.InfoSong
{
    public class QueryParam
    {
        public string method { get; set; }
        public string api_key => "ca9c29022683059d9bd536da723b6e88";
        public string format => "json";
    }

    public class QuerySearchTrackParam : QueryParam
    {
        public string track { get; set; }
    }

    public class QuerySearchParam : QueryParam
    {
        public string track {  get; set; }
        public string artist {  get; set; }
    }

    public class QueryInfoMbidParam : QueryParam
    {
        public string mbid {  get; set; }
    }

    public class QueryInfoAlbumParam : QueryParam
    {
        public string album { get; set; }
        public string artist { get; set; }
    }

}
