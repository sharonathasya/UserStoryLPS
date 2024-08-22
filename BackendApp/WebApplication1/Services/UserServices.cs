using BackendApp.Helpers;
using BackendApp.Interface;
using BackendApp.Models;
using BackendApp.ViewModels;
using System.Net;
using System.Numerics;
using System.Reflection;

namespace BackendApp.Services
{
    public class UserServices : IUserServices
    {
        public readonly ITokenManager _tokenManger;
        public readonly dbContext _dbcontext;

        public UserServices(ITokenManager tokenManager, dbContext dbcontext)
        {
            _tokenManger = tokenManager;
            _dbcontext = dbcontext;
        }

        public async Task<(bool status, string message)> AddDataUser(ReqAddUser req)
        {
            DateTime aDate = DateTime.Now;

            try
            {
                var currentUser = _tokenManger.GetPrincipal();
                var checkdata = _dbcontext.Account.Where(x => x.Username == req.Username).FirstOrDefault();
                if (checkdata == null)
                {
                    Account insertAcc = new Account
                    {
                        Email = req.Email,
                        Username = req.Username,
                        Password = req.Password,
                        PhoneNumber = req.PhoneNumber,
                        CreatedTime = aDate,
                        IsDeleted = false,
                        IsActive = true
                    };
                    _dbcontext.Account.Add(insertAcc);
                    _dbcontext.SaveChanges();

                    var getAccId = _dbcontext.Account.Where(x => x.Username == req.Username).FirstOrDefault();
                    User insertUser = new User
                    {
                        Email = req.Email,
                        Phone = req.Phone,
                        Address = req.Address,
                        FirstName = req.FirstName,
                        LastName = req.LastName,
                        Gender = req.Gender,
                        BirthDate = req.BirthDate,
                        RegistDate = aDate,
                        AccountId = getAccId.Id
                    };
                    _dbcontext.User.Add(insertUser);
                    _dbcontext.SaveChanges();

                    AccountRole accRole = new AccountRole
                    {
                        AccountId = (int)getAccId.Id,
                        RoleId = 7
                    };
                    _dbcontext.AccountRole.Add(accRole);
                    _dbcontext.SaveChanges();
                    return (true, "Data Berhasil Ditambahkan");
                } else
                {
                    return (false, "Username sudah terdaftar");
                }
            
             }catch(Exception e)
            {
                return (false, "Terjadi Kesalahan");
            }
        }
        
        public async Task<(bool status, string message)> DeleteData(ReqIdUser req)
        {
            try
            {
                var getdata = _dbcontext.Account.Where(e => e.Username == req.Username).FirstOrDefault();
                if (getdata != null)
                {
                    _dbcontext.Remove(getdata);
                    _dbcontext.SaveChanges();
                    return (true, "Data berhasil dihapus");
                }
                else
                {
                    return (false, "Data tidak ditemukan");
                }
            }catch(Exception e)
            {
                return (false, "Terjadi Kesalahan");
            }
        }

        public async Task<(bool status, string message)> GetUserByEmail(ReqIdUser req)
        {
            try
            {
                var getdata = _dbcontext.Account.Where(e => e.Email == req.Email).FirstOrDefault();
                if (getdata == null)
                {
                    return (true, "Email User sudah terdaftar");
                }
                else
                {
                    return (false, "Email User belum terdaftar, silahkan daftar terlebih dahulu");
                }
            }
            catch (Exception e)
            {
                return (false, "Terjadi Kesalahan");
            }
        }



    }
}
