using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace SampleAuthentication.InfraStructures.Extentions
{
    public static class IdentityExtensions
    {
        public static string FindFirstValue(this ClaimsIdentity identity, string claimType)
        {
            return identity?.FindFirst(claimType)?.Value;
        }

        public static string FindFirstValue(this IIdentity identity, string claimType)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            return claimsIdentity?.FindFirstValue(claimType);
        }

        public static Int32 GetUserId(this IIdentity identity)
        {
            if (identity != null)
            {
                var id = identity.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrEmpty(id))
                    return Convert.ToInt32(id);
            }
            return -1;
        }

        public static Int32 GetUserAppId(this IIdentity identity)
        {
            if (identity != null)
            {
                var id = identity.FindFirstValue("UserAppId");
                if (!string.IsNullOrEmpty(id))
                    return Convert.ToInt32(id);
            }
            return -1;
        }


        public static string GetUserName(this IIdentity identity)
        {
            return identity?.FindFirstValue(ClaimTypes.Name);
        }
    }
}
