using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ExifDeleteLib.Core;

namespace ExifDeleteLib
{
    public class ZipCreator
    {

        private ZipBuilder builder;
        private JPGClearImageWriter clearImageWriter;
        private JPGMetadataReader metadataReader;
        public ZipCreator()
        {
            builder = new ZipBuilder();
            clearImageWriter = new JPGClearImageWriter();
            metadataReader = new JPGMetadataReader();
        }

        public void CreateZip(string[] path)
        {

            builder.CopyImagesToZip(path);
        }

    }
}
