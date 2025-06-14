using AppLayer.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLayer.Services.Handlers.ModifierHandlers
{
    class ContextProcessing
    {
        private IImageHandler _imageHandler;
        private readonly List<IImageHandler> _imageHandlers;
        public ContextProcessing(IImageHandler imageHandler)
        {
            _imageHandler = imageHandler;
        }
        public ContextProcessing(List<IImageHandler> imageHandlers)
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
