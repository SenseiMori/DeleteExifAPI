using System.Threading.Tasks;
using AppCore.Model.Interfaces;
using DeleteExifCore.Core.JPG;

namespace AppCore.Services.Handlers.ModifierHandlers
{
    public class RemoveEXIFHandler : IImageHandlerAsync
    {
        JPGMetadataReader reader = new();

        public async Task <byte[]> Handler(string path)
        {
            byte[] data = await reader.DeleteExifMarkers(path);
            return data;



        }
            
    }
}
