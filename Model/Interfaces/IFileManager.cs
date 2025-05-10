using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRemoveExif.Services.RemoveMetadata
{
    interface IFileManager
    {
        void Add();
        void Save();
        void Delete();
    }
}
