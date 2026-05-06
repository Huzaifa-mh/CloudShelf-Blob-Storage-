using CloudShelf_Blob_Storage_.Data;
using CloudShelf_Blob_Storage_.DTO;
using CloudShelf_Blob_Storage_.Services.Interface;
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

            var extension  = path


        }
    }
}
