using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TextRemoveExif.Model.Entities;

namespace TextRemoveExif.Model.Interfaces
{
    interface IDataProvider
    {
        MyImage GetInfo(string image);
    }
}
