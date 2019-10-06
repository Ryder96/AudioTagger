using System.Collections.Generic;
using System.Xml.Serialization;

namespace AudioTagger.Network.ResponseDataXML
{
    [XmlRoot(ElementName = "SearchLyricResult", Namespace = "http://api.chartlyrics.com/")]
    public class SearchLyricResult
    {
        [XmlElement(ElementName = "TrackChecksum")]
        public string TrackChecksum { get; set; }

        [XmlElement(ElementName = "TrackId")]
        public int TrackId { get; set; }

        [XmlElement(ElementName = "LyricChecksum")]
        public string LyricChecksum { get; set; }

        [XmlElement(ElementName = "LyricId")]
        public int LyricId { get; set; }

        [XmlElement(ElementName = "SongUrl")]
        public string SongUrl { get; set; }

        [XmlElement(ElementName = "ArtistUrl")]
        public string ArtistUrl { get; set; }

        [XmlElement(ElementName = "Artist")]
        public string Artist { get; set; }
        [XmlElement(ElementName = "Song")]
        public string Song { get; set; }
        [XmlElement(ElementName = "SongRank")]
        public int SongRank { get; set; }
    }

    [XmlRoot(ElementName = "ArrayOfSearchLyricResult", Namespace = "http://api.chartlyrics.com/")]
    public class ArrayOfSearchLyricResult
    {
        [XmlElement(ElementName = "SearchLyricResult", Namespace = "http://api.chartlyrics.com/")]
        public List<SearchLyricResult> SearchLyricResult { get; set; }
        [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsd { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }

}
