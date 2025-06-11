using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using AppLayer.Model.Entities;

namespace AppLayer.Model.Interfaces
{
    interface IDataProvider
    {
        MyImage GetInfo(string image);
    }
}
