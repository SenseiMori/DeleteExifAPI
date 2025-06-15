using AppLayer.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLayer.Services.Handlers.ModifierHandlers
{
    class Context
    {
        private IImageHandler _imageHandler;
        private readonly List<IImageHandler> _imageHandlers;
        public Context(IImageHandler imageHandler)
        {
            _imageHandler = imageHandler;
        }
        public Context(List<IImageHandler> imageHandlers)
        {
            _imageHandlers = imageHandlers;
        }
        public void SetProcessStrategy(IImageHandler imageHandler)
        {
            _imageHandler = imageHandler;
        }
        public byte[] ImageProcess(byte[] data)
        {
            return _imageHandler.Handler(data);
        }
    }
}
