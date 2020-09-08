using System;
using System.Threading.Tasks;
using System.IO;
using Contacts.Functions.ThumbnailCreator.Models;
using JosephGuadagno.AzureHelpers.Storage;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Contacts.Functions.ThumbnailCreator
{
    public class CreateThumbnailImage
    {
        private readonly ISettings _settings;

        public CreateThumbnailImage(ISettings settings)
        {
            _settings = settings;
        }
        
        [FunctionName("CreateThumbnailImage")]
        public async Task RunAsync([QueueTrigger("thumbnail-create")]
            Domain.Models.Messages.ImageToConvert imageToConvert, ILogger log)
        {
            log.LogDebug($"Creating Thumbnail for contact id '{imageToConvert.ContactId}'.");

            // Get the Image To Convert
            Blobs contactImageContainer = IsDevelopment()
                ? new Blobs(_settings.ContactBlobStorageAccount,
                    imageToConvert.ContainerName)
                : new Blobs(_settings.ContactBlobStorageAccountName, null,
                    imageToConvert.ContainerName);

            var imageToConvertStream = new MemoryStream();
            var wasDownloaded =
                await contactImageContainer.DownloadToAsync(imageToConvert.ImageName, imageToConvertStream);

            if (wasDownloaded == false)
            {
                log.LogCritical($"Could not download the source image for contact id of '{imageToConvert.ContactId}'");
                return;
            }
            imageToConvertStream.Position = 0; // Need to reset because Azure SDK does not close the stream
            
            // Create the Thumbnail
            var sourceImage = await Image.LoadAsync(imageToConvertStream);
            sourceImage.Mutate(x => x.Resize(_settings.ResizeWidthSize, _settings.ResizeHeightSize));
            var thumbnailStream = new MemoryStream();
            await sourceImage.SaveAsync(thumbnailStream, new JpegEncoder());
            thumbnailStream.Position = 0; // Need to reset the position to 0 so that Azure SDK can upload it
            
            // Upload New Thumbnail
            Blobs contactThumbnailContainer = IsDevelopment()
                ? new Blobs(_settings.ContactThumbnailBlobStorageAccount,
                    _settings.ContactThumbnailImageContainerName)
                : new Blobs(_settings.ContactThumbnailBlobStorageAccountName, null,
                    _settings.ContactThumbnailImageContainerName);
            
            var thumbnailImageBlobInfo =
                await contactThumbnailContainer.UploadAsync(imageToConvert.ImageName, thumbnailStream, true);

            log.LogDebug($"Saved thumbnail for contact id '{imageToConvert.ContactId}' was '{thumbnailImageBlobInfo!=null}'");
        }

        private static bool IsDevelopment()
        {
            return Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT") == "Development";
        }
    }
}