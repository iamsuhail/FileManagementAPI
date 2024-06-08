using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FileManagementAPI.Database;
using FileManagementAPI.Models;

namespace FileManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FolderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FolderController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("CreateFolder")]
        public async Task<IActionResult> CreateFolder(string folderName)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", folderName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                return Ok($"Folder '{folderName}' created successfully.");
            }
            else
            {
                return BadRequest($"Folder '{folderName}' already exists.");
            }
        }


        /*[HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllFolders()
        {
            var folders = await _context.Folders.Include(f => f.Files).ToListAsync();
            return Ok(folders);
        }*/

        /*[HttpPut]
        [Route("Update")]
        public async Task<IActionResult> UpdateFolder(int id, [FromBody] Folder folder)
        {
            var existingFolder = await _context.Folders.FindAsync(id);
            if (existingFolder == null)
            {
                return NotFound("Folder not found.");
            }

            existingFolder.Name = folder.Name;
            existingFolder.Path = folder.Path;
            existingFolder.UpdatedAt = DateTime.UtcNow;

            _context.Folders.Update(existingFolder);
            await _context.SaveChangesAsync();

            return Ok(existingFolder);
        }*/

        /*[HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteFolder(int id)
        {
            var folder = await _context.Folders.Include(f => f.Files).FirstOrDefaultAsync(f => f.Id == id);
            if (folder == null)
            {
                return NotFound("Folder not found.");
            }

            try
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Documents", folder.Path);
                if (Directory.Exists(folderPath))
                {
                    Directory.Delete(folderPath, true);
                }

                _context.Folders.Remove(folder);
                await _context.SaveChangesAsync();

                return Ok("Folder deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }*/
        [HttpPost]
        [Route("UploadFileToFolder")]
        public async Task<IActionResult> UploadFileToFolder(string folderName, IFormFile file)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", folderName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            if (file != null && file.Length > 0)
            {
                var filePath = Path.Combine(folderPath, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Ok($"File '{file.FileName}' uploaded to folder '{folderName}' successfully.");
            }
            else
            {
                return BadRequest("File is empty or not provided.");
            }
        }

    }
}
