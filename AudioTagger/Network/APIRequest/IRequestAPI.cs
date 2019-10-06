using System.Threading.Tasks;
using AudioTagger.Network.ResponseDataXML;
using AudioTagger.Network.ResponseDataJson;
using AudioTagger.Network.ResponseDataJson.AlbumInfo;
using AudioTagger.Network.APIRequest.LastFM.Parameters;
using Refit;

namespace AudioTagger.Network.APIRequest
{
    interface IRequestAPI
    {
        // Query song info from LastFM
        [Get("/2.0/")]
        Task<TrackSearch> SearchTrack(QueryParam param);

        [Get("/2.0/")]
        Task<TrackInfo> SearchTrackInfo(QueryParam param);


        //Query album info from LastFM
        [Get("/2.0/")]
        Task<AlbumInfoResponse> SearchAlbumInfo(QueryParam param);


        //http://musicbrainz.org/ws/2/artist/?query=artist:fred&fmt=json


        //Query song lyrics response is in xml

        [Get("/SearchLyric")]
        Task<ArrayOfSearchLyricResult> SearchLyrics(string artist, string song);

        [Get("/GetLyric")]
        Task<GetLyricResult> GetLyrics(int lyricID, string lyricChecksum);

    }
}
