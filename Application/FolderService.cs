using MyDMS.Application.FileStorage;
using MyDMS.Domain;
using MyDMS.Domain.Dto;
using MyDMS.Domain.Enums;
using MyDMS.Infrastructure;

namespace MyDMS.Application
{
    public class FolderService
    {
        private FolderRepository _folderRepository;
        private IFileStorageService _fileStorageService;

        public FolderService(FolderRepository folderRepository, IFileStorageService fileStorageService)
        {
            _folderRepository = folderRepository;
            _fileStorageService = fileStorageService;
        }

        public async Task CreateFolderAsync(FolderDto folderDto)
        {
            Folder folder = new Folder
            {
                FolderId = folderDto.FolderId,
                FolderName = ((FolderTypes)folderDto.FolderType).ToString() + '_' + folderDto.FolderId,
                FolderOwnerId = folderDto.FolderOwnerId,
                FolderType = folderDto.FolderType,
                CreateDateTime = DateTime.Now
            };
            await _folderRepository.CreateFolderAsync(folder);
        }

        public async Task<FolderObjectDto> GetFolderAsync(string folderId, int folderType)
        {
            var folderName = _folderRepository.GetFolderName(folderId);
            var documents = await _fileStorageService.DownloadAllFilesAsync(folderId);
            var folders = await _folderRepository.GetFoldersAsync(folderId);
            List<FolderObjectDto> subfolders = new List<FolderObjectDto>();

            foreach (var f in folders)
            {
                subfolders.Add(await GetFolderAsync(f.FolderId, f.FolderType));
            }

            FolderObjectDto folderObject = new FolderObjectDto
            {
                FolderId = folderId,
                FolderName = folderName,
                FolderType = folderType,
                Documents = documents,
                SubFolders = subfolders
            };

            return folderObject;
        }

        public async Task<List<Folder>> GetFoldersAsync(string ownerId)
        {
            return await _folderRepository.GetFoldersAsync(ownerId);
        }

        public async Task UpdateFolderAsync(Folder folder)
        {
            await _folderRepository.UpdateFolderAsync(folder);
        }

        public async Task DeleteFolderAsync(string folderId)
        {
            await _folderRepository.DeleteFolderAsync(folderId);
        }
    }
}
