using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using DataAccessLayer.Repositories;

namespace API.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ReviewRatingController : Controller
    {
        private readonly IReviewRatingRepository reviewRatingRepository;

        public ReviewRatingController(IReviewRatingRepository ratingRepo)
        {
            this.reviewRatingRepository = ratingRepo;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var reviews = reviewRatingRepository.GetAll();

            return new ObjectResult(reviews);
        }

    }
}
