namespace Contacts.Domain.Models.Messages
{
    public class ImageToConvert
    {
        public int ContactId { get; set; }
        public string ContainerName { get; set; }
        public string ImageName { get; set; }
    }
}