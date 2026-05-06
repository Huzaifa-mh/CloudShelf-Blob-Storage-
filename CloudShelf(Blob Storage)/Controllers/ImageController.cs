using CloudShelf_Blob_Storage_.Data;
using CloudShelf_Blob_Storage_.DTO;
using CloudShelf_Blob_Storage_.Services.Interface;
using CloudShelf_Blob_Storage_.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CloudShelf_Blob_Storage_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController(IBlobService service, AppDBContext context, IHttpClientFactory httpClientFactory) : ControllerBase
    {
        private readonly IBlobService _blobService = service;
        private readonly AppDBContext _context = context;
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromBody] UploadImageRequest request)
        {
            if (string.IsNullOrEmpty(request.ImageUrl))
            {
                return BadRequest("ImageUrl is required!");
            }

            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(request.ImageUrl);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Could not dowload the image from the url.");
            }

            var imageStream = await response.Content.ReadAsStreamAsync();

            var contentType = response.Content.Headers.ContentType?.MediaType ?? "image/jpeg";

            var extension = Path.GetExtension(new Uri(request.ImageUrl).AbsolutePath);

            if (string.IsNullOrEmpty(extension))
            {
                extension = ".jpg";
            }

            var fileName = $"{Guid.NewGuid()}{extension}";

            await _blobService.UploadBlobAsync(imageStream, fileName, contentType);

            var image = new Image
            {
                FileName = fileName,
                OriginalUrl = request.ImageUrl,
                ContentType = contentType,
                UploadedAt = DateTime.UtcNow
            };

            _context.Images.Add(image);
            await _context.SaveChangesAsync();

            var sasUrl = _blobService.GenerateSasUrl(image.FileName);

            return Ok(new ImageResponse
            {
                Id = image.Id,
                SasUrl = sasUrl,
                OriginalUrl = image.OriginalUrl,
                UploadedAt = image.UploadedAt
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImage(int id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }
            
            var sasUrl = _blobService.GenerateSasUrl(image.FileName);

            return Ok(new ImageResponse
            {
                Id = image.Id,
                SasUrl = sasUrl,
                OriginalUrl = image.OriginalUrl,
                UploadedAt = image.UploadedAt
            });
        }
    }
}
