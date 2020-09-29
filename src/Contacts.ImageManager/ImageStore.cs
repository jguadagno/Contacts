using System;
using System.IO;
using System.Threading.Tasks;
using Contacts.Domain.Interfaces;
using JosephGuadagno.AzureHelpers.Storage;

namespace Contacts.ImageManager
{
    public class ImageStore: IImageStore
    {
        private readonly Blobs _blobs;

        public ImageStore(Blobs blobs)
        {
            _blobs = blobs;
        }
        
        public async Task<string> SaveImageAsync(Stream imageStream, string filename, bool overwriteIfExists = true)
        {
            var blobContentInfo = await _blobs.UploadAsync(filename, imageStream, overwriteIfExists);
            
            return $"{GetBlobContainerName()}/{filename}"; 
        }

        public string GetBlobContainerName()
        {
            return $"{_blobs.BlobContainerClient.Name}";
        }

        public async Task<Stream> GetImageAsync(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException(nameof(filename), "The filename name must specified");
            }

            var memoryStream = new MemoryStream();
            var wasDownloaded = await _blobs.DownloadToAsync(filename, memoryStream);

            if (wasDownloaded)
            {
                memoryStream.Position = 0;
            }

            return memoryStream;
        }
    }
}