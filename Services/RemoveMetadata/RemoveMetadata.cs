using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using TextRemoveExif.Model;
using TextRemoveExif.ViewModel;
using System.Windows;
using ExifDeleteLib;

namespace TextRemoveExif
{
    public class RemoveMetadata
    {
        private ObservableCollection<Image> _images { get; set; }
        JPGMetadataReader reader { get; set; }
        JPGClearImageWriter writer { get; set; }
        ZipCreator creator { get; set; }
             
        
        public RemoveMetadata(ObservableCollection<Image> images)
        {
            _images = images;
            reader = new JPGMetadataReader();
            writer = new JPGClearImageWriter();
            creator = new ZipCreator();
        }
       public void Remove()
        {
            string[] path = _images.Select(path => path.FilePath).ToArray();
            reader.ReadExifFromImage(path);
            writer.SaveClearImages(reader.newImages);
        }

        public void CreateZip()
        {
            string[] path = _images.Select(path => path.FilePath).ToArray();
            creator.CreateZip(path);

        }
        
    }
}
