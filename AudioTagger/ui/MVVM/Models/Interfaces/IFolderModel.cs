using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace AudioTagger.ui.MVVM.Models.Interfaces
{
    public interface IFolderModel
    {
        Task<IList<StorageFile>> OpenFolder();
        Task<IList<StorageFile>> OpenFolder(string cwd);
        string PathFolder();
    }
}
