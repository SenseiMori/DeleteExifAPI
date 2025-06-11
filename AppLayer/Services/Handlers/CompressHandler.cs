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
using ExifDeleteLib;
using AppLayer.Model.Entities;
using ModifierCore.Core.ImageManipulation;
using ModifierCore.Core.Const;
using AppLayer.Model.Interfaces;
using AppLayer.ViewModel;
using Windows.ApplicationModel.Background;


namespace AppLayer
{
    public class CompressHandler : IImageHandler
    {
        ImageCompressor _compressor = new ImageCompressor();
        CompressLevel _compressLevel = new CompressLevel();
        private readonly IMainViewModel _mainViewModel;
        public CompressHandler(IMainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        public byte [] Handler (byte[] originData)
        {
            byte[] data = originData;

            if (_mainViewModel.IsExtraCompress)
            {
                data = _compressor.JPGCompress(originData, CompressLevel.Extra);
            }
            else if (_mainViewModel.IsNormalCompress)
            {
                data = _compressor.JPGCompress(originData, CompressLevel.Normal);
            }
            else if (_mainViewModel.IsBestCompress)
            {
                data = _compressor.JPGCompress(originData, CompressLevel.Best);
            }
            return data;

        }
   
        
    }
}
