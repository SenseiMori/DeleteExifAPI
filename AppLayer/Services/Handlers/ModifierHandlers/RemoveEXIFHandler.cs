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
using AppLayer.Model.Interfaces;
using DeleteExifCore.Core.JPG;

namespace AppLayer.Services.Handlers.ModifierHandlers
{
    public class RemoveEXIFHandler : IImageHandlerAsync
    {
        JPGMetadataReader reader = new();

        public byte[] GetBytesFromFile (MyImage image)
        {
            return File.ReadAllBytes(image.FilePath);
        }
        public async Task <byte[]> Handler(string path)
        {
            byte[] data = await reader.DeleteExifMarkers(path);
            return data;



        }
            
    }
}
