﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace AudioTagger.ui.Data
{
    public class SongFile
    { 
        public string FileName { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string TagType { get; set; }
        public TagLib.Tag Tag { get;set; }
        public string Path { get; set; }
        public BitmapImage Image { get; internal set; }

        public SongFile(){}

        public SongFile(SongFile file)
        {
            FileName = file.FileName ?? string.Empty;
            Title = file.Title ?? string.Empty;
            Artist = file.Artist ?? string.Empty;
            Album = file.Album ?? string.Empty;
            Path = file.Path ?? string.Empty;
            TagType = file.TagType ?? string.Empty;
            Tag = file.Tag;
            Image = file.Image;
        }

        public override string ToString()
        {
            string info = "";
            info += "Title\n";
            info += Title + "\n";
            info += "Artist(s)\n";
            info += Artist + "\n";
            info += "Album name\n";
            info += Album + "\n";
            return info;
        }
    }
}
