namespace Contacts.WebUi.Models
{
    public class Settings : ISettings
    {
        public string ApiRootUri { get; set; }
        public string ApiScopeUri { get; set; }
        public string AppInsightsKey { get; set; }
    }
}