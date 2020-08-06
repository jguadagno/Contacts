namespace Contacts.WebUi.Models
{
    public interface ISettings
    {
        string ApiRootUri { get; set; }
        string ApiScopeUri { get; set; }
        string AppInsightsKey { get; set; }
        string ContactBlobStorageAccount { get; set; }
        string ContactImageContainerName { get; set; }
        string ContactImageUrl { get; set; }
    }
}