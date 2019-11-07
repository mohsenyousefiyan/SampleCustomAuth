using Microsoft.AspNetCore.Authentication;
using SampleAuthentication.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SampleAuthentication.InfraStructures.Extentions
{
    public class UserClaimsProvider : IClaimsTransformation
    {
        private readonly IUserRepository userRepository;

        public UserClaimsProvider(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if(principal!=null)
            {
                ClaimsIdentity identity = principal.Identity as ClaimsIdentity;
                if (identity != null && identity.IsAuthenticated && identity.Name != null)
                {
                    var userclaims = userRepository.GetUserClaims(identity.GetUserId());
                    foreach (var item in userclaims)
                        identity.AddClaim(new Claim(type: item.Key, value: item.Value, valueType: ClaimValueTypes.String, issuer: "FromDb"));
                }            
            }

            return Task.FromResult(principal);
        }
    }
}
