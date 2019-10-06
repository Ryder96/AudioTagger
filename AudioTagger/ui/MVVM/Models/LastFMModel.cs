using AudioTagger.Network.APIRequest;
using AudioTagger.Network.APIRequest.LastFM.Parameters;
using AudioTagger.Network.ResponseDataJson;
using AudioTagger.Network.ResponseDataJson.AlbumInfo;
using Refit;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AudioTagger.ui.MVP.LastFM
{
    class LastFMModel : ILastFMModel
    {
        private readonly string LASTFM_ENDPOINT = "http://ws.audioscrobbler.com";

        public async Task<TrackInfo> RequestInfoTrack(string mbid)
        {
            QueryInfoMbidParam param = new QueryInfoMbidParam()
            {
                method = "track.getInfo",
                mbid = mbid
            };
            var LastFMApi = RestService.For<IRequestAPI>(LASTFM_ENDPOINT);

            var response = await LastFMApi.SearchTrackInfo(param);
            return response;
        }

        public async Task<TrackInfo> RequestInfoTrack(string title, string artist)
        {
            QuerySearchParam param = new QuerySearchParam()
            {
                method = "track.getInfo",
                track = title,
                artist = artist
            };
            var LastFMApi = RestService.For<IRequestAPI>(LASTFM_ENDPOINT);
            var response = await LastFMApi.SearchTrackInfo(param);
            return response;
        }
        
        public async Task<TrackSearch> SearchTrack(string title, string artist)
        {
            QuerySearchParam param = new QuerySearchParam()
            {
                method = "track.search",
                track = title,
                artist = artist
            };
      
            var LastFMApi = RestService.For<IRequestAPI>(LASTFM_ENDPOINT);

            var response = await LastFMApi.SearchTrack(param);

            return response;
        }

        public async Task<AlbumInfoResponse> QueryInfoAlbum(string mbid)
        {
            QueryInfoMbidParam param = new QueryInfoMbidParam()
            {
                method = "album.getInfo",
                mbid = mbid
            };

            var httpClient = new HttpClient(new Network.Utils.HttpLoggingHandler())
            {
                BaseAddress = new Uri(LASTFM_ENDPOINT)
            };
            var LastFMApi = RestService.For<IRequestAPI>(httpClient);


            //var LastFMApi = RestService.For<IRequestAPI>(LASTFM_ENDPOINT);
            var response = await LastFMApi.SearchAlbumInfo(param);
            return response;
        }

        public async Task<AlbumInfoResponse> QueryInfoAlbum(string artist, string album)
        {
            QueryInfoAlbumParam param = new QueryInfoAlbumParam()
            {
                method = "album.getInfo",
                artist = artist,
                album = album
            };
            var LastFMApi = RestService.For<IRequestAPI>(LASTFM_ENDPOINT);
            var response = await LastFMApi.SearchAlbumInfo(param);
            return response;
        }
    }
}
