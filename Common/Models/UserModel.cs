using System;
using System.ComponentModel;

namespace Common.Models
{
    public class UserModel
    {
        public UserModel()
        {
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime? StoppedDate { get; set; }

    }
}
