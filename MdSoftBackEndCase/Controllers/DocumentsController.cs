using Azure.Core;
using MdSoftBackEndCase.Models;
using MdSoftBackEndCase.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class DocumentsController : ControllerBase
{
    private readonly IDocumentService _documentService;

    public DocumentsController(IDocumentService documentService)
    {
        _documentService = documentService;
    }

    // GET: api/Documents
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Document>>> GetDocuments()
    {
        try
        {
            var documents = await _documentService.GetDocumentsAsync();
            return Ok(documents);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // POST: api/Documents/upload
    [HttpPost("upload")]
    public async Task<IActionResult> UploadDocument(IFormFile file)
    {
        try
        {
            var username = Request.Headers["Username"].ToString();
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("Username is required.");
            }

            var fileName = await _documentService.UploadDocumentAsync(file, username);
            return Ok(new { FileName = fileName });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // GET: api/Documents/download/1
    [HttpGet("download/{id}")]
    public async Task<IActionResult> DownloadDocument(int id)
    {
        try
        {
            var fileBytes = await _documentService.DownloadDocumentAsync(id);
            var document = await _documentService.GetDocumentByIdAsync(id);
            return File(fileBytes, document.FileType, document.FileName);
        }
        catch (FileNotFoundException)
        {
            return NotFound("Document not found.");
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
