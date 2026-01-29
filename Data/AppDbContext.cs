using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Work_Track.Models;

namespace Work_Track.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        //create a users table from user model
        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }

    }
}
