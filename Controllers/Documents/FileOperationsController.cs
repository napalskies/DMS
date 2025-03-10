using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
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
        private readonly UserManager<ApplicationUser> _userManager;

        public FileOperationsController(IFileStorageService fileStorageService, UserManager<ApplicationUser> userManager)
        {
            _fileStorageService = fileStorageService;
            _userManager = userManager;
        }

        [HttpPost("upload")]
        [Authorize]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var u = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.Write("user: " + u);
            var filepath = await _fileStorageService.UploadFileAsync(file, u);
            return Ok($"File uploaded at {filepath}");
        }

        [HttpGet("download")]
        public async Task<IActionResult> Download(string guid)
        {
            var file = await _fileStorageService.DownloadFileAsync(guid);
            //var fileStream = new MemoryStream(file.FileData);
            // Download file from server
            //return File(fileStream, file.ContentType);// Bytes, file.ContentType);
            return File(file.FileStream, file.ContentType);
        }
    }
}
