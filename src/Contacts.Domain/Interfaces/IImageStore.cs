using System.IO;
using System.Threading.Tasks;

namespace Contacts.Domain.Interfaces
{
    public interface IImageStore
    {
        Task<string> SaveImageAsync(Stream imageStream, string filename = null, bool overwriteIfExists = true);
        string GetBlobContainerName();
        Task<Stream> GetImageAsync(string filename);
    }
}