namespace Contacts.WebUi.Models
{
    public class Settings : ISettings
    {
        public string ApiRootUri { get; set; }
        public string ApiScopeUri { get; set; }
        public string AppInsightsKey { get; set; }
        public string ContactBlobStorageAccount { get; set; }
        public string ContactBlobStorageAccountName { get; set; }
        public string ContactImageContainerName { get; set; }
        public string ContactImageUrl { get; set; }
        public string ContactThumbnailBlobStorageAccountName { get; set; }
        public string ContactThumbnailImageContainerName { get; set; }

        public string ThumbnailQueueName { get; set; }
        public string ThumbnailQueueStorageAccount { get; set; }
        public string ThumbnailQueueStorageAccountName { get; set; }
    }
}