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
using ModifierCore.Core.Const;
using ModifierCore.Core.ImageManipulation;
using AppLayer.Model.Interfaces;
using AppLayer.ViewModel;

namespace AppLayer.Services.Handlers.ModifierHandlers
{
    public class ResizeHandler : IImageHandler
    {
        ImageResize _resize = new ImageResize();
        IMainViewModel _mainViewModel;
        public ResizeHandler(IMainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }
        public byte[] Handler(byte[] originData)
        {
            byte[] data = originData;
            if (_mainViewModel.IsExtraResolution)
            {
                data = _resize.ResizeJPG(originData, SizeScale.Extra);
            }
            else if (_mainViewModel.IsNormalResolution)
            {
                data = _resize.ResizeJPG(originData, SizeScale.Normal);
            }
            else if (_mainViewModel.IsBestResolution)
            {
                data = _resize.ResizeJPG(originData, SizeScale.Best);
            }
            return data;
        }
    }
}
