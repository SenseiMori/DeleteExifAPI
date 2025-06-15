using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModifierCore.Core.Const;



namespace ModifierCore.Core.ImageManipulation
{
    public class ImageResize
    {
        Scaler scaler = new Scaler();
        public async Task <byte[]> ResizeJPG(string path, SizeScale scale)
        {
            using (Image image = await Image.LoadAsync(path))
            {
                (int, int) widthAndHeight = scaler.GetScaledSize(image.Width, image.Height, scale);
                 using (MemoryStream imageStream = new MemoryStream())
                {
                    image.Mutate(x => x.Resize(widthAndHeight.Item1, widthAndHeight.Item2));
                    await image.SaveAsync(imageStream, new JpegEncoder());
                    return imageStream.ToArray();
                }
            }
        }
        public (int, int) GetJPGSize(Stream MyImage)
        {
            ImageInfo info = Image.Identify(MyImage);
            return (info.Width, info.Height);
        }
    }
}
