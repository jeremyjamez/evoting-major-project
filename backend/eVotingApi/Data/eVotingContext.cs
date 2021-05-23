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

        public DbSet<Election> Elections { get; set; }
        public DbSet<PollingDivision> PollingDivisions { get; set; }
        public DbSet<PollingCentre> PollingCentres { get; set; }
        public DbSet<PollingStation> PollingStations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder
                //.UseSqlServer(Configuration);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PollingCentre>()
                .HasOne(pc => pc.PollingDivision)
                .WithMany(pd => pd.PollingCentres)
                .HasForeignKey(pc => pc.DivisionId);

            modelBuilder.Entity<PollingStation>()
                .HasOne(ps => ps.PollingCentre)
                .WithMany(pc => pc.PollingStations)
                .HasForeignKey(ps => ps.CentreId);             
        }
    }
}
