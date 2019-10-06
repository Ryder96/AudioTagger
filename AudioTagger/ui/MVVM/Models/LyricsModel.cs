using Refit;
using System.Threading.Tasks;
using AudioTagger.Network.APIRequest;
using AudioTagger.Network.ResponseDataXML;
using AudioTagger.ui.MVVM.Models.Interfaces;
using System.Net.Http;
using System;

namespace AudioTagger.ui.MVVM.Models
{
    class LyricsModel : ILyricsModel
    {
        private readonly string API_ENDPOINT = "http://api.chartlyrics.com/apiv1.asmx";


        public async Task<GetLyricResult> RetrieveLyrics(int lyricID, string lyricChecksum)
        {
            var httpClient = new HttpClient(new Network.Utils.HttpLoggingHandler())
            {
                BaseAddress = new Uri(API_ENDPOINT)
            };

            var lyricsAPI = RestService.For<IRequestAPI>(httpClient,
                new RefitSettings
                {
                    ContentSerializer = new XmlContentSerializer()
                });

            var response = await lyricsAPI.GetLyrics(lyricID, lyricChecksum);

            return response;
        }

        public async Task<ArrayOfSearchLyricResult> SearchLyrics(string artist, string title)
        {
            /*var lyricsAPI = RestService.For<IRequestAPI>(API_ENDPOINT,
                new RefitSettings
                {
                    ContentSerializer = new XmlContentSerializer()
                });
            */
            var httpClient = new HttpClient(new Network.Utils.HttpLoggingHandler())
            {
                BaseAddress = new Uri(API_ENDPOINT)
            };
            var lyricsAPI = RestService.For<IRequestAPI>(httpClient,
                new RefitSettings
                {
                    ContentSerializer = new XmlContentSerializer()
                });

            var response = await lyricsAPI.SearchLyrics(artist, title);
            return response;
        }
    }
}
