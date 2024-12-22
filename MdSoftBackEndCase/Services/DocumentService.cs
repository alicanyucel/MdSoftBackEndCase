using DocumentManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using MdSoftBackEndCase.Models;

namespace MdSoftBackEndCase.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<DocumentService> _logger;

        public DocumentService(AppDbContext context, IWebHostEnvironment env, ILogger<DocumentService> logger)
        {
            _context = context;
            _env = env;
            _logger = logger;
        }

        // Belgeleri tarihe göre sıralayarak al
        public async Task<IEnumerable<Document>> GetDocumentsAsync()
        {
            return await _context.Documents.OrderByDescending(d => d.UploadedAt).ToListAsync();
        }

        // Belgeyi ID'ye göre al
        public async Task<Document> GetDocumentByIdAsync(int id)
        {
            return await _context.Documents.FindAsync(id);
        }

        // Belge yükle
        public async Task<string> UploadDocumentAsync(IFormFile file, string uploadedBy)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file uploaded.");

            var filePath = Path.Combine(_env.ContentRootPath, "uploads", file.FileName);

            // Dosyayı kaydet
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Yeni belge kaydını oluştur
            var document = new Document
            {
                FileName = file.FileName,
                FileType = file.ContentType,
                FileSize = file.Length,
                UploadedBy = uploadedBy,
                UploadedAt = DateTime.Now,
                FilePath = filePath
            };

            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Document uploaded: {file.FileName} by {uploadedBy}");
            return file.FileName;
        }

        // Belgeyi indir
        public async Task<byte[]> DownloadDocumentAsync(int id)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document == null)
                throw new FileNotFoundException("Document not found.");

            return await File.ReadAllBytesAsync(document.FilePath);
        }
    }
}