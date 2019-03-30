using System.Threading.Tasks;
using FileUploader.Domain;
using Microsoft.AspNetCore.Mvc;

namespace FileUploader.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly StorageService _storageService;

        public UploadController(StorageService storageService)
        {
            _storageService = storageService;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            await _storageService.Save(Request.Body);

            return Ok();
        }
    }
}