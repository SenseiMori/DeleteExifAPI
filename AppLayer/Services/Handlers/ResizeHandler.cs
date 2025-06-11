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

namespace AppLayer
{
    public class ResizeHandler: IImageHandler
    {
        ImageResize _resize = new ImageResize();
        //MainViewModel _mainViewModel { get; }
        IMainViewModel _mainViewModel;
        public ResizeHandler(IMainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }



        public byte[] Handler (byte[] originData)
        {
            byte[] data = originData;
            if (_mainViewModel.IsExtraWeight)
            {
                data = _resize.ResizeJPG(originData, Weight.Extra);
            }
            else if(_mainViewModel.IsNormalWeight)
            {
                data = _resize.ResizeJPG(originData, Weight.Normal);
            }
            else if (_mainViewModel.IsBestWeight)
            {
                data = _resize.ResizeJPG(originData, Weight.Best);
            }
            return data;
        }


        
    }
}
