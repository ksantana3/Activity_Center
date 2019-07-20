using System;
using Microsoft.EntityFrameworkCore;
using Activity_Center.Models;
using System.Linq;
using Activity = Activity_Center.Models.Activity;


namespace Activity_Center.Models
{
    public class DBContext : DbContext {
        public DBContext(DbContextOptions options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            optionsBuilder.UseSqlite("Filename=mydb.db");
        }
        public DbSet<User> Users {get;set;}
        public DbSet<Activity> Activities {get;set;}
        public DbSet<Join> Joins {get;set;}
    }
}