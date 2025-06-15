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
            string newDirectory = Path.Combine(Path.GetDirectoryName(originalPath), "ClearImages");
            Directory.CreateDirectory(newDirectory);
            string newFile = Path.Combine(newDirectory, Path.GetFileName(originalPath));
            File.WriteAllBytes(newFile, data);
            return newFile;
        }
    }
}
