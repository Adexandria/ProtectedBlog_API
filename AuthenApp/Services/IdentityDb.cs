using AuthenApp.BlogModel;
using AuthenApp.UserModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenApp.Services
{
    public class IdentityDb:IdentityDbContext<SignUp>
    {
        public IdentityDb(DbContextOptions<IdentityDb> options):base(options)
        {
           
        }
        public DbSet<Blog> BlogsP { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
