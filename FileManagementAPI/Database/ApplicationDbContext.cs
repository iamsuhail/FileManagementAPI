using Microsoft.EntityFrameworkCore;
using FileManagementAPI.Models;

namespace FileManagementAPI.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<FileMetadata> FileMetadatas { get; set; }
        public DbSet<Folder> FolderData { get; set; }
    }
}
