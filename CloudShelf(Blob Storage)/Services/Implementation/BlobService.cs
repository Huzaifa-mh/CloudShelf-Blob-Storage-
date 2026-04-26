using Azure.Storage.Blobs;
using CloudShelf_Blob_Storage_.Services.Interface;

namespace CloudShelf_Blob_Storage_.Services.Implementation
{
    public class BlobService : IBlobService
    {
        private readonly string _connectionString;
        private readonly string _containerName;
        private readonly string _accountName;
        private readonly string _accountKey;

        public  BlobService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AzureStorage:ConnectionString");
            _containerName = configuration["AzureStorage:ContainerName"];
            _accountName = configuration["AzureStorage:AccountName"];
            _accountKey = configuration["AzureStorage:AccountKey"];
        }

        public async Task<string> UploadBlobAsync(Stream imageStream, string fileName, string contentType)
        {
            //connect to azure
            var blobServiceClient = new BlobServiceClient(_connectionString);

            //get your container
            var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

        }
    }
}
