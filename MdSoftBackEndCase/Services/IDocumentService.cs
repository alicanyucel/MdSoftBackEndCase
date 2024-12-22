

using MdSoftBackEndCase.Models;

namespace MdSoftBackEndCase.Services;

public interface IDocumentService
{
    Task<IEnumerable<Document>> GetDocumentsAsync();
    Task<Document> GetDocumentByIdAsync(int id);
    Task<string> UploadDocumentAsync(IFormFile file, string uploadedBy);
    Task<byte[]> DownloadDocumentAsync(int id);
}
