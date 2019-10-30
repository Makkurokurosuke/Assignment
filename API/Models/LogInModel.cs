using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class LogInModel
    {
        public LogInModel()
        {

        }

        [Required]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        [MaxLength(20)]
        public string Password { get; set; }


    }
}