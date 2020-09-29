using System;
using System.Threading.Tasks;
using System.IO;
using Contacts.Domain.Interfaces;
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
            log.LogDebug($"Creating thumbnail of '{imageToConvert.ImageName}' in container '{imageToConvert.ContainerName}'.");

            var imageUrl = await _imageManager.CreateThumbnailImageAsync(imageToConvert.ImageName,
                _settings.ResizeWidthSize, _settings.ResizeHeightSize);

            log.LogDebug($"Saved thumbnail of '{imageToConvert.ImageName}' to '{imageUrl}'");
        }

        private static bool IsDevelopment()
        {
            return Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT") == "Development";
        }
    }
}