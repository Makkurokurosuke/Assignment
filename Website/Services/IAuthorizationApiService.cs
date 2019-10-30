using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Models;

namespace Website.Services
{
    public interface IAuthorizationApiService
    {
        Task<string> LogIn(UserModel userModel);
    }
}