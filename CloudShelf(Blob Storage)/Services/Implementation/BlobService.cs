using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
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
            _connectionString = configuration["AzureStorage:ConnectionString"];
            _containerName = configuration["AzureStorage:ContainerName"];
            _accountName = configuration["AzureStorage:AccountName"];
            _accountKey = configuration["AzureStorage:AccountKey"];
        }

        public async Task<string> UploadBlobAsync(Stream imageStream, string fileName, string contentType)
        {
            
        }

        public string GenerateSasUrl(string filename)
        {

            //create credentials using account name and key
            var credential = new StorageSharedKeyCredential(_accountName, _accountKey);

            //build the sas token
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = _containerName,
                BlobName = filename,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
            };


            //set permissions to read only
            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            //build the full sas url
            var sasToken = sasBuilder.ToSasQueryParameters(credential).ToString();
            var blobUrl = $"https://{_accountName}.blob.core.windows.net/{_containerName}/{filename}?{sasToken}";

            return blobUrl;
        }
    }
}
