using Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Website.Models
{
    public class ViewReviewListViewModel
    {
        public ViewReviewListViewModel()
        {
            ReviewList = new List<UserReviewModel>();
        }



        public List<UserReviewModel> ReviewList { get; set; }



    }
}