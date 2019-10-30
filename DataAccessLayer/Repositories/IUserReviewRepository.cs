using System.Collections.Generic;
using Common.Models;

namespace DataAccessLayer.Repositories
{
    public interface IUserReviewRepository
    {
        int Add(string comment, int userId, int ratingId);

        List<UserReviewModel> GetAll();

        UserReviewModel GetById(int reviewId);
    }
}