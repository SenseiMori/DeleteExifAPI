using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Linq;
using ExifDeleteLib.Core;
using System.Collections.ObjectModel;

namespace ExifDeleteLib
{
    public class JPGClearImageWriter
    {
        JPGFile JPGFile { get; set; }
        JPGDirectory JPGDirectory { get; set; }
        public List<string> newClearImages;


        public JPGClearImageWriter()
        {
            JPGFile = new JPGFile();
            JPGDirectory = new JPGDirectory();
            newClearImages = new List<string>();
        }
        public List<string> SaveClearImages(Dictionary<string, byte[]> clearImages) 
        {
            foreach (var image in clearImages)
            {
                string IKey = image.Key;
                byte[] IValue = image.Value;

                string clearImagesDirectory = JPGDirectory.CreateDirectory(IKey);
                string clearImage = JPGDirectory.CreateFileInNewDirectory(IKey, clearImagesDirectory);
                JPGFile.WriteClearDataToNewFile(IValue, clearImage);

                newClearImages.Add(clearImage);
            }
            return newClearImages;
            
        }
    }
}
