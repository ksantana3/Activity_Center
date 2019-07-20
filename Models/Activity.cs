using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace Activity_Center.Models
{
    public class Activity
    {
        [Key]
        public int ActivityId {get;set;}
        [Required (ErrorMessage="Activity name is required!")]
        public string Title {get;set;}
        [Required (ErrorMessage="Days/ Hours/ Minutes selection is required!")]
        public int Time {get;set;}
        [Required (ErrorMessage="Description is required!")]
        public string Description {get;set;}
        [Required (ErrorMessage="Activity start time is required!")]
        
        public DateTime StartTime {get;set;}
        [Required (ErrorMessage="Duration of activity is required!")]
        [Range(1, 25000, ErrorMessage = "Please enter a valid duration")]
        public int Duration {get;set;}
        public int PlannerId {get;set;}
        public User Planner {get;set;}
        public List<Join> AttendingUsers {get;set;}

        public void Display()
        {
            Console.WriteLine($"{Title} {Time} {StartTime}");
        }
    }
}