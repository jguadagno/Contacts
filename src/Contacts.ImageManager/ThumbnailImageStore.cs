using Contacts.Domain.Interfaces;
using JosephGuadagno.AzureHelpers.Storage;

namespace Contacts.ImageManager
{
    public class ThumbnailImageStore: ImageStore, IThumbnailImageStore
    {
        public ThumbnailImageStore(Blobs blobs) : base(blobs)
        {
        }

    }
}