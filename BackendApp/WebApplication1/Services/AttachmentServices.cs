using BackendApp.Helpers;
using BackendApp.Interface;
using BackendApp.Models;
using BackendApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace BackendApp.Services
{
    public class AttachmentServices : IAttachmentServices
    {
        public readonly ITokenManager _tokenManger;
        public readonly dbContext _dbcontext;
        private IHostingEnvironment _hostingEnvironment;

        public AttachmentServices(ITokenManager tokenManager, dbContext dbcontext, IHostingEnvironment hostingEnvironment)
        {
            _tokenManger = tokenManager;
            _dbcontext = dbcontext;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<string> UploadFile(IFormFile _IFormFile)
        {
            string FileName = "";
            var currentUser = _tokenManger.GetPrincipal();
            try
            {
                
                TblSystemParameter AllowExtValue = _dbcontext.TblSystemParameter.SingleOrDefault((TblSystemParameter m) => m.Key == "AllowedFileUploadType");
                TblSystemParameter MaxFileValue = _dbcontext.TblSystemParameter.SingleOrDefault((TblSystemParameter m) => m.Key == "MaxFileUploadSize");
                string AllowedFileUploadType = AllowExtValue.Value;
                decimal MaxFileUploadSize = decimal.Parse(MaxFileValue.Value);
                decimal SizeFile = _IFormFile.Length / 1000000;
                string Ext = Path.GetExtension(_IFormFile.FileName);

                if (!AllowedFileUploadType.Contains(Ext))
                {
                    return "File type tidak sesuai";
                }
                if (SizeFile > MaxFileUploadSize)
                {
                    return "Ukuran file melebihi batas";
                }
                else
                {
                    FileInfo _FileInfo = new FileInfo(_IFormFile.FileName);
                    //FileName = Path.GetFileNameWithoutExtension(_IFormFile.FileName);
                    var _GetFilePath = Helpers.Common.GetFilePath(_IFormFile.FileName);
                    TblAttachment fileAttachment = new TblAttachment
                    {
                        FileName = _IFormFile.FileName,
                        Path = _GetFilePath,
                        Guid = _IFormFile.FileName + DateTime.Now.ToString(),
                        CreatedById = int.Parse(currentUser.User_id),
                        CreatedDateTime = DateTime.Now,
                        AccountId = int.Parse(currentUser.AccountId),
                    };
                    _dbcontext.TblAttachment.Add(fileAttachment);
                    _dbcontext.SaveChanges();
                    using (var _FileStream = new FileStream(_GetFilePath, FileMode.Create))
                    {
                        await _IFormFile.CopyToAsync(_FileStream);
                    }
                   

                }
                    return FileName;


                    
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<(byte[], string, string)> DownloadFile(int Id)
        {
            try
            {

                TblAttachment Attachment = _dbcontext.TblAttachment.Where((TblAttachment m) => m.Id == Id).FirstOrDefault();
                MemoryStream memory = new MemoryStream();
                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(Attachment.Path, out var _ContentType))
                {
                    _ContentType = "application/octet-stream";
                }
                memory.Position = 0L;
                var _ReadAllBytesAsync = await File.ReadAllBytesAsync(Attachment.Path);
                return (_ReadAllBytesAsync, _ContentType, Path.GetFileName(Attachment.FileName));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(bool status, string message, List<ResDataAttachment> data)> GetList()
        {
            List<ResDataAttachment> res = new List<ResDataAttachment>();
            try
            {
                var currentUser = _tokenManger.GetPrincipal();
                var getdata = _dbcontext.TblAttachment.ToList();
                
                if (getdata != null)
                {
                    foreach (var item in getdata)
                    {
                        var account = _dbcontext.Account.Where(x => x.Id == item.AccountId).FirstOrDefault();
                        ResDataAttachment dataitem = new ResDataAttachment
                        {
                            Id = item.Id,
                            FileName = item.FileName,
                            CreatedBy = account.Username,
                            CreatedDateTime = (item.CreatedDateTime.HasValue ? item.CreatedDateTime.Value.ToString("yyyy-MM-dd") : null),
                        };
                        res.Add(dataitem);

                    }

                    return (true, "List data", res);
                }
                else
                {
                    return (false, "Data tidak ditemukan", res);
                }
            }
            catch (Exception e)
            {
                return (false, "Terjadi Kesalahan", res);
            }
        }
    }
}
