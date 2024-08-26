using FrontendApp.Controllers;
using FrontendApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http.Headers;
using System.Text;

namespace FrontendApp.Controllers
{
    public class AttachmentController : BaseController
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:44392/api/Upload/";
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
                


                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
            
        }

        public async Task<IActionResult> CreateAttachment()
        {
            var IsLoggin = IsUserLoggedIn();
            if (IsLoggin)
            {


                return View("/Views/Attachment/UploadAttachment.cshtml");
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

        public async Task<IActionResult> Download(int Id)
        {

            if (Id == 0)
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
                            var payload = new { Id = Id };
                            var jsonContent = JsonConvert.SerializeObject(payload);
                            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                            var response = await _httpClient.PostAsync(_apiUrl + "downloadfile?Id=" + Id, null);
                            var ss = response;

                            var filePath = response.Headers;
                            var result2 = await response.Content.ReadAsStringAsync();

                            if (response.IsSuccessStatusCode)
                            {
                                var result = await response.Content.ReadAsByteArrayAsync();

                                var contentDisposition = response.Content.Headers.ContentDisposition;
                                var fileType = response.Content.Headers.ContentType;
                                string filename = "default_filename.pdf"; // Default filename if not provided
                                string contentType = "application/pdf";

                                if (contentDisposition != null && !string.IsNullOrEmpty(contentDisposition.FileName) && fileType != null)
                                {
                                    filename = contentDisposition.FileName.Trim('"'); // Remove any surrounding quotes
                                    contentType = fileType.ToString();

                                }

                                return File(result, contentType, filename);
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

        }
        

        [HttpPost]
        public async Task<IActionResult> SubmitData(Attachment model)
        {

            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            if (token != null)
            {
                var OO = JsonConvert.SerializeObject(model);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var content = new StringContent(JsonConvert.SerializeObject(model.UploadFile), Encoding.UTF8, "application/json");
                var form = new MultipartFormDataContent();

                // Add the file
                if (model.UploadFile != null && model.UploadFile.Length > 0)
                {
                    var fileContent = new StreamContent(model.UploadFile.OpenReadStream());
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(model.UploadFile.ContentType);
                    form.Add(fileContent, "UploadFile", model.UploadFile.FileName);
                }

                var response = await _httpClient.PostAsync(_apiUrl + "uploadfile", form);
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
