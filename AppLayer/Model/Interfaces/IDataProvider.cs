using System.Threading.Tasks;
using AppCore.Model.Entities;

namespace AppCore.Model.Interfaces
{
    interface IDataProvider
    {
        MyImage GetInfo(string image);
    }
    interface IDataProviderAsync
    {
        Task <MyImage> GetInfo(string image);
    }
}
