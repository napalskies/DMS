﻿using MyDMS.Infrastructure.Data;
using System.Reflection.Metadata;

namespace MyDMS.Infrastructure
{
    public class FileRepository : IFileRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string _uploadPath;

        public FileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task UploadFileAsync(Domain.Document document)
        {
            _context.Documents.Add(document);
            await _context.SaveChangesAsync();
        }

        public async Task<Domain.Document> DownloadFileAsync(string guid)
        {
            var document = _context.Documents.FirstOrDefault(d => d.DocumentId == Guid.Parse(guid));
            return document;
        }

        public void DeleteFileAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Domain.Document>> DownloadAllFilesAsync(string folderId)
        {
            var documents = _context.Documents.Where(d => d.FolderId == folderId).ToList();
            return documents;
        }
    }
}
