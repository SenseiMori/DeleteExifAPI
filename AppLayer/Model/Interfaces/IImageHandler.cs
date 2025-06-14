using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLayer.Model.Interfaces
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
