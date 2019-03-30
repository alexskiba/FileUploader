using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FileUploader.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            using (var fileStream = new FileStream("bin/tmp.csv", FileMode.Create))
            {
                await Request.Body.CopyToAsync(fileStream);
                fileStream.Flush();
            }

            return Ok();
        }
    }
}