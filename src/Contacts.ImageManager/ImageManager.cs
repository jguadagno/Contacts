using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Contacts.Domain.Interfaces;
using JosephGuadagno.AzureHelpers.Storage;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Contacts.ImageManager
{
    public class ImageManager: IImageManager
    {
        private readonly Queue _queue;
        private readonly IImageStore _imageStore;
        private readonly IThumbnailImageStore _thumbnailImageStore;

        public ImageManager(IImageStore imageStore, IThumbnailImageStore thumbnailImageStore, Queue queue)
        {
            _imageStore = imageStore;
            _thumbnailImageStore = thumbnailImageStore;
            _queue = queue;
        }
        
        public async Task<string> SaveImageAsync(Stream imageStream, string filename = null)
        {
            if (imageStream == null || imageStream.Length == 0)
            {
                throw new ArgumentNullException(nameof(imageStream),
                    "Image Stream can not be null or have a length of zero");
            }

            if (string.IsNullOrEmpty(filename))
            {
                var imageFormat = await Image.DetectFormatAsync(imageStream);
                var fileExtension = imageFormat.FileExtensions.First();

                filename = $"{Guid.NewGuid()}.{fileExtension}";
            }

            var imageUrl = await _imageStore.SaveImageAsync(imageStream, filename);
            
            var thumbnailCreateMessage = new Domain.Models.Messages.ImageToConvert
            {
                ContainerName = _imageStore.GetBlobContainerName(),
                ImageName = filename
            };
            
            var sendReceipt = await _queue.AddMessageWithBase64EncodingAsync(thumbnailCreateMessage);
            return imageUrl;
        }

        public async Task<string> CreateThumbnailImageAsync(string filename, int resizeWidth = 120, int resizeHeight = 120)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException(nameof(filename), "The filename name must specified");
            }
            
            // Get Source image from Image Store
            var sourceImageStream = await _imageStore.GetImageAsync(filename);
            if (sourceImageStream == null)
            {
                // TODO: Throw exception? Set return value to null?
                return null;
            }

            // Create Thumbnail
            var sourceImage = await Image.LoadAsync(sourceImageStream);
            sourceImage.Mutate(x => x.Resize(resizeWidth, resizeHeight));
            var thumbnailStream = new MemoryStream();
            await sourceImage.SaveAsync(thumbnailStream, new JpegEncoder());
            thumbnailStream.Position = 0; // Need to reset the position to 0 so that Azure SDK can upload it

            // Save Thumbnail
            var imageUrl = await _thumbnailImageStore.SaveImageAsync(thumbnailStream, filename);

            // Return Thumbnail Url
            return imageUrl;
        }
        
    }
}