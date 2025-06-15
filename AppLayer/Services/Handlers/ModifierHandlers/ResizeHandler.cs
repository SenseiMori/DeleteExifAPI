using System;
using System.Threading.Tasks;
using ModifierCore.Core.Const;
using ModifierCore.Core.ImageManipulation;
using AppCore.Model.Interfaces;

namespace AppCore.Services.Handlers.ModifierHandlers
{
    public class ResizeHandler : IImageHandlerAsync
    {
        ImageResize _resize = new ();

        IMainViewModel _mainViewModel;
        public ResizeHandler(IMainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }
        public async Task <byte[]> Handler(string path)
        {
            byte[] data = Array.Empty<byte>();
            if (_mainViewModel.IsExtraResolution)
            {
                data = await _resize.ResizeJPG(path, SizeScale.Extra);
            }
            else if (_mainViewModel.IsNormalResolution)
            {
                data = await _resize.ResizeJPG(path, SizeScale.Normal);
            }
            else if (_mainViewModel.IsBestResolution)
            {
                data = await _resize.ResizeJPG(path, SizeScale.Best);
            }
            return data;
        }
    }
}
