using System.Collections.Generic;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Common.Models;

namespace API.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserReviewController : Controller
    {
        private readonly IUserReviewRepository userReviewRepository;
        private readonly IUserRepository userRepository;
        private readonly IReviewRatingRepository reviewRatingRepository;

        public UserReviewController(IUserReviewRepository reviewRepo,
            IReviewRatingRepository ratingRepo,
            IUserRepository userRepo)
        {
            this.userReviewRepository = reviewRepo;
            this.userRepository = userRepo;
            this.reviewRatingRepository = ratingRepo;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var reviews = userReviewRepository.GetAll();


            return new ObjectResult(reviews);
        }

        [HttpPost("Add")]
        public IActionResult Add([FromBody]SubmitUserReviewModel review)
        {

            if (ModelState.IsValid)
            {
                bool userFound = userRepository.DoesUserExist(review.UserId);
                bool ratingFound = reviewRatingRepository.DoesRatingExist(review.RatingId);

                if (!userFound)
                {
                    ModelState.AddModelError(nameof(review.UserId), "User id is invalid. The user does not exist.");
                }
                if (!ratingFound)
                {

                    ModelState.AddModelError(nameof(review.RatingId), "Rating id is invalid. The rating does not exist.");
                }

                if ((!userFound) || (!ratingFound))
                {
                    throw Common.Extension.ModelValidationException.ErrorFactory(ModelState);
                }

                review.ReviewId = userReviewRepository.Add(review.Comment, review.UserId, review.RatingId);
                return new ObjectResult(review);


            }
            else
            {
                throw Common.Extension.ModelValidationException.ErrorFactory(ModelState);
            }
        }


        [HttpGet("GetById/{reviewId}")]
        public IActionResult GetById(int reviewId)
        {
            var review = userReviewRepository.GetById(reviewId);

            if (review == null)
            {
                throw new Common.Extension.DataNotFoundException("There is no review found for the review id.");
            }

            return new ObjectResult(review);
        }
    }
}
