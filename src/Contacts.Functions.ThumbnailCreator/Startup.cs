using System;
using Contacts.Domain.Interfaces;
using Contacts.Functions.ThumbnailCreator;
using Contacts.Functions.ThumbnailCreator.Models;
using Contacts.ImageManager;
using JosephGuadagno.AzureHelpers.Storage;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Contacts.Functions.ThumbnailCreator
{
    public class Startup: FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.TryAddSingleton<ISettings>(s => new Settings());
            
            builder.Services.TryAddSingleton<IImageStore>(provider =>
            {
                var settings = provider.GetService<ISettings>();
                
                var blobs = IsDevelopment()
                    ? new Blobs(settings.ContactBlobStorageAccount, settings.ContactImageContainerName)
                    : new Blobs(settings.ContactBlobStorageAccountName, null, settings.ContactImageContainerName);
                return new ImageStore(blobs);
            });
            
            builder.Services.TryAddSingleton<IThumbnailImageStore>(provider =>
            {
                var settings = provider.GetService<ISettings>();
                var blobs = IsDevelopment()
                    ? new Blobs(settings.ContactBlobStorageAccount, settings.ContactThumbnailImageContainerName)
                    : new Blobs(settings.ContactBlobStorageAccountName, null, settings.ContactThumbnailImageContainerName);
                return new ThumbnailImageStore(blobs);
            });

            builder.Services.TryAddSingleton<IImageManager, ImageManager.ImageManager>();

        }

        private bool IsDevelopment()
        {
            return Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT") == "Development"; 
        }
    }
}