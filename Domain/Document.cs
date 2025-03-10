namespace MyDMS.Domain
{
    public class Document
    {
        public Guid DocumentId { get; set; }
        public int DocumentType { get; set; }
        public DateTime CreateDateTime { get; set; }
        public byte[] FileData { get; set; }
        public string ContentType { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
