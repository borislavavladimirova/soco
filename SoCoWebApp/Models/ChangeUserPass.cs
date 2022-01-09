using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SoCoWebApp.Models
{
    public class ChangeUserPass
    {
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Display(Name = "Old password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        [MaxLength(30), MinLength(4)]
        public string OldPassword { get; set; }

        [Display(Name = "New password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        [MaxLength(30), MinLength(4)]
        public string NewPassword { get; set; }
    }
}