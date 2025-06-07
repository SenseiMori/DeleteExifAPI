using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ExifDeleteLib;
using ExifDeleteLib.Core;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using TextRemoveExif.Model.Entities;
using TextRemoveExif.Services.RemoveMetadata;
using WinRT;

namespace TextRemoveExif.Services.ImageManipulation
{
    public class ImageResize
    {
        ObservableCollection<MyImage> _images { get; set; }

        JPGDirectory JPGDirectory = new JPGDirectory();
        public ImageResize(ObservableCollection<MyImage> images)
        {
            images = new ObservableCollection<MyImage>();
        }
        Scaler scaler = new Scaler();
        public void ResizeJPG(ObservableCollection<MyImage> images, Weight scale)
        {

            string newFolder = Path.Combine(Path.GetDirectoryName(images[0].FilePath), "clear");
            Directory.CreateDirectory(newFolder);

            foreach (var jpg in images) 
            {
                {
                    string newFileName = Path.Combine(newFolder, Path.GetFileName(jpg.FilePath));
                    (int, int) widthAndHeight = scaler.ResizeTo(jpg.Width, jpg.Height, scale);
                    using (Image image = Image.Load(jpg.FilePath))
                    {
                        using (MemoryStream imageStream = new MemoryStream())
                        {
                            image.Mutate(x => x.Resize(widthAndHeight.Item1, widthAndHeight.Item2));
                            image.Save(imageStream, new JpegEncoder());
                        }
                        jpg.Width = image.Width;
                        jpg.Height = image.Height;
                        jpg.WidthHeight = $"{image.Width}x{image.Height}";
                    }
                }
            }
        }

    }
}
