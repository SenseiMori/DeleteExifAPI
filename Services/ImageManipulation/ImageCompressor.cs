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

namespace TextRemoveExif.Services.ImageManipulation
{
    public class ImageCompressor
    {

        public void JPGCompress(ObservableCollection<MyImage> images)
        {
            var encoder = new JpegEncoder
            {
                Quality = (int)Weight.Best,
            };
            foreach (var jpg in images)
                {
                    using (Image image = Image.Load(jpg.FilePath))
                        {
                            image.Save(jpg.FilePath,encoder);
                        }

                }
        }
    }
}
