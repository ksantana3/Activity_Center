using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Activity = Activity_Center.Models.Activity;

namespace Activity_Center.Models
{
    public class Join
    {
        [Key]
        public int JoinId {get;set;}
        public int UserId {get;set;}
        public User Joiner {get;set;}
        public int ActivityId {get;set;}
        public Activity JoinedActivity {get;set;}
    }
}