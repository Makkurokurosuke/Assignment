using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using DataAccessLayer.Repositories;
using System.Net;
using System.Security.Claims;
using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using Common.Models;
using Common;
using API.Models;

namespace API.Controllers
{
    [Route("api/v1/[controller]")]
    public class AccountController : Controller
    {
        private readonly IUserRepository userRepository;

        public AccountController(IUserRepository userRepo)
        {
            this.userRepository = userRepo;
        }


        [HttpPost("Login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LogInModel model)
        {

            if (ModelState.IsValid)
            {

                Common.Models.UserModel userModel = new Common.Models.UserModel
                {
                    Username = model.Username,
                    Password = model.Password
                };

                int userId = userRepository.FindUser(userModel);

                if (userId != 0)
                {

                    var token = JWTHelper.GenerateToken(model.Username);

                    var result = new
                    {
                        Token = token,
                        UserName = model.Username,
                        UserId = userId,
                        ExpiresIn = AuthConfig.ApiJwtExpirationSec
                    };

                    return new CustomJsonResult(HttpStatusCode.OK, result);
                }
                else
                {

                    return new CustomJsonResult(HttpStatusCode.Unauthorized, new Common.Extension.UserNotFoundException("Username or password is incorrect."));

                }
            }
            else
            {
                throw Common.Extension.ModelValidationException.ErrorFactory(ModelState);
            }

        }


    }
}
