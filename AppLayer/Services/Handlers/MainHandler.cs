using AppLayer.Model.Entities;
using AppLayer.Model.Interfaces;
using AppLayer.Services.SaveFileService;
using AppLayer.ViewModel;
using ExifDeleteLib;
using ModifierCore.Core.Const;
using ModifierCore.Core.ImageManipulation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLayer.Services.Handlers
{
    public class MainHandler
    {
        ObservableCollection<MyImage> _images {get; set;}

        List<IImageHandler> imageHandlers = new List<IImageHandler> ();
        DirectoryService directoryService = new DirectoryService ();

        ImageCompressor _compressor = new ImageCompressor();
        JPGMetadataReader reader = new JPGMetadataReader();
        ImageResize _resize = new ImageResize();
        IMainViewModel _mainViewModel;
        public MainHandler(ObservableCollection<MyImage> images, IMainViewModel main)
        {
            _images = images;
            _mainViewModel = main;
            
        }

        public List<IImageHandler> GetHandlers ()
        {
            if (_mainViewModel.IsResize)
               imageHandlers.Add(new ResizeHandler(_mainViewModel));
            if (_mainViewModel.IsCompress)
               imageHandlers.Add(new CompressHandler(_mainViewModel));
            if(_mainViewModel.IsRemove) 
                imageHandlers.Add(new RemoveEXIFHandler());
            return imageHandlers;
        }

        public void Processing (string path)
        {
            List <IImageHandler> handlers = GetHandlers();

            byte[] data = File.ReadAllBytes(path);
            byte[] buffer = data;
                foreach (IImageHandler handler in handlers)
                {
                    byte [] newData = handler.Handler(buffer);
                    directoryService.SaveModifierFile(newData, path);
                    buffer = newData;
                }
            handlers.Clear();
            
        }
    }
}
