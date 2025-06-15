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
using DeleteExifCore.Core.JPG;

namespace AppLayer.Services.Handlers
{
    public class ImageInfoHandler : IDataProviderAsync
    {
        private readonly IMainViewModel _mainViewModel;

        List<byte> exifData = new();
        JPGMetadataReader metadataReader = new();
        Scaler scaler = new Scaler();
        ImageResize imageResize = new ImageResize();
        ImageCompressor imageCompressor = new ImageCompressor();

        public ImageInfoHandler(IMainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }
        public async Task <MyImage> GetInfo(string path)
        {
            exifData = await metadataReader.ReadExifFromImage(path);
            var fileInfo = new FileInfo(path);
            var imageInfo = await Image.IdentifyAsync(path);
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
        public async Task GetExpectedData(ObservableCollection<MyImage> images, SizeScale scale, CompressLevel compressLevel)
        {
            foreach (MyImage image in images)
            {
                byte[] originalData = await File.ReadAllBytesAsync(image.FilePath);
                byte[] currentData = originalData;

                if (scale != SizeScale.None)
                {
                    image.ExpectedWidthHeight = scaler.ConvertToNewSize((image.Width, image.Height), scale);
                    currentData = await imageResize.ResizeJPG(image.FilePath, scale);
                }
                if (compressLevel != CompressLevel.None)
                {
                    byte [] compressedSize = await imageCompressor.JPGCompress(image.FilePath, compressLevel);
                    image.ExpectedSize = imageCompressor.GetBytesReadable(compressedSize.Length);
                }
                else
                {
                    image.ExpectedSize = $"{imageCompressor.GetBytesReadable(currentData.Length)}";
                }
            }
        }
    }
}


