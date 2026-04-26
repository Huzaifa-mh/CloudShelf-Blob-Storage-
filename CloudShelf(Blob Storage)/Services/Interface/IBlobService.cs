namespace CloudShelf_Blob_Storage_.Services.Interface
{
    public interface IBlobService
    {
        Task<string> UploadBlobAsync(Stream imageStream, string fileName, string contentType);
        string GenerateSasUrl(string fileName);
    }
}
