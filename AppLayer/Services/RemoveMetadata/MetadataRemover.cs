using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using ExifDeleteLib;
using AppLayer.Model.Entities;
using ModifierCore.Core.ImageManipulation;

namespace AppLayer
{
    public class MetadataRemover
    {
        ObservableCollection<MyImage> _images { get; set; }
        JPGMetadataReader reader { get; set; }
        JPGClearImageWriter writer { get; set; }
        ZipCreator creator { get; set; }
        ImageResize resizer { get; set; }
        


        public MetadataRemover(ObservableCollection<MyImage> images)
        {
            _images = images;
            reader = new JPGMetadataReader();
            writer = new JPGClearImageWriter();
            creator = new ZipCreator();
            resizer = new ImageResize();
        }
       public void Remove()
        {      
            string[] path = _images.Select(path => path.FilePath).ToArray();
        }
        public void SaveImages()
        {
            //writer.SaveClearImages(reader.newImages);
        }
        public void CreateZip()
        {
            string[] path = _images.Select(path => path.FilePath).ToArray();
            creator.CreateZip(path);

        }
        
    }
}
