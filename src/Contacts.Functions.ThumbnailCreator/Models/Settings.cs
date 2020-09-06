using System;
using JosephGuadagno.Extensions.Types;

namespace Contacts.Functions.ThumbnailCreator.Models
{
    public class Settings: ISettings
    {
        public string ContactBlobStorageAccount => Environment.GetEnvironmentVariable("ContactBlobStorageAccount");
        public string ContactBlobStorageAccountName  => Environment.GetEnvironmentVariable("ContactBlobStorageAccountName");
        public string ContactImageContainerName  => Environment.GetEnvironmentVariable("ContactImageContainerName");
        public string ThumbnailQueueName  => Environment.GetEnvironmentVariable("ThumbnailQueueName");
        public string ThumbnailQueueStorageAccount  => Environment.GetEnvironmentVariable("ThumbnailQueueStorageAccount");
        public string ThumbnailQueueStorageAccountName  => Environment.GetEnvironmentVariable("ThumbnailQueueStorageAccountName");
        public string ContactThumbnailBlobStorageAccount  => Environment.GetEnvironmentVariable("ContactThumbnailBlobStorageAccount");
        public string ContactThumbnailBlobStorageAccountName  => Environment.GetEnvironmentVariable("ContactThumbnailBlobStorageAccountName");
        public string ContactThumbnailImageContainerName  => Environment.GetEnvironmentVariable("ContactThumbnailImageContainerName");

        public int ResizeHeightSize
        {
            get
            {
                var value = Environment.GetEnvironmentVariable("ResizeHeightSize").ToOrDefault<int>();
                return value == 0 ? 120 : value;
            }
        }

        public int ResizeWidthSize{
            get
            {
                var value = Environment.GetEnvironmentVariable("ResizeWidthSize").ToOrDefault<int>();
                return value == 0 ? 120 : value;
            }
        }
    }
}