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
        JPGMetadataRemover MetadataRemover { get; set; }
             
        
        public RemoveMetadata(ObservableCollection<Image> images)
        {
            _images = images;
            MetadataRemover = new JPGMetadataRemover() ?? throw new NullReferenceException(nameof(MetadataRemover));
        }
       public void Remove()
        {
            string[] path = _images.Select(path => path.FilePath).ToArray();
            MetadataRemover.RemoveExifForImage(path);

        }
        
    }
}
