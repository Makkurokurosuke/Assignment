using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Common.Models
{
    public class UserReviewModel
    {
        public UserReviewModel()
        {
        }

        public int Id { get; set; }


        public string Comment { get; set; }
        public DateTime? CreatedDate { get; set; }
        public UserModel User { get; set; }
        public ReviewRatingTypeModel ReviewRating { get; set; }

        public bool ShouldSerializeUser()
        {
            if (this.User != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ShouldSerializeComment()
        {
            if (!String.IsNullOrEmpty(this.Comment))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
