﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using eVotingApi.Models;

namespace eVotingApi.Migrations
{
    [DbContext(typeof(eVotingContext))]
    [Migration("20201227165515_UpdateFK")]
    partial class UpdateFK
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("eVotingApi.Models.Candidate", b =>
                {
                    b.Property<long>("MemberId")
                        .HasColumnType("bigint");

                    b.Property<long>("ConstituencyId")
                        .HasColumnType("bigint");

                    b.HasKey("MemberId");

                    b.HasIndex("ConstituencyId");

                    b.ToTable("Candidate");
                });

            modelBuilder.Entity("eVotingApi.Models.Candidate_Vote", b =>
                {
                    b.Property<long>("ConstituencyId")
                        .HasColumnType("bigint");

                    b.Property<long>("CandidateId")
                        .HasColumnType("bigint");

                    b.Property<long>("ElectionId")
                        .HasColumnType("bigint");

                    b.Property<long>("VoteCount")
                        .HasColumnType("bigint");

                    b.HasKey("ConstituencyId", "CandidateId");

                    b.HasIndex("CandidateId");

                    b.HasIndex("ElectionId");

                    b.ToTable("Candidate_Vote");
                });

            modelBuilder.Entity("eVotingApi.Models.Constituency", b =>
                {
                    b.Property<long>("ConstituencyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<long>("MemberId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Parish")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RegisteredVotersCount")
                        .HasColumnType("int");

                    b.HasKey("ConstituencyId");

                    b.ToTable("Constituency");
                });

            modelBuilder.Entity("eVotingApi.Models.Election", b =>
                {
                    b.Property<long>("ElectionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<DateTime>("ElectionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ElectionType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ElectionId");

                    b.ToTable("Election");
                });

            modelBuilder.Entity("eVotingApi.Models.Member", b =>
                {
                    b.Property<long>("MemberId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("JoinDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MiddleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("PartyId")
                        .HasColumnType("bigint");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Prefix")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Suffix")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MemberId");

                    b.HasIndex("PartyId");

                    b.ToTable("Member");
                });

            modelBuilder.Entity("eVotingApi.Models.PoliticalParty", b =>
                {
                    b.Property<long>("PartyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PartyId");

                    b.ToTable("PoliticalParty");
                });

            modelBuilder.Entity("eVotingApi.Models.PollingCentre", b =>
                {
                    b.Property<long>("CentreId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<long>("DivisionId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("PollingDivisionDivisionId")
                        .HasColumnType("bigint");

                    b.HasKey("CentreId");

                    b.HasIndex("PollingDivisionDivisionId");

                    b.ToTable("PollingCentre");
                });

            modelBuilder.Entity("eVotingApi.Models.PollingDivision", b =>
                {
                    b.Property<long>("DivisionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<long>("ConstituencyId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DivisionId");

                    b.HasIndex("ConstituencyId");

                    b.ToTable("PollingDivision");
                });

            modelBuilder.Entity("eVotingApi.Models.PollingStation", b =>
                {
                    b.Property<long>("StationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<long>("CentreId")
                        .HasColumnType("bigint");

                    b.Property<long?>("PollingCentreCentreId")
                        .HasColumnType("bigint");

                    b.Property<int>("StationNumber")
                        .HasColumnType("int");

                    b.HasKey("StationId");

                    b.HasIndex("PollingCentreCentreId");

                    b.ToTable("PollingStation");
                });

            modelBuilder.Entity("eVotingApi.Models.Vote", b =>
                {
                    b.Property<long>("VoteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<long>("CandidateId")
                        .HasColumnType("bigint");

                    b.Property<long>("ElectionId")
                        .HasColumnType("bigint");

                    b.Property<long>("VoterId")
                        .HasColumnType("bigint");

                    b.HasKey("VoteId");

                    b.HasIndex("ElectionId");

                    b.HasIndex("VoterId");

                    b.ToTable("Vote");
                });

            modelBuilder.Entity("eVotingApi.Models.Voter", b =>
                {
                    b.Property<long>("VoterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("ConstituencyId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MiddleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Occupation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Prefix")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telephone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("VoterId");

                    b.HasIndex("ConstituencyId");

                    b.ToTable("Voter");
                });

            modelBuilder.Entity("eVotingApi.Models.Candidate", b =>
                {
                    b.HasOne("eVotingApi.Models.Constituency", "Constituency")
                        .WithMany("Candidates")
                        .HasForeignKey("ConstituencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("eVotingApi.Models.Member", "Member")
                        .WithOne("Candidate")
                        .HasForeignKey("eVotingApi.Models.Candidate", "MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Constituency");

                    b.Navigation("Member");
                });

            modelBuilder.Entity("eVotingApi.Models.Candidate_Vote", b =>
                {
                    b.HasOne("eVotingApi.Models.Candidate", "Candidate")
                        .WithMany("Candidate_Votes")
                        .HasForeignKey("CandidateId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("eVotingApi.Models.Constituency", "Constituency")
                        .WithMany("Candidate_Votes")
                        .HasForeignKey("ConstituencyId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("eVotingApi.Models.Election", "Election")
                        .WithMany("Candidate_Votes")
                        .HasForeignKey("ElectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Candidate");

                    b.Navigation("Constituency");

                    b.Navigation("Election");
                });

            modelBuilder.Entity("eVotingApi.Models.Member", b =>
                {
                    b.HasOne("eVotingApi.Models.PoliticalParty", "PoliticalParty")
                        .WithMany("Members")
                        .HasForeignKey("PartyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PoliticalParty");
                });

            modelBuilder.Entity("eVotingApi.Models.PollingCentre", b =>
                {
                    b.HasOne("eVotingApi.Models.PollingDivision", "PollingDivision")
                        .WithMany("PollingCentres")
                        .HasForeignKey("PollingDivisionDivisionId");

                    b.Navigation("PollingDivision");
                });

            modelBuilder.Entity("eVotingApi.Models.PollingDivision", b =>
                {
                    b.HasOne("eVotingApi.Models.Constituency", "Constituency")
                        .WithMany("PollingDivisions")
                        .HasForeignKey("ConstituencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Constituency");
                });

            modelBuilder.Entity("eVotingApi.Models.PollingStation", b =>
                {
                    b.HasOne("eVotingApi.Models.PollingCentre", "PollingCentre")
                        .WithMany("PollingStations")
                        .HasForeignKey("PollingCentreCentreId");

                    b.Navigation("PollingCentre");
                });

            modelBuilder.Entity("eVotingApi.Models.Vote", b =>
                {
                    b.HasOne("eVotingApi.Models.Election", "Election")
                        .WithMany("Votes")
                        .HasForeignKey("ElectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("eVotingApi.Models.Voter", "Voter")
                        .WithMany("Votes")
                        .HasForeignKey("VoterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Election");

                    b.Navigation("Voter");
                });

            modelBuilder.Entity("eVotingApi.Models.Voter", b =>
                {
                    b.HasOne("eVotingApi.Models.Constituency", "Constituency")
                        .WithMany("Voters")
                        .HasForeignKey("ConstituencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Constituency");
                });

            modelBuilder.Entity("eVotingApi.Models.Candidate", b =>
                {
                    b.Navigation("Candidate_Votes");
                });

            modelBuilder.Entity("eVotingApi.Models.Constituency", b =>
                {
                    b.Navigation("Candidate_Votes");

                    b.Navigation("Candidates");

                    b.Navigation("PollingDivisions");

                    b.Navigation("Voters");
                });

            modelBuilder.Entity("eVotingApi.Models.Election", b =>
                {
                    b.Navigation("Candidate_Votes");

                    b.Navigation("Votes");
                });

            modelBuilder.Entity("eVotingApi.Models.Member", b =>
                {
                    b.Navigation("Candidate");
                });

            modelBuilder.Entity("eVotingApi.Models.PoliticalParty", b =>
                {
                    b.Navigation("Members");
                });

            modelBuilder.Entity("eVotingApi.Models.PollingCentre", b =>
                {
                    b.Navigation("PollingStations");
                });

            modelBuilder.Entity("eVotingApi.Models.PollingDivision", b =>
                {
                    b.Navigation("PollingCentres");
                });

            modelBuilder.Entity("eVotingApi.Models.Voter", b =>
                {
                    b.Navigation("Votes");
                });
#pragma warning restore 612, 618
        }
    }
}
