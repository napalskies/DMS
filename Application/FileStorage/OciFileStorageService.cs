using Microsoft.Extensions.Options;
using MyDMS.Domain.Dto;
using Oci.Common.Auth;
using Oci.Common;
using Oci.ObjectstorageService;
using Oci.ObjectstorageService.Requests;
using System;
using System.Text.Json;
using System.Text;

namespace MyDMS.Application.FileStorage
{
    public class OciFileStorageService : IFileStorageService
    {
        public Task<IEnumerable<string>> DownloadAllFilesAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<DocumentDto> DownloadFileAsync(string fileName)
        {
            var configFilePath = Path.GetFullPath(@"C:\Users\AU102240\.oci\config.file");
            var config = ConfigFileReader.Parse(configFilePath);
            var provider = new ConfigFileAuthenticationDetailsProvider(config);

            var _osClient = new ObjectStorageClient(provider);

            var getObjectRequest = new GetObjectRequest()
            {
                BucketName = "my-bucket",
                NamespaceName = "axrurnrhfmhr",
                ObjectName = fileName
            };

            var response = _osClient.GetObject(getObjectRequest).GetAwaiter().GetResult();

            using (var streamReader = new StreamReader(response.InputStream))
            {
                var documentJson = await streamReader.ReadToEndAsync();
                var document = JsonSerializer.Deserialize<Domain.Document>(documentJson);
                document.FileData = EncryptionService.DecryptFile(document.FileData);

                var dataStream = new MemoryStream(document.FileData);
                return new DocumentDto
                {
                    FileStream = dataStream,
                    ContentType = document.ContentType
                };
            }
        }

        public async Task<string> UploadFileAsync(IFormFile file, string userId)
        {
            //var provider = new ConfigFileAuthenticationDetailsProvider(DMSConfig.Profile);
            //var _osClient = new ObjectStorageClient(provider, new ClientConfiguration());
            var configFilePath = Path.GetFullPath(@"C:\Users\AU102240\.oci\config.file");
            var config = ConfigFileReader.Parse(configFilePath);
            var provider = new ConfigFileAuthenticationDetailsProvider(config);

            var _osClient = new ObjectStorageClient(provider);

            Domain.Document document = new Domain.Document
            {
                DocumentId = Guid.NewGuid(),
                DocumentType = 1,
                CreateDateTime = DateTime.Now,
                FileData = await EncryptionService.EncryptFile(file),
                ContentType = file.ContentType,
                UserId = userId
            };

            var jsonDocument = JsonSerializer.Serialize(document);

            var putObjectRequest = new PutObjectRequest()
            {
                BucketName = "my-bucket",
                NamespaceName = "axrurnrhfmhr",
                ObjectName = file.FileName,
                PutObjectBody = new MemoryStream(Encoding.UTF8.GetBytes(jsonDocument)),
                ContentLength = Encoding.UTF8.GetByteCount(jsonDocument)
            };

            try
            {
                var response = _osClient.PutObject(putObjectRequest).Result;
                return $"file uploaded;{response.ToString()}";
            }
            catch (Exception e)
            {
                return e.InnerException.Message;
            }
        }
    }
}
