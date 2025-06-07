using ExifDeleteLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExifDeleteLib
{
    public class ZipCreator
    {

        private ZipBuilder builder;
        private JPGClearImageWriter clearImageWriter;
        private JPGMetadataReader metadataReader;
        public  ZipCreator()
        {
            builder = new ZipBuilder();
            clearImageWriter = new JPGClearImageWriter();
            metadataReader = new JPGMetadataReader();
        }

        public void CreateZip(string [] path)
        {
            
            builder.CopyImagesToZip(path);
        }

    }
}
