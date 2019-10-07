using AudioTagger.Network.ResponseDataJson;
using AudioTagger.Network.ResponseDataJson.AlbumInfo;
using System.Threading.Tasks;

namespace AudioTagger.ui.MVP.LastFM
{
    interface ILastFMModel
    {
        Task<TrackSearch> SearchTrack(string title, string artist);
        Task<TrackInfo> RequestInfoTrack(string mbid);
        Task<TrackInfo> RequestInfoTrack(string title, string artist);
        //Tast<AlbumInfo> RequestoAlbumInfo(string mbid);
        Task<AlbumInfoResponse> RequestInfoAlbum(string mbid);
        Task<AlbumInfoResponse> RequestInfoAlbum(string artist,string album);

    }
}
