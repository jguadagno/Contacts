namespace Contacts.Functions.ThumbnailCreator.Models
{
    public interface ISettings
    {
        public string ContactBlobStorageAccount { get; }
        public string ContactBlobStorageAccountName { get;  }
        public string ContactImageContainerName { get;  }
        public string ThumbnailQueueName { get;  }
        public string ThumbnailQueueStorageAccount { get;  }
        public string ThumbnailQueueStorageAccountName { get;  }
        public string ContactThumbnailBlobStorageAccount { get;  }
        public string ContactThumbnailBlobStorageAccountName { get;  }
        public string ContactThumbnailImageContainerName { get;  }
        public int ResizeHeightSize { get;  }
        public int ResizeWidthSize { get;  }
    }
}