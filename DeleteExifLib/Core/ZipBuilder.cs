using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace ExifDeleteLib.Core
{
    public class ZipBuilder
    {

        public void CopyImagesToZip(string[] clearImages)
        {
            {
                string zipPath = Path.Combine(Path.GetDirectoryName(clearImages[0]), "name.zip");
                using var outStream = new FileStream(zipPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 4096, true);
                using var archive = new ZipArchive(outStream, ZipArchiveMode.Create, true);
                foreach (string fileName in clearImages)
                {
                    using (var inputStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
                    {
                        ZipArchiveEntry fileEntry = archive.CreateEntry(Path.GetFileName(fileName));
                        using var entryStream = fileEntry.Open();
                        inputStream.CopyTo(entryStream);

                    }
                }
            }
        }
    }
}
