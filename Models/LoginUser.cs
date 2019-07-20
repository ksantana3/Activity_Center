using System;
using System.ComponentModel.DataAnnotations;
using Activity = Activity_Center.Models.Activity;

namespace Activity_Center.Models
{
    public class LoginUser
    {
        [Required (ErrorMessage="Email address is required!")]
        [EmailAddress]
        public string LoginEmail {get;set;}
        [Required (ErrorMessage="Password is required!")]
        [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
        public string LoginPassword {get;set;}
    }
}