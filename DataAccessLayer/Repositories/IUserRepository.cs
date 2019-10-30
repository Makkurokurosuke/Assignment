using System.Collections.Generic;
using Common.Models;

namespace DataAccessLayer.Repositories
{
    public interface IUserRepository
    {
        int FindUser(UserModel userModel);
        bool DoesUserExist(int userId);
    }
}