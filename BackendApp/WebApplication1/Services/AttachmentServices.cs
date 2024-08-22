using BackendApp.Helpers;
using BackendApp.Interface;
using BackendApp.Models;
using BackendApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace BackendApp.Services
{
    public class AttachmentServices : IAttachmentServices
    {
        public readonly ITokenManager _tokenManger;
        public readonly dbContext _dbcontext;

        public AttachmentServices(ITokenManager tokenManager, dbContext dbcontext)
        {
            _tokenManger = tokenManager;
            _dbcontext = dbcontext;
        }

        public async Task<string> UploadFile(IFormFile _IFormFile)
        {
            string FileName = "";
            var currentUser = _tokenManger.GetPrincipal();
            try
            {
                FileInfo _FileInfo = new FileInfo(_IFormFile.FileName);
                FileName = _IFormFile.FileName + "_" + DateTime.Now.Ticks.ToString() + _FileInfo.Extension;
                var _GetFilePath = Helpers.Common.GetFilePath(FileName);
                using (var _FileStream = new FileStream(_GetFilePath, FileMode.Create))
                {
                    await _IFormFile.CopyToAsync(_FileStream);
                }
                return FileName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<(byte[], string, string)> DownloadFile(string FileName)
        {
            try
            {
                var _GetFilePath = Helpers.Common.GetFilePath(FileName);
                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(_GetFilePath, out var _ContentType))
                {
                    _ContentType = "application/octet-stream";
                }
                var _ReadAllBytesAsync = await File.ReadAllBytesAsync(_GetFilePath);
                return (_ReadAllBytesAsync, _ContentType, Path.GetFileName(_GetFilePath));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}
