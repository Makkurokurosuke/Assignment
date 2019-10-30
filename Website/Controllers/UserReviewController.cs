using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using Website.Models;
using Website.Services;

namespace Website.Controllers
{
    [Route("{lang}/[controller]")]
    public class UserReviewController : Controller
    {
        private readonly IUserReviewApiService userReviewService;
        private readonly IReviewRatingApiService reviewRatingService;

        public UserReviewController(IUserReviewApiService reviewService,
            IReviewRatingApiService ratingService
            )
        {
            this.userReviewService = reviewService;
            this.reviewRatingService = ratingService;
        }

        [HttpGet]
        [Route("SubmitReview")]
        public async Task<IActionResult> SubmitReview()
        {
            var viewModel = new SubmitUserReviewViewModel();
            var ratings = await reviewRatingService.GetAll();
            viewModel.RatingTypeList = ratings.ToList();
            return View(viewModel);
        }

        [HttpPost]
        [Route("SubmitReview")]
        public async Task<IActionResult> SubmitReview(SubmitUserReviewViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var reviewModel = new SubmitUserReviewModel
                {
                    Comment = viewModel.Comment,
                    UserId = JWTHelper.GetJWTUserId(),
                    RatingId = Convert.ToInt32(viewModel.SelectedRating)
                };

                await userReviewService.Add(reviewModel);

                return RedirectToAction("ReviewSubmitted", "UserReview");
            }

            return View(viewModel);
        }

        [Route("ReviewSubmitted")]
        public IActionResult ReviewSubmitted()
        {
            return View();
        }

        [Route("ViewReviewList")]
        public async Task<IActionResult> ViewReviewList()
        {
            var viewModel = new ViewReviewListViewModel();

            var reviews = await userReviewService.GetAll();
            viewModel.ReviewList = reviews.ToList();

            return View(viewModel);
        }

        [Route("ViewReviewDetail")]
        public async Task<IActionResult> ViewReviewDetail(int id = 0)
        {
            ViewReviewDetailViewModel viewModel = new ViewReviewDetailViewModel();

            if (id != 0)
            {
                viewModel.Review = await userReviewService.GetById(id);
            }

            return View(viewModel);
        }


    }
}
