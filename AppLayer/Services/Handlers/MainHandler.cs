using AppCore.Model.Entities;
using AppCore.Model.Interfaces;
using AppCore.Services.SaveFileService;
using DeleteExifCore.Core.JPG;
using ModifierCore.Core.ImageManipulation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AppCore.Services.Handlers.ModifierHandlers
{
    public class MainHandler
    {
        List<IImageHandlerAsync> imageHandlers = new ();
        ImageSaveService saveService = new ();

        ImageCompressor _compressor = new ();
        JPGMetadataReader reader = new ();
        ImageResize _resize = new ();

        ObservableCollection<MyImage> _images { get; set; }
        IMainViewModel _mainViewModel;
        public MainHandler(ObservableCollection<MyImage> images, IMainViewModel main)
        {
            _images = images;
            _mainViewModel = main;
        }
        public List<IImageHandlerAsync> GetHandlers()
        {
            if (_mainViewModel.IsResize)
                imageHandlers.Add(new ResizeHandler(_mainViewModel));
            if (_mainViewModel.IsCompress)
                imageHandlers.Add(new CompressHandler(_mainViewModel));
            if (_mainViewModel.IsRemove)
               imageHandlers.Add(new RemoveEXIFHandler());
            return imageHandlers;
        }
        public async Task Processing(string path)
        {
            List<IImageHandlerAsync> handlers = GetHandlers();

            foreach (IImageHandlerAsync handler in handlers)
            {
                byte[] newData = await handler.Handler(path);
                saveService.Save(newData, path);
            }
            handlers.Clear();
        }
    }
}
