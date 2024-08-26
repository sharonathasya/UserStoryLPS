using BackendApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace BackendApp.Interface
{
    public interface IAttachmentServices
    {
        Task<string> UploadFile(IFormFile _IFormFile);
        Task<(byte[], string, string)> DownloadFile(int Id);
        Task<(bool status, string message, List<ResDataAttachment> data)> GetList();

    }
}
