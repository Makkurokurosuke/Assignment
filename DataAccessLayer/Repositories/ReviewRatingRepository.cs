using Common.Models;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class ReviewRatingRepository : IReviewRatingRepository
    {

        public ReviewRatingRepository()
        {
        }

        public List<ReviewRatingTypeModel> GetAll()
        {

            List<ReviewRatingTypeModel> ratings = null;

            using (UserReviewContext context = new UserReviewContext())
            {
                ratings = (from ratingTbl in context.RatingTypes
                           orderby ratingTbl.Id
                           select new ReviewRatingTypeModel
                           {
                               EnglishDesc = ratingTbl.EnglishDesc,
                               FrenchDesc = ratingTbl.FrenchDesc,
                               Id = ratingTbl.Id
                           }).ToList();
            }
            return ratings;
        }

        public bool DoesRatingExist(int ratingId)
        {
            bool ratingExist = false;

            using (UserReviewContext context = new UserReviewContext())
            {
                int count = context.RatingTypes.Where(x => x.Id == ratingId).Count();
                ratingExist = count > 0;
            }

            return ratingExist;
        }
    }
}
