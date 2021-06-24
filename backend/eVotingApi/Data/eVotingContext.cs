using eVotingApi.Models;
using eVotingApi.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace eVotingApi.Data
{
    public class eVotingContext : IdentityDbContext<ApplicationUser>
    {
        public eVotingContext(DbContextOptions<eVotingContext> options)
            :base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);          
        }
    }
}
