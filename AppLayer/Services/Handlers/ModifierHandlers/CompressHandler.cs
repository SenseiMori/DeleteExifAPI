using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using AppLayer.Model.Entities;
using ModifierCore.Core.ImageManipulation;
using ModifierCore.Core.Const;
using AppLayer.Model.Interfaces;
using AppLayer.ViewModel;
using Windows.ApplicationModel.Background;

namespace AppLayer.Services.Handlers.ModifierHandlers
{
    public class CompressHandler : IImageHandlerAsync
    {
        ImageCompressor _compressor = new();
        CompressLevel _compressLevel = new();

        private readonly IMainViewModel _mainViewModel;
        public CompressHandler(IMainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }
        public async Task<byte[]> Handler(string path)
        {
            byte[] data = Array.Empty<byte>();

            if (_mainViewModel.IsExtraCompress)
            {
                data = await _compressor.JPGCompress(path, CompressLevel.Extra);
            }
            else if (_mainViewModel.IsNormalCompress)
            {
                data = await _compressor.JPGCompress(path, CompressLevel.Normal);
            }
            else if (_mainViewModel.IsBestCompress)
            {
                data = await _compressor.JPGCompress(path, CompressLevel.Best);
            }
            return data;
        }
    }
}
