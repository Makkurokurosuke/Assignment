using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models
{
    public partial class Reviews
    {
        public int Id { get; set; }
        public int RatingTypeId { get; set; }
        public string Comment { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int UserId { get; set; }

        public virtual RatingTypes RatingType { get; set; }
        public virtual Users User { get; set; }
    }
}
