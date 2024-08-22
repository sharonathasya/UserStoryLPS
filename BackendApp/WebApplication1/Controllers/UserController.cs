using BackendApp.Interface;
using BackendApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeewashAPICore.ViewModels;
using System.Security.Principal;

namespace BackendApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _user;
        public UserController(IUserServices user)
        {
            _user = user;
        }

        [Authorize]
        [HttpPost("AddDataUser")]
        public async Task<IActionResult> AddDataUser([FromBody] ReqAddUser req)
        {
            var res = new ServiceResponseSingle<string>();
            try
            {
                var _ = await _user.AddDataUser(req);

                if (!_.status)
                {
                    res.CODE = 0;
                    res.MESSAGE = _.message;
                }
                else
                {
                    res.CODE = 1;
                    res.MESSAGE = _.message;
                    //res.DATA = _.data;
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

        

        [Authorize]
        [HttpPost("DeleteData")]
        public async Task<IActionResult> DeleteData([FromBody] ReqIdUser req)
        {
            var res = new ServiceResponseSingle<string>();
            try
            {
                var _ = await _user.DeleteData(req);

                if (!_.status)
                {
                    res.CODE = 0;
                    res.MESSAGE = _.message;
                }
                else
                {
                    res.CODE = 1;
                    res.MESSAGE = _.message;
                    //res.DATA = _.data;
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

        [Authorize]
        [HttpPost("GetUser")]
        public async Task<IActionResult> GetUserByEmail([FromBody] ReqIdUser req)
        {
            var res = new ServiceResponseSingle<string>();
            try
            {
                var _ = await _user.GetUserByEmail(req);

                if (!_.status)
                {
                    res.CODE = 0;
                    res.MESSAGE = _.message;
                }
                else
                {
                    res.CODE = 1;
                    res.MESSAGE = _.message;
                    //res.DATA = _.data;
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
