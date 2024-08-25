using FrontendApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;

namespace FrontendApp.Controllers
{
    public class LoginController : BaseController
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:44392/api/Account/LoginAccount";
        public LoginController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public IActionResult Logout()
        {
          
            HttpContext.Session.Remove("JWToken");
            return RedirectToAction("Login", "Login");
        }
        public IActionResult Login()
        {
            var IsLoggin = IsUserLoggedIn();
            if (IsLoggin)
            {
                return View("/views/Attachment/ListAttachment.cshtml");
            }
            else
            {
                return View("/views/login/index.cshtml");
            }
           
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                var loginModel = new
                {
                    Username = model.USERNAME,
                    Password = model.PASSWORD
                };

                var response = await _httpClient.PostAsync(_apiUrl, new StringContent(JsonConvert.SerializeObject(loginModel), Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<ServiceResponseSingle<ResLogin>>(result);
                    if (data.CODE == 1)
                    {
                        var token = data.DATA.TOKEN;
                        ViewBag.Token = token;
                        TempData["SuccessMessage"] = data.MESSAGE;
                        HttpContext.Session.SetString("JWToken", token.ToString());
                        return RedirectToAction("ListData", "Attachment");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = data.MESSAGE;
                        return RedirectToAction("Login", "Login");
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View("/Views/Login/index.cshtml");
        }
    }
}
