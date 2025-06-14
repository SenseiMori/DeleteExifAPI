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

namespace AppLayer.Services.Handlers.ModifierHandlers
{
    public class RemoveEXIFHandler : IImageHandler
    {
        JPGMetadataReader reader = new();
        public byte[] Handler(byte[] originData) => reader.DeleteExifMarkers(originData);
    }
}
