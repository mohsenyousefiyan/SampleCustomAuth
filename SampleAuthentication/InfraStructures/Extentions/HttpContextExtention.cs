using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SampleAuthentication.Entites.Enums;
using SampleAuthentication.InfraStructures.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleAuthentication.InfraStructures.Extentions
{
    public static class HttpContextExtention
    {
        static AESCryptography aESCryptography = new AESCryptography(key: "@$SmsManagment_Panel_1398_07_1@$", keysize: CryptographyEnums.AESKeySize.Key256);


        public static void WriteResponseCookie<T>(this HttpContext httpContext, string key, T Value)
        {
            var cookievalue = JsonConvert.SerializeObject(Value);
            cookievalue = aESCryptography.Encrypt(cookievalue);

            var options = new CookieOptions();
            options.SameSite = SameSiteMode.Lax;
            options.Expires = DateTime.Now.AddDays(1);

            httpContext.Response.Cookies.Append(key, cookievalue, options);
        }

        public static T ReadRequestCookie<T>(this HttpContext httpContext, string key)
        {
            if (httpContext.Request.Cookies[key] != null)
            {
                var value = JsonConvert.DeserializeObject<T>(aESCryptography.Decrypt(httpContext.Request.Cookies[key]));
                return value;
            }

            return default(T);
        }
    }
}
