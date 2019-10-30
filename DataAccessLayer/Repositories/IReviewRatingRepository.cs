using System.Collections.Generic;
using Common.Models;

namespace DataAccessLayer.Repositories
{
    public interface IReviewRatingRepository
    {
        List<ReviewRatingTypeModel> GetAll();

        bool DoesRatingExist(int ratingId);
    }
}