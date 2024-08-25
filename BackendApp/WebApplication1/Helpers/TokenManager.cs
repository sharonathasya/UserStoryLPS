
using SeewashAPICore.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using BackendApp.ViewModels;
using SeewashAPICore.Helpers;

namespace BackendApp.Helpers
{
    public interface ITokenManager
    {
        Principal_VM GetPrincipal();
    }

    public class TokenManager : ITokenManager
    {
        private IHttpContextAccessor _context;
        public TokenManager(IHttpContextAccessor httpContextAccessor)
        {
            _context = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor)); ;
        }
        public Principal_VM GetPrincipal()
        {
            string authorization = _context.HttpContext.Request.Headers["Authorization"];
            if (authorization != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = authorization.Split(" ")[1];
                var parsedToken = tokenHandler.ReadJwtToken(token);
                var keyHash = GetConfig.AppSetting["AppSettings:KeyHash"];

                var User_id = parsedToken.Claims.Where(c => c.Type == "USERID").FirstOrDefault().Value;
                var UserName = parsedToken.Claims.Where(c => c.Type == "USERNAME").FirstOrDefault().Value;
                var AccountId = parsedToken.Claims.Where(c => c.Type == "ACCOUNTID").FirstOrDefault().Value;
                var RoleId = parsedToken.Claims.Where(c => c.Type == "ROLEID").FirstOrDefault().Value;
                var RoleName = parsedToken.Claims.Where(c => c.Type == "ROLENAME").FirstOrDefault().Value;
                

                return new Principal_VM()
                {
                    User_id = User_id,
                    UserName = UserName,
                    AccountId = AccountId,
                    RoleId = RoleId,
                    RoleName = RoleName,
                    
                };
            }
            else
            {
                return null;
            }
        }
    }
}
