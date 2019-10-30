using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models
{
    public partial class RatingTypes
    {
        public RatingTypes()
        {
            Reviews = new HashSet<Reviews>();
        }

        public int Id { get; set; }
        public string EnglishDesc { get; set; }
        public string FrenchDesc { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual ICollection<Reviews> Reviews { get; set; }
    }
}
