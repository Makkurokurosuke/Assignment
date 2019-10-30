using Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Website.Models
{
    public class SubmitUserReviewViewModel
    {
        public SubmitUserReviewViewModel()
        {
            RatingTypeList = new List<ReviewRatingTypeModel>();
        }

        public string SelectedRating { get; set; }


        public List<ReviewRatingTypeModel> RatingTypeList { get; set; }

        [Required]
        [Display(Name = "Comment", ResourceType = typeof(Resources.SharedResource))]
        [StringLength(250)]
        public string Comment { get; set; }


    }
}