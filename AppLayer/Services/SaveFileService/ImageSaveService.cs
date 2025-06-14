using AppLayer.Services.Handlers;
using AppLayer.Services.RemoveMetadata;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLayer.Services.SaveFileService
{
    class ImageSaveService : IFileManager
    {
        public string Save(byte[] data, string originalPath)
        {
            string targetDir = Path.Combine(Path.GetDirectoryName(originalPath)!, "ClearImages");
            Directory.CreateDirectory(targetDir);
            string targetFile = Path.Combine(targetDir, Path.GetFileName(originalPath));
            File.WriteAllBytes(targetFile, data);
            return targetFile;
        }
    }
}
