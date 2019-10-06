using AudioTagger.Network.ResponseDataXML;
using System.Threading.Tasks;

namespace AudioTagger.ui.MVVM.Models.Interfaces
{
    interface ILyricsModel
    {
        Task<ArrayOfSearchLyricResult> SearchLyrics(string artist, string title);

        Task<GetLyricResult> RetrieveLyrics(int lyricid, string lyricchecksum);
    }
}
