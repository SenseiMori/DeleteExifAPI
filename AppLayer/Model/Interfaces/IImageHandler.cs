using System.Threading.Tasks;

namespace AppCore.Model.Interfaces
{
    public interface IImageHandler
    {
        byte[] Handler(byte[] data);
    }

    public interface IImageHandlerAsync
    {
       Task<byte[]> Handler(string path);
    }
}
