using System.Threading.Tasks;

namespace AppCore.Services.RemoveMetadata
{
    interface IFileManager
    {
        string Save(byte[] data, string originalPath);
        //void Add();
        //void Delete();
    }
    interface IFileManagerAsync
    {
        Task<string> Save(byte[] data, string originalPath);
        //void Add();
        //void Delete();
    }
}
