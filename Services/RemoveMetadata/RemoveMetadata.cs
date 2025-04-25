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
    class RemoveMetadata
    {
        private ObservableCollection<Image> _images { get; set; }
        
        
        public RemoveMetadata(ObservableCollection<Image> images)
        {
            _images = images;
        }
        
        
        public void test()
        {
            
            
        }
        
    }
}
