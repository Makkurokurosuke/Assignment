using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Common.Models
{
    public class SubmitUserReviewModel
    {
        public SubmitUserReviewModel()
        {
        }

        [Required]
        [MaxLength(250)]
        public string Comment { get; set; }

        //user id is not mandatory if it is anonymous user id would be 0
        public int UserId { get; set; }

        [Required]
        public int RatingId { get; set; }


        public int ReviewId { get; set; }
    }
}
