namespace FileManagementAPI.Models
{
    public class Folder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        // public ICollection<FileMetadata> Files { get; set; }
        public int ParentFolderId { get; set; }

    }
}
