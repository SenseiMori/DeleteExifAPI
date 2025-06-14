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
    public class JPGMetadataReader
    {
        JPGFile JPGFile { get; set; }

        public JPGMetadataReader()
        {
            JPGFile = new JPGFile();
        }

        public async Task<byte[]> DeleteExifMarkers(string pathToFile) => await JPGFile.GetJPGWithoutAppSegments(pathToFile);

        #region поиск маркеров. Сделать быстрее, чище
        public async Task<List<byte>> ReadExifFromImage(string file)
        {
            List<byte> markersList = new List<byte>();
            List<byte> data = new List<byte>();
            data = await JPGFile.GetMarkersAppSegment(file);

            foreach (byte marker in data)
            {
                markersList.Add(marker);
            }
            return markersList;
        }
        #endregion
    }
}
