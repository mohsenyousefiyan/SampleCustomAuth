using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SampleAuthentication.DAL;
using SampleAuthentication.InfraStructures.Extentions;
using SampleAuthentication.ViewModels;

namespace SampleAuthentication.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository userRepository;

        public AccountController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = userRepository.UserLogin(model.UserName,model.Password);
                if (user==null)
                    ModelState.AddModelError("", "نام کاربری یا کلمه عبور نادرست است");
                else
                {
                    var userToken = user;
                    HttpContext.WriteResponseCookie("UserToken", userToken);
                    return Redirect(returnUrl ?? "/");
                }
            }

            return View(model);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}