using System;
using System.IO;
using System.Net;
using TagLib;

namespace AudioTagger.ui.Data
{
    public class DataFile
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string CoverUrl { get; set; }
        public string FolderUrl { get; set; }
        public string FileName { get; set; }
        public DataFile() { }

        private IPicture GeneratePicture()
        {
            if (CoverUrl == null || CoverUrl.Equals(string.Empty))
            {
                return null;
            }
            byte[] imageBytes;
            using (WebClient client = new WebClient())
            {
                imageBytes = client.DownloadData(CoverUrl);
            }
            TagLib.Id3v2.AttachedPictureFrame cover = new TagLib.Id3v2.AttachedPictureFrame
            {
                Type = PictureType.FrontCover,
                Description = "Cover",
                MimeType = System.Net.Mime.MediaTypeNames.Image.Jpeg,
                Data = imageBytes,
                TextEncoding = StringType.UTF16
            };

            return cover;
        }

        public override string ToString()
        {
            return "DataFile\n" + Title + "\n" + Artist + "\n" + Album + "\n" + CoverUrl + "\n" + FolderUrl + "\n" + FileName + "\n";
        }

        public async void ApplyTag()
        {
            Windows.Storage.StorageFolder folder = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(FolderUrl);
            Windows.Storage.StorageFile file = await folder.GetFileAsync(FileName);
            Stream fileStream = await file.OpenStreamForWriteAsync();
            TagLib.File tagFile = TagLib.File.Create(new StreamFileAbstraction(FileName, fileStream, fileStream));
            tagFile.Tag.Title = Title;
            tagFile.Tag.Performers = new string[] { Artist };
            tagFile.Tag.Album = Album;
            if (tagFile.MimeType == "taglib/mp3")
            {
                IPicture pic = GeneratePicture();
                if (pic != null)
                {
                    tagFile.Tag.Pictures = new IPicture[] { pic };
                }
            }
            if (tagFile.MimeType == "taglib/flac")
            {

            }
            tagFile.Save();
            tagFile.Dispose();
            fileStream.Close();
        }
    }
}
