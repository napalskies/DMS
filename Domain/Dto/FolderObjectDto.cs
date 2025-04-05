namespace MyDMS.Domain.Dto
{
    public class FolderObjectDto
    {
        public string FolderId { get; set; }
        public string FolderName { get; set; }
        public int FolderType { get; set; }
        public List<FolderObjectDto> SubFolders { get; set; }
        public IEnumerable<string> Documents { get; set; }
    }
}
