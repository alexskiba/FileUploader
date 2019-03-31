using System;
using System.Net;
using System.Threading.Tasks;
using FileUploader.Domain;
using Microsoft.AspNetCore.Mvc;
using Serilog;

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
            try
            {
                // todo: implement sessionId
                await _storageService.Save(Request.Body, Guid.NewGuid().ToString("N"));

                return Ok();
            }
            catch (ValidationException e)
            {
                Log.Warning(e, "Request validation failed");
                return BadRequest(e.Message);
            }
            catch (ApplicationException e)
            {
                Log.Error(e, "Request execution failed");
                return base.StatusCode((int) HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}