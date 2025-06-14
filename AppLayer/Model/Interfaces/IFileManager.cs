using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLayer.Services.RemoveMetadata
{
    interface IFileManager
    {
        string Save(byte[] data, string originalPath);
        //void Add();
        //void Delete();
    }
}
