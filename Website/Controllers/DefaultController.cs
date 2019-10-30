using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Common.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Website.Models;
using Website.Services;

namespace Website.Controllers
{
    [Route("{lang}/[controller]")]
    public class DefaultController : Controller
    {
        private readonly IAuthorizationApiService authorizationService;

        public DefaultController(IAuthorizationApiService authService
           )
        {
            this.authorizationService = authService;
        }


        [HttpGet]
        [Route("LogIn")]
        public IActionResult LogIn()
        {
            // var sessionExpired = TempData["SessionExpired"];
            var view = new LogInViewModel();

            return View(view);
        }


        [HttpPost]
        [Route("LogIn")]
        public async Task<IActionResult> LogIn(LogInViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                UserModel userModel = new UserModel
                {
                    Password = viewModel.Password,
                    Username = viewModel.UserName
                };

                var token = await authorizationService.LogIn(userModel);
                if (!String.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Index", "Home");
                }

            }

            ModelState.AddModelError(nameof(LogInViewModel.UserName), "Username or password is invalid.");


            return View(viewModel);
        }

        [Route("LogOut")]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("LogIn", "Default");
        }


        [Route("Error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
