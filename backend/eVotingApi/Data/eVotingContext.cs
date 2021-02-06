using eVotingApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace eVotingApi.Data
{
    public class eVotingContext : IdentityDbContext
    {
        public eVotingContext(DbContextOptions<eVotingContext> options)
            :base(options)
        {

        }

        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Election> Elections { get; set; }
        public DbSet<Voter> Voters { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Constituency> Constituencies { get; set; }
        public DbSet<PollingDivision> PollingDivisions { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<PollingCentre> PollingCentres { get; set; }
        public DbSet<PollingStation> PollingStations { get; set; }
        public DbSet<PoliticalParty> PoliticalParties { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MemberOfParliament> MemberOfParliaments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder
                //.UseSqlServer(Configuration);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Vote>()
                .HasOne(v => v.Voter)
                .WithMany(vo => vo.Votes)
                .HasForeignKey(v => v.VoterId);

            modelBuilder.Entity<Voter>()
                .HasOne(v => v.Constituency)
                .WithMany(c => c.Voters)
                .HasForeignKey(v => v.ConstituencyId);

            modelBuilder.Entity<Member>()
                .HasOne(m => m.PoliticalParty)
                .WithMany(pp => pp.Members)
                .HasForeignKey(m => m.PartyId);

            modelBuilder.Entity<Member>()
                .HasOne<Candidate>(m => m.Candidate)
                .WithOne(c => c.Member)
                .HasForeignKey<Candidate>(c => c.CandidateId);

            modelBuilder.Entity<PollingCentre>()
                .HasOne(pc => pc.PollingDivision)
                .WithMany(pd => pd.PollingCentres)
                .HasForeignKey(pc => pc.DivisionId);

            modelBuilder.Entity<PollingStation>()
                .HasOne(ps => ps.PollingCentre)
                .WithMany(pc => pc.PollingStations)
                .HasForeignKey(ps => ps.CentreId);

            modelBuilder.Entity<MemberOfParliament>()
                .HasKey(k => new { k.ConstituencyId, k.CandidateId });                
        }
    }
}
