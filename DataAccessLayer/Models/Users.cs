using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models
{
    public partial class Users
    {
        public Users()
        {
            Reviews = new HashSet<Reviews>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime? StoppedDate { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual ICollection<Reviews> Reviews { get; set; }
    }
}
