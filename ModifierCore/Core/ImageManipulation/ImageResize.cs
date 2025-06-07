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
using static SixLabors.ImageSharp.Image;


namespace ModifierCore.Core.ImageManipulation
{
    public class ImageResize
    {
        Image image { get; set; }
        Scaler scaler = new Scaler();
        public string ResizeJPG(string MyImage, Weight scale)
        {

            string newFolder = Path.Combine(Path.GetDirectoryName(MyImage), "clear");
            Directory.CreateDirectory(newFolder);


                    string newFileName = Path.Combine(newFolder, Path.GetFileName(MyImage));
                    (int, int) widthAndHeight = scaler.ResizeTo(image.Width, image.Height, scale);
                    using (Image image = Image.Load(MyImage))
                    {
                        using (MemoryStream imageStream = new MemoryStream())
                        {
                            image.Mutate(x => x.Resize(widthAndHeight.Item1, widthAndHeight.Item2));
                            image.Save(imageStream, new JpegEncoder());
                            return MyImage;
                        }
                    }
        }

        public (int, int) GetJPGSize (Stream MyImage)
        {
           ImageInfo info = Image.Identify(MyImage);
            return (info.Width, info.Height);
        }

    }
}
