using System.Threading.Tasks;
using AudioTagger.Network.InfoSong;
using AudioTagger.Network.ResponseDataJson;
using AudioTagger.Network.ResponseDataJson.AlbumInfo;
using Refit;

namespace AudioTagger.Network
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

    }
}
