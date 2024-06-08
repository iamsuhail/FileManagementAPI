using System.ComponentModel.DataAnnotations;

namespace FileManagementAPI
{
    public class DocumentAttachments
    {
        public class FileUploadViewModel
        {
            public IFormFile File { get; set; }
            public string Uid { get; set; }
            public string Parent { get; set; }
            public string Data { get; set; }
            public string Name { get; set; }
            public string Url { get; set; }
            public string Path { get; set; }
        }


    }
}
