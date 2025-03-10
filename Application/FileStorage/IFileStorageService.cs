using MyDMS.Domain.Dto;

namespace MyDMS.Application.FileStorage
{
    public interface IFileStorageService
    {
        Task<string> UploadFileAsync(IFormFile file, string userId);
        Task<DocumentDto> DownloadFileAsync(string fileName);
    }
}
