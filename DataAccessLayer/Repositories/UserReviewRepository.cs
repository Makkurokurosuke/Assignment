using Common.Models;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class UserReviewRepository : IUserReviewRepository
    {

        public UserReviewRepository()
        {
        }

        public List<UserReviewModel> GetAll()
        {
            List<UserReviewModel> reviews = null;

            using (UserReviewContext context = new UserReviewContext())
            {
                reviews = (from reviewTbl in context.Reviews
                           join ratingTbl in context.RatingTypes
                           on reviewTbl.RatingTypeId equals ratingTbl.Id
                           orderby reviewTbl.CreatedDate descending
                           select new UserReviewModel
                           {
                               Id = reviewTbl.Id,
                               Comment = reviewTbl.Comment,
                               CreatedDate = reviewTbl.CreatedDate,
                               ReviewRating = new ReviewRatingTypeModel
                               {
                                   EnglishDesc = ratingTbl.EnglishDesc,
                                   FrenchDesc = ratingTbl.FrenchDesc,
                                   Id = ratingTbl.Id
                               }
                           }).ToList();
            }
            return reviews;
        }

        public UserReviewModel GetById(int reviewId)
        {
            UserReviewModel review = null;

            using (UserReviewContext context = new UserReviewContext())
            {
                review = context.Reviews.Where(x => x.Id == reviewId)
                    .Select
                    (x => new UserReviewModel
                    {
                        Id = x.Id,
                        Comment = x.Comment,
                        CreatedDate = x.CreatedDate,
                        ReviewRating = new ReviewRatingTypeModel
                        {
                            Id = x.RatingTypeId
                        }
                    }
                    ).FirstOrDefault();

                if (review != null)
                {

                    review.ReviewRating = context.RatingTypes.Where(x => x.Id == review.ReviewRating.Id)
                    .Select
                    (x => new ReviewRatingTypeModel
                    {
                        EnglishDesc = x.EnglishDesc,
                        FrenchDesc = x.FrenchDesc,
                        Id = x.Id
                    }
                    ).FirstOrDefault();
                }
            }

            return review;
        }

        public int Add(string comment, int userId, int ratingId)
        {
            using (UserReviewContext context = new UserReviewContext())
            {

                var timeUtc = DateTime.UtcNow;
                TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);

                var review = new DataAccessLayer.Models.Reviews
                {
                    Comment = comment,
                    CreatedDate = easternTime,
                    UserId = userId,
                    RatingTypeId = ratingId
                };
                context.Reviews.Add(review);
                context.SaveChanges();

                return review.Id;
            }

        }

    }
}
