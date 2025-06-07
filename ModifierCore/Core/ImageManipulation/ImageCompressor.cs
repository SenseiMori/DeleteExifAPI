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
using TextRemoveExif.Model.Entities;
using System.IO;

namespace TextRemoveExif.Services.ImageManipulation
{
    public class ImageCompressor
    {
        public ImageCompressor(ObservableCollection<MyImage> images)
        {
            images = new ObservableCollection<MyImage>();
        }
        public void JPGCompress(ObservableCollection<MyImage> images, CompressLevel compressLevel)
        {
            ImageInfoHandler imageInfoHandler = new ImageInfoHandler();
            ImageResize imageResize = new ImageResize(images);
           
            foreach (var jpg in images)
                {
                    using (Image image = Image.Load(jpg.FilePath))
                        {
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                image.Save(memoryStream, GetCompressLevel(compressLevel));

                                jpg.Size = imageInfoHandler.GetBytesReadable(memoryStream.Length);
                                //byte[] weight = memoryStream.ToArray(); 
                                //imageResize.tempImages.Add(jpg);
                            }
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
