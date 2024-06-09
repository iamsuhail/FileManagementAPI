using FileManagementAPI.Database;
using FileManagementAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
namespace FileManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsUploadController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DocumentsUploadController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("UploadFile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadFile(IFormFile file, int folderId)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Invalid file.");
            }

            var folder = await _context.FolderData.FindAsync(folderId);
            if (folderId != 0 && folder == null)
            {
                return NotFound("Folder not found.");
            }

            var filename = file.FileName;
            var baseDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Documents");
            var filepath = baseDirectory;

            if (folderId != 0 && folder != null)
            {

                filepath = Path.Combine(baseDirectory, folder.Name);
            }

            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }

            //var extension = Path.GetExtension(filename);

            var completePath = Path.Combine(filepath, filename);
            try
            {
                using (var stream = new FileStream(completePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var fileMetadata = new FileMetadata
                {
                    FileName = filename,
                    FilePath = completePath,
                    FileSize = file.Length,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    FolderId = folderId
                };

                _context.FileMetadatas.Add(fileMetadata);
                await _context.SaveChangesAsync();

                return Ok(fileMetadata);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetAllFiles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllFiles()
        {
            try
            {
                var files = _context.FileMetadatas.ToList();
                return Ok(files);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("DownloadFile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DownloadFile(string filename)
        {
            var fileMetadata = _context.FileMetadatas.FirstOrDefault(f => f.FileName == filename);
            if (fileMetadata == null)
            {
                return NotFound("File not found.");
            }

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileMetadata.FilePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var bytes = await System.IO.File.ReadAllBytesAsync(fileMetadata.FilePath);
            return File(bytes, contentType, Path.GetFileName(fileMetadata.FilePath));
        }

        [HttpDelete]
        [Route("DeleteFile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteFile(string filename)
        {
            var fileMetadata = _context.FileMetadatas.FirstOrDefault(f => f.FileName == filename);
            if (fileMetadata == null)
            {
                return NotFound("File not found.");
            }

            try
            {
                if (System.IO.File.Exists(fileMetadata.FilePath))
                {
                    System.IO.File.Delete(fileMetadata.FilePath);
                }

                _context.FileMetadatas.Remove(fileMetadata);
                await _context.SaveChangesAsync();

                return Ok("File deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}