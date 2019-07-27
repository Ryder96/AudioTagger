using AudioTagger.Network.ResponseDataJson;
using AudioTagger.Network.ResponseDataJson.AlbumInfo;
using AudioTagger.Network.ResponseDataJson.CoverArtArchiveResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioTagger.ui.MVP.LastFM
{
    interface ILastFMModel
    {
        Task<TrackSearch> SearchTrack(string title, string artist);
        Task<TrackInfo> RequestInfoTrack(string mbid);
        Task<TrackInfo> RequestInfoTrack(string title, string artist);
        //Tast<AlbumInfo> RequestoAlbumInfo(string mbid);
        Task<AlbumInfoResponse> QueryInfoAlbum(string mbid);
        Task<AlbumInfoResponse> QueryInfoAlbum(string artist,string album);

    }
}
