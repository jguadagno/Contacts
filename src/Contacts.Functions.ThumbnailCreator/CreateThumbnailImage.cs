using System;
using System.Threading.Tasks;
using Contacts.Domain.Interfaces;
using Contacts.Functions.ThumbnailCreator.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Contacts.Functions.ThumbnailCreator
{
    public class CreateThumbnailImage
    {
        private readonly ISettings _settings;
        private readonly IImageManager _imageManager;

        public CreateThumbnailImage(ISettings settings, IImageManager imageManager)
        {
            _settings = settings;
            _imageManager = imageManager;
        }
        
        [FunctionName("CreateThumbnailImage")]
        public async Task RunAsync([QueueTrigger("thumbnail-create")]
            Domain.Models.Messages.ImageToConvert imageToConvert, ILogger log)
        {
            log.LogDebug("Creating thumbnail of '{ImageName}' in container '{ContainerName}'", imageToConvert.ImageName, imageToConvert.ContainerName);

            var imageUrl = await _imageManager.CreateThumbnailImageAsync(imageToConvert.ImageName,
                _settings.ResizeWidthSize, _settings.ResizeHeightSize);

            log.LogDebug("Saved thumbnail of '{ImageName}' to {ImageUrl}'", imageToConvert.ImageName, imageUrl);
        }

        private static bool IsDevelopment()
        {
            return Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT") == "Development";
        }
    }
}