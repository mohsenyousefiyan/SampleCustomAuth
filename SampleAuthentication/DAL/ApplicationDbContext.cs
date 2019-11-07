using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using SampleAuthentication.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleAuthentication.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserClaims> UserClaims { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        
    }
}
