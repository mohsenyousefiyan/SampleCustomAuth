using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Filters;
using SampleAuthentication.DAL;
using SampleAuthentication.Entites;
using SampleAuthentication.InfraStructures.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SampleAuthentication.InfraStructures.ActionFilters
{
    public class CustomAuthorizeFilter : Attribute, IAuthorizationFilter
    {
        private readonly IUserRepository userRepository;

        public CustomAuthorizeFilter(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var usertoken = context.HttpContext.ReadRequestCookie<User>("UserToken");
            if (usertoken != null)
            {
                var claims = new List<Claim>()
                 {
                    new Claim(type:ClaimTypes.Name,value:usertoken.UserName,issuer:"FromCookie",valueType:ClaimValueTypes.String),
                    new Claim(type:ClaimTypes.NameIdentifier,value:usertoken.Id.ToString(),issuer:"FromCookie",valueType:ClaimValueTypes.Integer32),
                 };

                var userclaims = userRepository.GetUserClaims(usertoken.Id);
                foreach (var item in userclaims)
                    claims.Add(new Claim(type: item.Key, value: item.Value, valueType: ClaimValueTypes.String, issuer: "FromDb"));

                context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));             
            }
            else
                context.HttpContext.Response.Redirect("/Account/Login");
        }
    }
}
