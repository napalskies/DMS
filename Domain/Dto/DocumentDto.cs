namespace MyDMS.Domain.Dto
{
    public class DocumentDto
    {
        public Stream FileStream { get; set; }
        public string ContentType { get; set; } 
        public string FileName { get; set; }
    }
}
