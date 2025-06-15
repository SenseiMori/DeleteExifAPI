using AppCore.Services.RemoveMetadata;
using System.IO;
namespace AppCore.Services.SaveFileService
{
    class ImageSaveService : IFileManager
    {
        public string Save(byte[] data, string originalPath)
        {
            string newDirectory = Path.Combine(Path.GetDirectoryName(originalPath), "New Images");
            Directory.CreateDirectory(newDirectory);
            string newFile = Path.Combine(newDirectory, Path.GetFileName(originalPath));
            File.WriteAllBytes(newFile, data);
            return newFile;
        }
    }
}
