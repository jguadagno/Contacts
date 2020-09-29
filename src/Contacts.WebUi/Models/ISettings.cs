namespace Contacts.WebUi.Models
{
    public interface ISettings
    {
        string ApiRootUri { get; set; }
        string ApiScopeUri { get; set; }
        string AppInsightsKey { get; set; }
        string ContactBlobStorageAccount { get; set; }
        string ContactBlobStorageAccountName { get; set; }
        string ContactImageContainerName { get; set; }
        string ContactImageUrl { get; set; }    
        string ContactThumbnailBlobStorageAccountName { get; set; }
        string ContactThumbnailImageContainerName { get; set; }
        string ThumbnailQueueName { get; set; }
        string ThumbnailQueueStorageAccount { get; set; }
        string ThumbnailQueueStorageAccountName { get; set; }
        
    }
}