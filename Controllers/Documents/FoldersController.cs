using Microsoft.AspNetCore.Mvc;
using MyDMS.Application;
using MyDMS.Application.FileStorage;
using MyDMS.Domain;
using MyDMS.Domain.Dto;

namespace MyDMS.Controllers.Documents
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoldersController : ControllerBase
    {
        private readonly FolderService _folderService;

        public FoldersController(FolderService folderService)
        {
            _folderService = folderService;
        }

        [HttpGet("folder")]
        public async Task<IActionResult> Get(string folderId, int folderType) {
            var folder = await _folderService.GetFolderAsync(folderId, folderType);    
            return Ok(folder);
        }

        [HttpGet("folders")]
        public async Task<IActionResult> Get(string ownerId)
        {
            var folders = await _folderService.GetFoldersAsync(ownerId);
            return Ok(folders);
        }

        [HttpPost("folder")]
        public async Task<IActionResult> Post(FolderDto folder)
        {
            Console.WriteLine(folder);
            if (folder.FolderType == 1)
                folder.FolderOwnerId = null;
            await _folderService.CreateFolderAsync(folder);
            return Ok();
        }
    }
}
