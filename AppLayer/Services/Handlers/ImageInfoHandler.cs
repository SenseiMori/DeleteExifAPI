using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppLayer.Model.Entities;
using ExifDeleteLib;
using ModifierCore.Core.Const;
using AppLayer.Model.Interfaces;
using ModifierCore.Core.ImageManipulation;
using Windows.Data.Text;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.VoiceCommands;

namespace AppLayer.Services.Handlers
{
    public class ImageInfoHandler : IDataProvider
    {
        private readonly IMainViewModel _mainViewModel;

        List<byte> exifData = new List<byte>();
        JPGMetadataReader metadataReader = new JPGMetadataReader();
        Scaler scaler = new Scaler();
        ImageResize imageResize = new ImageResize();
        ImageCompressor imageCompressor = new ImageCompressor();

        public ImageInfoHandler(IMainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }
        public MyImage GetInfo(string path)
        {
            exifData = metadataReader.ReadExifFromImage(path);
            var fileInfo = new FileInfo(path);
            var imageInfo = Image.Identify(path);
            return new MyImage
            {
                FileName = path,
                FilePath = Path.GetFullPath(path),
                ShortFileName = Path.GetFileNameWithoutExtension(path),
                Width = imageInfo.Width,
                Height = imageInfo.Height,
                WidthHeight = $"{imageInfo.Width}x{imageInfo.Height}",
                Size = imageCompressor.GetBytesReadable(fileInfo.Length),
                ExpectedWidthHeight = string.Empty,
                ExpectedSize = string.Empty,
                JPGMarkers = exifData
            };
        }
        public void GetExpectedData(ObservableCollection<MyImage> images, SizeScale scale, CompressLevel compressLevel)
        {
            foreach (MyImage image in images)
            {
                byte[] originalData = File.ReadAllBytes(image.FilePath);
                byte[] currentData = originalData;

                if (scale != SizeScale.None)
                {
                    image.ExpectedWidthHeight = scaler.ConvertToNewSize((image.Width, image.Height), scale);
                    currentData = imageResize.ResizeJPG(originalData, scale);
                }
                if (compressLevel != CompressLevel.None)
                {
                    long compressedSize = imageCompressor.JPGCompress(currentData, compressLevel).Length;
                    image.ExpectedSize = imageCompressor.GetBytesReadable(compressedSize);
                }
                else
                {
                    image.ExpectedSize = $"{imageCompressor.GetBytesReadable(currentData.Length)}";
                }
            }
        }
    }
}


