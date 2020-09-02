using System.Threading.Tasks;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Contacts.Functions.ThumbnailCreator
{
    public static class CreateThumbnailImage
    {
        [FunctionName("CreateThumbnailImage")]
        public static async Task RunAsync([QueueTrigger("thumbnail-create", Connection = "ThumbnailQueueStorageAccount")]
            Domain.Models.Messages.ImageToConvert imageToConvert, ILogger log)
        {
            log.LogDebug($"Creating Thumbnail for contact id '{imageToConvert.ContactId}'.");

            // Get the Image To Convert
            var contactImageContainer =
                new JosephGuadagno.AzureHelpers.Storage.Blobs("UseDevelopmentStorage=true",
                    imageToConvert.ContainerName);
            var imageToConvertStream = new MemoryStream();
            var wasDownloaded =
                await contactImageContainer.DownloadToAsync(imageToConvert.ImageName, imageToConvertStream);

            if (wasDownloaded == false)
            {
                log.LogCritical($"Could not download the source image for contact id of '{imageToConvert.ContactId}'");
                return;
            }
            
            // Create the Thumbnail
            var sourceImage = await Image.LoadAsync(imageToConvertStream);
            sourceImage.Mutate(x => x.Resize(120,120));
            var thumbnailStream = new MemoryStream();
            await sourceImage.SaveAsync(thumbnailStream, new JpegEncoder());
            
            // Upload New Thumbnail
            var contactThumbnailContainer =
                new JosephGuadagno.AzureHelpers.Storage.Blobs("UseDevelopmentStorage=true",
                    "contact-images-thumbnails");
            var thumbnailImageBlobInfo =
                await contactThumbnailContainer.UploadAsync(imageToConvert.ImageName, imageToConvertStream, true);

            log.LogDebug($"Saved thumbnail for contact id '{imageToConvert.ContactId}' was '{thumbnailImageBlobInfo}'");
        }
    }
}