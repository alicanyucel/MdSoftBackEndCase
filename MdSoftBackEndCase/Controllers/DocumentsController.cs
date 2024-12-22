using MdSoftBackEndCase.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MdSoftBackEndCase.Controllers;

        [Route("api/[controller]/[action]")]
        [ApiController]
        public class DocumentController : ControllerBase
        {
            private readonly DocumentService _documentService;
            private readonly string _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

            public DocumentController(DocumentService documentService)
            {
                _documentService = documentService;
            }

            // Belge Yükleme
            [HttpPost("upload")]
            public async Task<IActionResult> UploadDocument([FromHeader] string user, [FromForm] IFormFile file)
            {
                if (user != "admin")
                    return Unauthorized("Only admins can upload documents.");

                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded.");

                var filePath = Path.Combine(_storagePath, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var document = await _documentService.UploadDocumentAsync(file.FileName, (int)file.Length);

                return Ok(new { document.Id, document.FileName });
            }

            // Belge İndirme
            [HttpGet("download/{fileName}")]
            public IActionResult DownloadDocument(string fileName)
            {
                var filePath = Path.Combine(_storagePath, fileName);

                if (!System.IO.File.Exists(filePath))
                    return NotFound("File not found.");

                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, "application/octet-stream", fileName);
            }

            // Belgeleri Listeleme
            [HttpGet("list")]
            public async Task<IActionResult> ListDocuments()
            {
                var documents = await _documentService.GetDocumentsAsync();
                return Ok(documents);
            }
        }

    }
}