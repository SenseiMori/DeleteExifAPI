using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRemoveExif.Model.Entities;

namespace TextRemoveExif.Services.ImageManipulation
{
    public class Scaler
    {
        MyImage myImage = new MyImage();
        public (int, int) ResizeTo(int width, int height, Weight option)
        {
            double scale = (int)option / 100.0;
            int newWidth = (int)(width * scale);
            int newHeight = (int)(height * scale);
            return (newWidth, newHeight);
        }
        public string ConvertToNewSize( (int, int) oldJPGSize, Weight scale)
        {
            (int, int) expectedSize = ResizeTo(oldJPGSize.Item1, oldJPGSize.Item2, scale);

            string result = $"{expectedSize.Item1}x{expectedSize.Item2}";
            return result;
        }
    }
}
