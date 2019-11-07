using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleAuthentication.InfraStructures.ActionFilters;

namespace SampleAuthentication.Controllers
{
    [ServiceFilter(typeof(CustomAuthorizeFilter))]
    public class HomeController : Controller
    {                
        public IActionResult Index()
        {
            return View(HttpContext.User.Claims);
        }

        [Authorize(Policy = "mobile")]
        public string ByMobile() => "Policy By Mobile Claim";

        [Authorize(Policy = "custommobile")]
        public string ByCustomMobile() => "Policy By Custom Mobile Claim";

        [Authorize(Policy = "role")]
        public string ByRole() => "Policy By Admin Role Claim";

        [Authorize(Policy = "mobilerole")]
        public string ByRoleMobile() => "Policy By Role And Mobile Claim";
    }
}