using System.IO;
using System.Threading.Tasks;

namespace Contacts.Domain.Interfaces
{
    public interface IImageManager
    {
        Task<string> SaveImageAsync(Stream imageStream, string filename = null);
        Task<string> CreateThumbnailImageAsync(string filename, int resizeWidth = 120, int resizeHeight = 120);
    }
}