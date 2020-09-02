using System.Threading.Tasks;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

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
            var originalImageBlobInfo =
                await contactImageContainer.DownloadToAsync(imageToConvert.ImageName, imageToConvertStream);
            
            // Create the Thumbnail
            // TODO: Fix the thumbnail creation, the image is empty
            Image newImage=GetReducedImage(32,32, imageToConvertStream);
            var thumbnailStream = new MemoryStream();
            newImage.Save(thumbnailStream, ImageFormat.Jpeg);
            
            // Upload New Thumbnail
            var contactThumbnailContainer =
                new JosephGuadagno.AzureHelpers.Storage.Blobs("UseDevelopmentStorage=true",
                    "contact-images-thumbnails");
            var thumbnailImageBlobInfo =
                await contactThumbnailContainer.UploadAsync(imageToConvert.ImageName, imageToConvertStream, true);

            log.LogDebug($"Saved thumbnail for contact id '{imageToConvert.ContactId}' was '{thumbnailImageBlobInfo}'");
        }

        private static Image GetReducedImage(int width, int height, Stream resourceImage)
        {
            try
            {
                Image image = Image.FromStream(resourceImage);
                Image thumb = image.GetThumbnailImage(width, height, () => false, IntPtr.Zero);

                return thumb;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        
        private static Stream GetStream(Image img, ImageFormat format)
        {
            var ms = new MemoryStream();
            img.Save(ms, format);
            return ms;
        }
    }
}