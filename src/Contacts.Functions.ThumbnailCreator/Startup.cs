using Contacts.Functions.ThumbnailCreator;
using Contacts.Functions.ThumbnailCreator.Models;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Contacts.Functions.ThumbnailCreator
{
    public class Startup: FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.TryAddSingleton<ISettings>(s => new Settings());
        }
    }
}