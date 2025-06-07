using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace ExifDeleteLib.Core
{
    public class JPGDirectory
    {
        public string GetDirectory (string directory) => Path.GetDirectoryName(directory);
        public string[] GetJPGFromFolder(string folder)
        {
            string[] images = Directory.GetFiles(folder, "*.jpg");
            return images;
        }
        public string CreateFileInNewDirectory(string OriginFile, string newDirectory)
        {

            string outFileName = Path.GetFileName(OriginFile);
            string outputFilePath = Path.Combine(newDirectory, outFileName);
            File.Copy(OriginFile, outputFilePath, true);
            return outputFilePath;

        }

        public string CreateDirectory(string originDirectory)
        {
            string dir = Path.GetDirectoryName(originDirectory);
            string newDirectory = Path.Combine(dir, "ClearImages");
            if (!Directory.Exists(newDirectory))
            {
                Directory.CreateDirectory(newDirectory);
            }
            
            return newDirectory;
        }
    }
}
