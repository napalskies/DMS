namespace MyDMS.Infrastructure
{
    public interface IFileRepository
    {
        public Task UploadFileAsync(Domain.Document document);
        public Task<Domain.Document> DownloadFileAsync(string filePath);
        public Task<List<Domain.Document>> DownloadAllFilesAsync(string userId);
    }
}
