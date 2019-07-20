using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Activity_Center.Models;
using Activity = Activity_Center.Models.Activity;

namespace Activity_Center.Controllers
{
    public class HomeController : Controller
    {

        private static DBContext context;
        private PasswordHasher<User> RegisterHasher = new PasswordHasher<User>();
        private PasswordHasher<LoginUser> LoginHasher = new PasswordHasher<LoginUser>();
        
        public HomeController(DBContext DBContext)
        {
            context = DBContext;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(User u)
        {
            if(ModelState.IsValid)
            {
                User register_user = context.Users.FirstOrDefault(e => e.Email == u.Email);
                if(register_user != null)
                {
                    ModelState.AddModelError("Email", "Email Address Exists");
                }
                else
                {
                    u.Password = RegisterHasher.HashPassword(u, u.Password);
                    context.Users.Add(u);
                    context.SaveChanges();
                    HttpContext.Session.SetInt32("UserId", u.UserId);
                    return Redirect("/home");
                }
            }
            return View("Index");
        }

        [HttpPost("login")]
        public IActionResult Login(LoginUser l)
        {
            if(ModelState.IsValid)
            {
                User logging_in_user = context.Users.FirstOrDefault(u => u.Email == l.LoginEmail);
                if(logging_in_user != null)
                {
                    var result = LoginHasher.VerifyHashedPassword(l, logging_in_user.Password, l.LoginPassword);
                    if(result == 0)
                    {
                        ModelState.AddModelError("LoginPassword", "Invalid Password");
                    }
                    else
                    {
                        HttpContext.Session.SetInt32("UserId", logging_in_user.UserId);
                        return Redirect("/home");
                    }
                }
                else
                {
                    ModelState.AddModelError("LoginEmail", "Invalid Email");
                }
            }
            return View("Index");
        }

        [HttpGet("home")]
        public IActionResult Home()
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if(UserId == null)
            {
                return Redirect("/");
            } 

            List<Activity> Activities = context.Activities
                .Include(a => a.Planner)
                .Include(a => a.AttendingUsers)
                .OrderBy(a => a.StartTime).ToList();
            
            for(int i=0; i<Activities.Count; i++)
            {
                if(Activities[i].StartTime < DateTime.Now)
                {
                    Activities.Remove(Activities[i]);
                }
            }
           
            ViewBag.Activities = Activities;
            ViewBag.UserId = UserId;
            return View();
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            return Redirect("/");
        }

        [HttpGet("activity/new")]
        public IActionResult NewActivity()
        {
            return View();
        }

        [HttpPost("activity")]
        public IActionResult CreateActivity(Activity a)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if(UserId == null)
            {
                return Redirect("/");
            } 
            if(ModelState.IsValid)
            {
                a.PlannerId = (int) UserId;
                context.Activities.Add(a);
                context.SaveChanges();
                return Redirect("/home");
            }
            else
            {
                return View("NewActivity", a);
            }
        }

        [HttpGet("edit/{ActivityId}")]
        public IActionResult Edit(int ActivityId)
        {
            Activity act = context.Activities.FirstOrDefault(a => a.ActivityId == ActivityId);
            return View(act);
        }

        [HttpGet("delete/{ActivityId}")]
        public IActionResult Delete(int ActivityId)
        {
            Activity a = context.Activities.FirstOrDefault(act => act.ActivityId == ActivityId);
            context.Activities.Remove(a);
            context.SaveChanges();
            return Redirect("/home");
        }

        [HttpGet("view/{ActivityId}")]
        public IActionResult ShowActivity(int ActivityId)
        {
            Activity a = context.Activities
                .Include(act => act.Planner)
                .Include(act => act.AttendingUsers)
                .ThenInclude(act => act.Joiner)
                .FirstOrDefault(act => act.ActivityId == ActivityId);
            ViewBag.Joins = a.AttendingUsers;
            return View(a);
        }

        [HttpGet("join/{ActivityId}")]
        public IActionResult Join(int ActivityId)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if(UserId == null)
            {
                return Redirect("/");
            } 
            Join j = new Join(){
                UserId = (int) UserId,
                ActivityId = ActivityId
            };
            context.Joins.Add(j);
            context.SaveChanges();
            return Redirect("/home");
        }

        [HttpGet("leave/{ActivityId}")]
        public IActionResult Leave(int ActivityId)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if(UserId == null)
            {
                return Redirect("/");
            } 
            Join join = context.Joins
                .Where(j => j.ActivityId == ActivityId)
                .FirstOrDefault(j => j.UserId == (int) UserId);
            context.Joins.Remove(join);
            context.SaveChanges();
            return Redirect("/home");
        }

        [HttpPost("update/{ActivityId}")]
        public IActionResult Update(int ActivityId, Activity a)
        {
            if(ModelState.IsValid)
            {
                Activity act = context.Activities.FirstOrDefault(actv => actv.ActivityId == ActivityId);
                act.Title = a.Title;
                act.Time = a.Time;
                act.Description = a.Description;
                act.StartTime = a.StartTime;
                act.Duration = a.Duration;
                context.SaveChanges();
                return Redirect("/home");
            }
            else
            {
                return View("Edit", a);
            }
        }

    }
}
