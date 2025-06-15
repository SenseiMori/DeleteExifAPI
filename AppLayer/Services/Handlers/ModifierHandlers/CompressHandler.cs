using System;
using System.Threading.Tasks;
using ModifierCore.Core.ImageManipulation;
using ModifierCore.Core.Const;
using AppCore.Model.Interfaces;

namespace AppCore.Services.Handlers.ModifierHandlers
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
