using ModifierCore.Core.ImageManipulation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLayer.Services.SaveFileService
{
     class DirectoryService
    {
        ImageInfoHandler imageInfoHandler = new ImageInfoHandler();

        public string SaveModifierFile(byte[] data, string pathToSave)
        {
            string newDirectory = CreateDirectory(pathToSave);
            //string newFile = CreateFileInNewDirectory(pathToSave, newDirectory);
            string newFile = Path.Combine(newDirectory, Path.GetFileName(pathToSave));
            File.WriteAllBytes(newFile, data);
            return pathToSave;
        }
        public string CreateDirectory(string originDirectory)
        {
            string dir = Path.GetDirectoryName(originDirectory);
            string newDirectory = Path.Combine(dir, "ClearImages");
            Directory.CreateDirectory(newDirectory);
            return newDirectory;
        }
        public string CreateFileInNewDirectory(string originFile, string newDirectory)
        {

            string outFileName = Path.GetFileName(originFile);
            string outputFilePath = Path.Combine(newDirectory, outFileName);
            File.Copy(originFile, outputFilePath, true);
            return outputFilePath;
        }

    }
}
