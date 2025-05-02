using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRemoveExif.Services.RemoveMetadata
{
    interface IExifJpgRemover
    {
        void Remove(string key);
    }
}
