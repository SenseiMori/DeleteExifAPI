using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using ExifDeleteLib.Core;
using System.Linq;

namespace ExifDeleteLib
{
    public class JPGMetadataReader
    {
        JPGFile JPGFile { get; set; }

        public JPGMetadataReader()
        {
            JPGFile = new JPGFile(); 
        }
        public List<byte> ReadExifFromImage(string file)
        {
                List<byte> markersList = new List<byte>();
                List<byte> data = new List<byte>(); 
                data = JPGFile.GetMarkers(file);

                foreach (byte marker in data)
                    {
                        markersList.Add(marker);
                    }   
                //byte[] clearData = JPGFile.FindMarkers(image);
               // data.Add(clearData); 
                //newImages.Add(image,clearData);
            
            return markersList;  

        }
    }
}
