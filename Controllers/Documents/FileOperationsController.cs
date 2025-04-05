using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using MyDMS.Application;
using MyDMS.Application.FileStorage;
using MyDMS.Domain;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace MyDMS.Controllers.Documents
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileOperationsController : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService;

        public FileOperationsController(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        [HttpPost("upload")]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload(IFormFile file, [FromForm]string folderId)
        {
            var guid = await _fileStorageService.UploadFileAsync(file, folderId);
            return Ok($"File uploaded as {guid}");
        }

        [HttpGet("download")]
        public async Task<IActionResult> Download(string guid)
        {
            var file = await _fileStorageService.DownloadFileAsync(guid);
            return File(file.FileStream, file.ContentType);
        }

        [HttpGet("download-all")]
        public async Task<IActionResult> DownloadAll(string? folderId)
        {
            var files = await _fileStorageService.DownloadAllFilesAsync(folderId);
            return Ok(files);
        }
    }
}
