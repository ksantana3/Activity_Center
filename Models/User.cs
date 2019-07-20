using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Activity = Activity_Center.Models.Activity;

namespace Activity_Center.Models
{
    public class User
    {
        [Key]
        public int UserId {get;set;}
        [Required (ErrorMessage="First name is required!")]
        [MinLength(2, ErrorMessage="First name must consist of 2 or more characters")]
        public string FirstName {get;set;}
        [Required (ErrorMessage="Last name is required!")]
        [MinLength(2, ErrorMessage="Last name must consist of 2 or more characters")]
        public string LastName {get;set;}
        [Required (ErrorMessage="Email address is required!")]
        [EmailAddress]
        public string Email {get;set;}
        [Required (ErrorMessage="Password is required!")]
        [MinLength(8)]
        public string Password {get;set;}
        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string Confirm {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        public List<Activity> PlannedActivities {get;set;}
        public List<Join> AttendingActivities {get;set;}
    }
}