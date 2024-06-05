using System;
using Microsoft.EntityFrameworkCore;
using NewWorldEmploymentServices.EntityModels;

namespace NewWorldEmploymentServices.AppWebDbContext
{
	public class AppDbContext: DbContext 
	{
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
          : base(options)
        {

        }

        public DbSet<Organization> Organizations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<PostJob> PostJobs { get; set; }
        public DbSet<JobCategory> JobCategories { get; set; }
        public DbSet<JobNature> JobNatures { get; set; }
    }
}

