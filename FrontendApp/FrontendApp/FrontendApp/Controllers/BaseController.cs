using Microsoft.AspNetCore.Mvc;

namespace FrontendApp.Controllers
{
    public class BaseController : Controller
    {
        protected bool IsUserLoggedIn()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("JWToken"));
        }
    }
}
