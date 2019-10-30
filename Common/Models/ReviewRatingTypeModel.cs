using System;
using System.ComponentModel;

namespace Common.Models
{
    public class ReviewRatingTypeModel
    {
        public ReviewRatingTypeModel()
        {
        }

        public int Id { get; set; }
        public string EnglishDesc { get; set; }
        public string FrenchDesc { get; set; }

    }
}
