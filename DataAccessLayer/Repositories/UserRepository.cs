using Common.Models;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository
    {

        public UserRepository()
        {
        }


        public int FindUser(UserModel userModel)
        {
            int userId = 0;

            using (UserReviewContext context = new UserReviewContext())
            {
                userId = context.Users.Where(x => x.Username == userModel.Username
                            && x.Password == userModel.Password)
                            .Select(x => x.Id).FirstOrDefault();
            }

            return userId;
        }

        public bool DoesUserExist(int userId)
        {
            bool userExist = false;

            using (UserReviewContext context = new UserReviewContext())
            {
                int count = context.Users.Where(x => x.Id == userId).Count();
                userExist = count > 0;
            }

            return userExist;
        }
    }
}
