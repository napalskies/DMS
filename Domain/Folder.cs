namespace MyDMS.Domain
{
    public class Folder
    {
        public string FolderId { get; set; }
        public int FolderType { get; set; }
        public string FolderName { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string? FolderOwnerId { get; set; }
        public Folder? FolderOwner { get; set; }
    }
}
