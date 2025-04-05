
using Microsoft.AspNetCore.Http.HttpResults;
using MyDMS.Domain.Dto;
using MyDMS.Infrastructure;

namespace MyDMS.Application.FileStorage
{
    public class DbFileStorageService : IFileStorageService
    {

        private IFileRepository _fileRepository;
        private string _uploadPath;

        public DbFileStorageService(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
            _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folderId)
        {
            Guid guid = Guid.NewGuid();
            
            /*
            string uniqueFileName = guid.ToString() + "_" + file.FileName;
            string filePath = Path.Combine(_uploadPath, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }*/

            var fileData = await EncryptionService.EncryptFile(file);

           
            Domain.Document document = new Domain.Document
            {
                DocumentId = guid,
                DocumentType = 1,
                CreateDateTime = DateTime.Now,
                FileData = fileData,
                ContentType = file.ContentType,
                FolderId = folderId
            };
            
            await _fileRepository.UploadFileAsync(document);

            return guid.ToString();
        }

        public async Task<DocumentDto> DownloadFileAsync(string guid)
        {
            var file = await _fileRepository.DownloadFileAsync(guid);
            var decryptedData = EncryptionService.DecryptFile(file.FileData);

            var fileStream = new MemoryStream(decryptedData);

            return new DocumentDto
            {
                FileStream = fileStream,
                ContentType = file.ContentType
            };
        }

        public async Task<IEnumerable<string>> DownloadAllFilesAsync(string folderId)
        {
            var files = await _fileRepository.DownloadAllFilesAsync(folderId);
            var docIdList = files.Select(f => f.DocumentId.ToString());
            return docIdList;
        }

        
    }
}
