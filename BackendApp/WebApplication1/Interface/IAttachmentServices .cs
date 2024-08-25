using BackendApp.ViewModels;
using System.Net.Mail;

namespace BackendApp.Interface
{
    public interface IAttachmentServices
    {
        Task<string> UploadFile(IFormFile _IFormFile);
        Task<(byte[], string, string)> DownloadFile(string FileName, int Id);
        Task<(bool status, string message, List<ResDataAttachment> data)> GetList();

    }
}
