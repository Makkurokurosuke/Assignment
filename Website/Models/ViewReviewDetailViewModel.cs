using Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Website.Models
{
    public class ViewReviewDetailViewModel
    {
        public ViewReviewDetailViewModel()
        {
            Review = new UserReviewModel();
        }



        public UserReviewModel Review { get; set; }



    }
}