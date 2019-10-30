using System;
using System.ComponentModel.DataAnnotations;

namespace Website.Models
{
    public class LogInViewModel
    {
        public LogInViewModel()
        {

        }

        [Required]
        [Display(Name = "Username", ResourceType = typeof(Resources.SharedResource))]
        [StringLength(20)]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Password", ResourceType = typeof(Resources.SharedResource))]
        [StringLength(20)]
        public string Password { get; set; }


    }
}