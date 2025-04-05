using MyDMS.Domain;
using MyDMS.Infrastructure.Data;

namespace MyDMS.Infrastructure
{
    public class FolderRepository
    {
        private readonly ApplicationDbContext _context;

        public FolderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateFolderAsync(Folder folder)
        {
            _context.Folders.Add(folder);
            await _context.SaveChangesAsync();
        }

        public async Task<Folder> GetFolderAsync(string folderId, int folderType)
        {
            var folder = _context.Folders.FirstOrDefault(f => f.FolderId == folderId && f.FolderType == folderType);
            return folder;
        }

        public string GetFolderName(string folderId)
        {
            return _context.Folders.FirstOrDefault(f => f.FolderId == folderId).FolderName;
        }

        public async Task<List<Folder>> GetFoldersAsync(string ownerId)
        {
            var folders = _context.Folders.Where(f => f.FolderOwnerId == ownerId).ToList();
            return folders;
        }

        public async Task UpdateFolderAsync(Folder folder)
        {
            _context.Folders.Update(folder);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFolderAsync(string folderId)
        {
            var folder = _context.Folders.FirstOrDefault(f => f.FolderId == folderId);
            _context.Folders.Remove(folder);
            await _context.SaveChangesAsync();
        }
    }
}
