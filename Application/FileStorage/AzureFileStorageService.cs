using MyDMS.Domain.Dto;

namespace MyDMS.Application.FileStorage
{
    public class AzureFileStorageService : IFileStorageService
    {
        public Task<DocumentDto> DownloadFileAsync(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<string> UploadFileAsync(IFormFile file, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
