using BackendApp.Interface;
using BackendApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SeewashAPICore.ViewModels;

namespace BackendApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IAttachmentServices _attachment;

        public UploadController(IAttachmentServices attachment)
        {
            _attachment = attachment;
        }

        [Authorize]
        [HttpPost]
        [Route("uploadfile")]
        public async Task<IActionResult> UploadFile(IFormFile _IFormFile)
        {
            var result = await _attachment.UploadFile(_IFormFile);
            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        [Route("downloadfile")]
        public async Task<IActionResult> DownloadFile(string FileName)
        {
            var result = await _attachment.DownloadFile(FileName);
            return File(result.Item1, result.Item2, result.Item2);
        }
    }
}
