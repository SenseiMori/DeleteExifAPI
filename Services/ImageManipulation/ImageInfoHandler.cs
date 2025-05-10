using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using SixLabors.ImageSharp;
using TextRemoveExif.Model.Entities;
using TextRemoveExif.Model.Interfaces;


namespace TextRemoveExif.Services.ImageManipulation
{
    public class ImageInfoHandler : IDataProvider
    {

        public MyImage GetInfo (string path)
        {
            Scaler scaler = new Scaler();

            var fileInfo = new FileInfo(path);
            var imageInfo = Image.Load(path);
            string exSize = scaler.ConvertToNewSize((imageInfo.Width, imageInfo.Height), Weight.Best);
            return new MyImage
            
            {
                FileName = path,
                FilePath = System.IO.Path.GetFullPath(path),
                ShortFileName = System.IO.Path.GetFileNameWithoutExtension(path),
                Width = imageInfo.Width,
                Height = imageInfo.Height,
                WidthHeight = $"{imageInfo.Width}x{imageInfo.Height}",
                Size = GetBytesReadable(fileInfo.Length),
                ExpectedWidthHeight = exSize,
            };

        }
        public string GetBytesReadable(long num)
        {
            long absolute_num = (num < 0 ? -num : num);
            string suffix;
            double readable;
            if (absolute_num >= 0x100000) // Megabyte
            {
                suffix = "MB";
                readable = (num >> 10);
            }
            else if (absolute_num >= 0x400) // Kilobyte
            {
                suffix = "KB";
                readable = num;
            }
            else
            {
                return num.ToString("0 B"); // Byte
            }
            // Divide by 1024 to get fractional value
            readable = (readable / 1024);
            // Return formatted number with suffix
            return readable.ToString("0.#") + suffix;
        }
    }
}
