using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Compression;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats;
using System.IO;
using ModifierCore.Core.Const;


namespace ModifierCore.Core.ImageManipulation
{
    public class ImageCompressor
    {
        public async Task<byte[]> JPGCompress(string path, CompressLevel compressLevel)
        {
            using (Image image = await Image.LoadAsync(path))
            {
                using (MemoryStream br = new MemoryStream())
                {
                    await image.SaveAsync(br, GetCompressLevel(compressLevel));
                    return br.ToArray();

                }

            }
        }
        public JpegEncoder GetCompressLevel(CompressLevel level)
        {
            var encoder = new JpegEncoder
            {
                Quality = (int)level,
                SkipMetadata = true,
            };

            return encoder;
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
