namespace CloudShelf_Blob_Storage_.DTO
{
    public class ImageResponse
    {
        public int Id { get; set; }
        public string SasUrl { get; set; }
        public string OriginalUrl { get; set; }
        public DateTime UploadedAt { get; set; }
}
}
