using MyDMS.Domain.Dto;

namespace MyDMS.Application.FileStorage
{
    public interface IFileStorageService
    {
        Task<string> UploadFileAsync(IFormFile file, string folderId);
        Task<DocumentDto> DownloadFileAsync(string fileName);

        Task<IEnumerable<string>> DownloadAllFilesAsync(string userId);
    }
}
