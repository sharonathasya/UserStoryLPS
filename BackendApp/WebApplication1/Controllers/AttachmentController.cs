using BackendApp.Interface;
using BackendApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SeewashAPICore.ViewModels;
using System.Net.Mail;

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
        [HttpPost]
        [Route("downloadfile")]
        public async Task<IActionResult> DownloadFile(int Id)
        {
            var result = await _attachment.DownloadFile(Id);
            return File(result.Item1, result.Item2, result.Item3);
        }

        [Authorize]
        [HttpPost("GetList")]
        public async Task<IActionResult> GetList()
        {
            var res = new ServiceResponse<ResDataAttachment>();
            try
            {
                var _ = await _attachment.GetList();

                if (!_.status)
                {
                    res.CODE = 0;
                    res.MESSAGE = _.message;
                }
                else
                {
                    res.CODE = 1;
                    res.MESSAGE = _.message;
                    res.DATA = _.data;
                }
            }
            catch (Exception ex)
            {
                res.CODE = 0;
                res.MESSAGE = ex.Message == null ? ex.InnerException.ToString() : ex.Message.ToString();
                return new BadRequestObjectResult(res);
            }

            return new OkObjectResult(res);
        }
    }
}
