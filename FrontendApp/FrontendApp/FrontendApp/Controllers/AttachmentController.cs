using FrontendApp.Controllers;
using FrontendApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace FrontendApp.Controllers
{
    public class AttachmentController : BaseController
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:44392/api/Attachment/";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AttachmentController(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            var IsLoggin = IsUserLoggedIn();
            if (IsLoggin)
            {
                var dropdown = await FetchDropdown();
                TempData["DropdownItems"] = dropdown;


                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
            
        }
        public async Task<IActionResult> ListData()
        {
            var IsLoggin = IsUserLoggedIn();
            if (IsLoggin)
            {
                if (ModelState.IsValid)
                {

                    var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
                    if (token != null)
                    {
                        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
                        var response = await _httpClient.PostAsync(_apiUrl + "GetList", null);
                        var ss = response;
                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            var data = JsonConvert.DeserializeObject<ServiceResponseSingle<List<ResDataAttachment>>>(result);
                            if (data.CODE == 1)
                            {

                                return View("/Views/Attachment/ListAttachment.cshtml", data.DATA);
                            }
                            else
                            {
                                TempData["ErrorMessage"] = data.MESSAGE;
                                return RedirectToAction("ListData", "Attachment");
                            }
                        }
                        else
                        {
                            TempData["ErrorMessage"] = " data.MESSAGE";
                            return RedirectToAction("ListData", "Attachment");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Login", "Login");
                    }


                }
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

           
            return View("/Views/Attachment/ListAttachment.cshtml");
        }

        public async Task<IActionResult> Edit(string? id)
        {

            if (id == null)
            {
                return View("/Views/Attachment/ListAttachment.cshtml");
            }
            else
            {
                var IsLoggin = IsUserLoggedIn();
                if (IsLoggin)
                {
                    if (ModelState.IsValid)
                    {
                        var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
                        if (token != null)
                        {
                            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            var payload = new { id = id };
                            var jsonContent = JsonConvert.SerializeObject(payload);
                            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                            var response = await _httpClient.PostAsync(_apiUrl + "GetDataById", content);
                            var ss = response;
                            if (response.IsSuccessStatusCode)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                var data = JsonConvert.DeserializeObject<ServiceResponseSingle<ResDataAttachment>>(result);
                                if (data.CODE == 1)
                                {
                                    var dropdown = await FetchDropdown();
                                    TempData["DropdownItems"] = dropdown;
                                    return View("/Views/Attachment/EditAttachment.cshtml", data.DATA);
                                }
                                else
                                {
                                    TempData["ErrorMessage"] = data.MESSAGE;
                                    return RedirectToAction("Edit", "Attachment");
                                }
                            }
                            else
                            {
                                TempData["ErrorMessage"] = " data.MESSAGE";
                                return RedirectToAction("Edit", "Attachment");
                            }
                        }
                        else
                        {
                            return RedirectToAction("Login", "Login");
                        }


                    }
                }
                else
                {
                    return RedirectToAction("Login", "Login");
                }

              
                return View("/Views/Attachment/EditAttachment.cshtml");
            }

        }
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "id Tidak ditemukan";
            }
            else
            {
                var IsLoggin = IsUserLoggedIn();
                if (IsLoggin)
                {
                    if (ModelState.IsValid)
                    {
                        var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
                        if (token != null)
                        {
                            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            var payload = new { id = id };
                            var jsonContent = JsonConvert.SerializeObject(payload);
                            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                            var response = await _httpClient.PostAsync(_apiUrl + "DeleteData", content);
                            var ss = response;
                            if (response.IsSuccessStatusCode)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                var data = JsonConvert.DeserializeObject<ServiceResponseSingle<ResDataAttachment>>(result);
                                if (data.CODE == 1)
                                {

                                    TempData["SuccessMessage"] = data.MESSAGE;
                                    return RedirectToAction("ListData", "Attachment");
                                }
                                else
                                {
                                    TempData["ErrorMessage"] = data.MESSAGE;
                                    return RedirectToAction("ListData", "Attachment");
                                }
                            }
                            else
                            {
                                TempData["ErrorMessage"] = "Terjadi kesalahan";
                                return RedirectToAction("ListData", "Attachment");
                            }
                        }
                        else
                        {
                            return RedirectToAction("Login", "Login");
                        }


                    }
                }
                else
                {
                    return RedirectToAction("Login", "Login");
                }

              
            }
            return RedirectToAction("ListData", "Attachment");
        }



        public async Task<List<ResDropdown>> FetchDropdown()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_apiUrl + "DropdownStorage", null);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<ServiceResponseSingle<List<ResDropdown>>>(result);
                    if (data.CODE == 1)
                    {
                        return data.DATA;
                    }
                    else
                    {
                      
                        throw new Exception(data.MESSAGE);
                    }
                }
                else
                {
                   
                    throw new Exception("Failed to fetch data from the API.");
                }
            }
            else
            {
         
                throw new UnauthorizedAccessException("Token is missing. Please log in.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SubmitData(Attachment model)
        {

            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            if (token != null)
            {
                var OO = JsonConvert.SerializeObject(model);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_apiUrl + "AddDataAttachment", content);
                var ss = response;
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<ServiceResponseSingle<string>>(result);
                    if (data.CODE == 1)
                    {

                        TempData["SuccessMessage"] = data.MESSAGE;
                        return RedirectToAction("ListData", "Attachment");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = data.MESSAGE;
                        return RedirectToAction("ListData", "Attachment");
                    }

                }
                else
                {
                    TempData["ErrorMessage"] = " data.MESSAGE";
                    return RedirectToAction("ListData", "Attachment");
                }
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }



            return View("/Views/Login/index.cshtml");
        }
    }
}
