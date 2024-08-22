using BackendApp.ViewModels;

namespace BackendApp.Interface
{
    public interface IAttachmentServices
    {
        Task<string> UploadFile(IFormFile _IFormFile);
        Task<(byte[], string, string)> DownloadFile(string FileName);

    }
}
