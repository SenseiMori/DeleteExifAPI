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



namespace ModifierCore.Core.ImageManipulation
{
    public class ImageCompressor
    {

        public string JPGCompress(string MyImage, CompressLevel compressLevel)
        {
            //ImageInfoHandler imageInfoHandler = new ImageInfoHandler();           

                    using (Image image = Image.Load(MyImage))
                        {
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                image.Save(memoryStream, GetCompressLevel(compressLevel));
                                //jpg.Size = imageInfoHandler.GetBytesReadable(memoryStream.Length);
                                //byte[] weight = memoryStream.ToArray(); 
                                //imageResize.tempImages.Add(jpg);
                                return MyImage;
                            }
                        }
        }
        public JpegEncoder GetCompressLevel (Enum level)
        {
            var encoder = new JpegEncoder
            {
                Quality = (int)Weight.Extra,
                SkipMetadata = true,
            };
            return encoder;
        }
    }
}
