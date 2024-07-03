using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using VideoTime.Services.Blobs;

namespace VideoTime.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class VideoMetadataController:RESTFulController
    {
        private readonly IBlobService blobService;

        public VideoMetadataController(IBlobService blobService)
        {
            this.blobService = blobService;
        }

        [HttpGet("{containerName}/{blobName}")]
        public async Task<IActionResult> GetVideo(string containerName, string blobName)
        {
            Stream videoStream = await blobService.GetBlobStreamAsync(blobName, containerName);

            if (videoStream == null)
            {
                return NotFound();
            }

            return File(videoStream, "video/mp4");
        }
    }
}
