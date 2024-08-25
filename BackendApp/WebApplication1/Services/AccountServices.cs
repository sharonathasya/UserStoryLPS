using BackendApp.Helpers;
using BackendApp.Interface;
using BackendApp.Models;
using BackendApp.ViewModels;
using Microsoft.IdentityModel.Tokens;
using SeewashAPICore.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BackendApp.Services
{
    public class AccountServices : IAccountServices
    {
        public readonly ITokenManager _tokenManger;
        public readonly dbContext _dbcontext;

        public AccountServices(ITokenManager tokenManager, dbContext dbcontext)
        {
            _tokenManger = tokenManager;
            _dbcontext = dbcontext;
        }

        public async Task<(bool status, string message, ResLogin data)> LoginAccount(ReqLogin req)
        {
            DateTime aDate = DateTime.Now;
            var keyHash = GetConfig.AppSetting["AppSettings:KeyHash"];
            var tokenlifetime = GetConfig.AppSetting["AppSettings:TokenLifetime"];
            var issuer = GetConfig.AppSetting["AppSettings:Issuer"];
            ResLogin resLogin = new ResLogin();
            try
            {                
                var getUser = _dbcontext.Account.Where(x => x.Username == req.USERNAME && x.Password == req.PASSWORD).FirstOrDefault();
                var getUserRole = _dbcontext.AccountRole.Where(x => x.AccountId == getUser.Id).FirstOrDefault();
                var getUserRoleName = _dbcontext.TblMasterRole.Where(x => x.Id == getUserRole.RoleId).FirstOrDefault();
                if (getUser != null) {
                    if (getUser.IsActive == true)
                    {
                        var KeyJWT = Encoding.ASCII.GetBytes(GetConfig.AppSetting["AppSettings:Secret"]);
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new Claim[]
                              {
                            new Claim("USERID", getUser.Userid.ToString() ),
                            new Claim("USERNAME",getUser.Username),
                            new Claim("ACCOUNTID", getUser.Id.ToString() ),
                            new Claim("ROLEID", getUserRole.RoleId.ToString() ),
                            new Claim("ROLENAME", getUserRoleName.RoleName.ToString() ),
                              }),
                            Expires = DateTime.UtcNow.AddMinutes(double.Parse(GetConfig.AppSetting["AppSettings:TokenLifetime"])),
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(KeyJWT), SecurityAlgorithms.HmacSha256Signature),
                            Issuer = GetConfig.AppSetting["AppSettings:Issuer"]

                        };
                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        resLogin.TOKEN = tokenHandler.WriteToken(token);
                        return (true, "Login Berhasil", resLogin);
                    }
                    else
                    {
                        return (false, "UserId sudah tidak aktif", resLogin);
                    }
                    
                }
                else
                {
                    return (false, "UserId atau Password Salah", resLogin);
                }
            }catch(Exception e)
            {
                return (false, e.Message, resLogin);
            }
        }
    }
}
