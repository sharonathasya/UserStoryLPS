using BackendApp.Interface;
using BackendApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeewashAPICore.ViewModels;

namespace BackendApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountServices _account;
        public AccountController(IAccountServices account)
        {
            _account = account;
        }

        [HttpPost("LoginAccount")]
        public async Task<IActionResult> LoginAccount([FromBody] ReqLogin req)
        {
            var res = new ServiceResponseSingle<ResLogin>();
            try
            {
                var _ = await _account.LoginAccount(req);

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
