using AudioTagger.ui.MVVM.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace AudioTagger.ui.MVVM.Models
{
    public class FolderModel : IFolderModel
    {
        private string m_PathFolderChoosen =  string.Empty;

        private async Task<IList<StorageFile>> PickFolder()
        {
            IList<StorageFile> listMusicFiles = null;
            //IList<StorageFolder> listSubFolder = null;
            var folderPicker = new Windows.Storage.Pickers.FolderPicker
            {
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.MusicLibrary
            };

            folderPicker.FileTypeFilter.Add("*");

            StorageFolder choosenFolder = await folderPicker.PickSingleFolderAsync();

            m_PathFolderChoosen = choosenFolder.Path;

            if (choosenFolder != null)
            {
                Windows.Storage.AccessCache.StorageApplicationPermissions.
                FutureAccessList.AddOrReplace("PickedFolderToken", choosenFolder);
                listMusicFiles = ConvertList(await choosenFolder.GetFilesAsync());

                RemoveNotMusicFiles(listMusicFiles);

            }

            return listMusicFiles;
        }



        private IList<StorageFile> ConvertList(IReadOnlyList<StorageFile> readOnlyList)
        {
            IList<StorageFile> newList = new List<StorageFile>();
            foreach (StorageFile file in readOnlyList)
            {
                newList.Add(file);
            }
            return newList;
        }

        private void RemoveNotMusicFiles(IList<StorageFile> songFiles)
        {
            foreach (StorageFile file in songFiles.ToList())
            {
                if (file.ContentType != "audio/mpeg")
                {
                    if (file.ContentType != "audio/x-flac")
                    {
                        songFiles.Remove(file);
                    }
                }
            }
        }

        public async Task<IList<StorageFile>> OpenFolder()
        {
            IList<StorageFile> list = await PickFolder();
            return list;
        }

        public async Task<IList<StorageFile>> OpenFolder(string cwd)
        {
            IList<StorageFile> listMusicFiles = null;
            var folder = await StorageFolder.GetFolderFromPathAsync(cwd);
            if (folder != null)
            {
                Windows.Storage.AccessCache.StorageApplicationPermissions.
                FutureAccessList.AddOrReplace("PickedFolderToken", folder);
                listMusicFiles = ConvertList(await folder.GetFilesAsync());
                RemoveNotMusicFiles(listMusicFiles);

            }

            return listMusicFiles;
        }

        public string PathFolder()
        {
            return m_PathFolderChoosen;
        }
    }
}
