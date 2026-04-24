namespace CloudShelf_Blob_Storage_.Models
{
    public class image
    {
        public int Id { get; set; }
        public string FileName { get; set; }

        public string OriginalUrl { get; set; }
        public string? ContentType {get; set;}
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
