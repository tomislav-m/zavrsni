using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using FootballCoachOnline.Data;
using FootballCoachOnline.Models;

namespace FootballCoachOnline.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170503231524_TeamId_MatchStats")]
    partial class TeamId_MatchStats
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FootballCoachOnline.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("User");
                });

            modelBuilder.Entity("FootballCoachOnline.Models.Club", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("Nickname");

                    b.Property<int?>("YearFounded");

                    b.HasKey("Id");

                    b.ToTable("Club");
                });

            modelBuilder.Entity("FootballCoachOnline.Models.Competition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Competition");
                });

            modelBuilder.Entity("FootballCoachOnline.Models.Match", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CompetitionId");

                    b.Property<DateTime>("Date");

                    b.Property<int>("MatchScoreId");

                    b.Property<int>("Matchday");

                    b.Property<bool>("Played");

                    b.Property<int>("Team1Id");

                    b.Property<int>("Team2Id");

                    b.HasKey("Id");

                    b.HasIndex("CompetitionId");

                    b.HasIndex("MatchScoreId")
                        .IsUnique();

                    b.HasIndex("Team1Id");

                    b.HasIndex("Team2Id");

                    b.ToTable("Match");
                });

            modelBuilder.Entity("FootballCoachOnline.Models.MatchScore", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Score1");

                    b.Property<int>("Score2");

                    b.HasKey("Id");

                    b.ToTable("MatchScore");
                });

            modelBuilder.Entity("FootballCoachOnline.Models.MatchStats", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("App");

                    b.Property<int>("Goals");

                    b.Property<int?>("GoalsConceded");

                    b.Property<int>("MatchId");

                    b.Property<int>("PlayerId");

                    b.Property<bool>("RedCard");

                    b.Property<bool>("Sub");

                    b.Property<int>("TeamId");

                    b.Property<bool>("YellowCard");

                    b.HasKey("Id");

                    b.HasIndex("MatchId");

                    b.HasIndex("PlayerId");

                    b.HasIndex("TeamId");

                    b.ToTable("MatchStats");
                });

            modelBuilder.Entity("FootballCoachOnline.Models.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("Name");

                    b.Property<string>("Nationality");

                    b.Property<int>("NaturalPosition");

                    b.Property<bool>("Physical");

                    b.Property<string>("PlaceOfBirth");

                    b.Property<string>("Role");

                    b.Property<string>("Surname");

                    b.Property<int>("TeamId");

                    b.HasKey("Id");

                    b.ToTable("Player");
                });

            modelBuilder.Entity("FootballCoachOnline.Models.PlayerStats", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Apps");

                    b.Property<int>("Goals");

                    b.Property<int>("GoalsConceded");

                    b.Property<int>("PlayerId");

                    b.Property<int>("RedCards");

                    b.Property<int>("Subs");

                    b.Property<int>("TeamId");

                    b.Property<DateTime>("Year");

                    b.Property<int>("YellowCards");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.HasIndex("TeamId");

                    b.ToTable("PlayerStats");
                });

            modelBuilder.Entity("FootballCoachOnline.Models.PlayerTeam", b =>
                {
                    b.Property<int>("PlayerId");

                    b.Property<int>("TeamId");

                    b.HasKey("PlayerId", "TeamId")
                        .HasName("PK_PlayerTeam");

                    b.HasIndex("TeamId");

                    b.ToTable("PlayerTeam");
                });

            modelBuilder.Entity("FootballCoachOnline.Models.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Age")
                        .IsRequired();

                    b.Property<int>("ClubId");

                    b.Property<string>("CoachId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ClubId");

                    b.ToTable("Team");
                });

            modelBuilder.Entity("FootballCoachOnline.Models.TeamCompetition", b =>
                {
                    b.Property<int>("TeamId");

                    b.Property<int>("CompetitionId");

                    b.HasKey("TeamId", "CompetitionId")
                        .HasName("PK_TeamCompetition");

                    b.HasIndex("CompetitionId");

                    b.ToTable("TeamCompetition");
                });

            modelBuilder.Entity("FootballCoachOnline.Models.TeamStats", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CompetitionId");

                    b.Property<int>("Draws");

                    b.Property<int>("GamesPlayed");

                    b.Property<int>("GoalsConceded");

                    b.Property<int>("GoalsScored");

                    b.Property<int>("Losses");

                    b.Property<int>("Penalties");

                    b.Property<int>("PenaltiesConceded");

                    b.Property<int>("TeamId");

                    b.Property<int>("Wins");

                    b.Property<int>("Year");

                    b.HasKey("Id");

                    b.HasIndex("CompetitionId");

                    b.HasIndex("TeamId");

                    b.ToTable("TeamStats");
                });

            modelBuilder.Entity("FootballCoachOnline.Models.Test", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<int>("PlayerId");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.ToTable("Test");
                });

            modelBuilder.Entity("FootballCoachOnline.Models.Training", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description");

                    b.Property<int>("TeamId");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.ToTable("Training");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("Role");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityRole");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaim");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaim");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogin");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserToken");
                });

            modelBuilder.Entity("FootballCoachOnline.Models.ApplicationRole", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole");

                    b.Property<string>("Description");

                    b.ToTable("ApplicationRole");

                    b.HasDiscriminator().HasValue("ApplicationRole");
                });

            modelBuilder.Entity("FootballCoachOnline.Models.Match", b =>
                {
                    b.HasOne("FootballCoachOnline.Models.Competition", "Competition")
                        .WithMany("Match")
                        .HasForeignKey("CompetitionId");

                    b.HasOne("FootballCoachOnline.Models.MatchScore", "MatchScore")
                        .WithOne("Match")
                        .HasForeignKey("FootballCoachOnline.Models.Match", "MatchScoreId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FootballCoachOnline.Models.Team", "Team1")
                        .WithMany("MatchTeam1")
                        .HasForeignKey("Team1Id")
                        .HasConstraintName("FK_Match_Team");

                    b.HasOne("FootballCoachOnline.Models.Team", "Team2")
                        .WithMany("MatchTeam2")
                        .HasForeignKey("Team2Id")
                        .HasConstraintName("FK_Match_Team1");
                });

            modelBuilder.Entity("FootballCoachOnline.Models.MatchStats", b =>
                {
                    b.HasOne("FootballCoachOnline.Models.Match", "Match")
                        .WithMany("MatchStats")
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FootballCoachOnline.Models.Player", "Player")
                        .WithMany("MatchStats")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FootballCoachOnline.Models.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FootballCoachOnline.Models.PlayerStats", b =>
                {
                    b.HasOne("FootballCoachOnline.Models.Player", "Player")
                        .WithMany("PlayerStats")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FootballCoachOnline.Models.Team", "Team")
                        .WithMany("PlayerStats")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FootballCoachOnline.Models.PlayerTeam", b =>
                {
                    b.HasOne("FootballCoachOnline.Models.Player", "Player")
                        .WithMany("PlayerTeam")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FootballCoachOnline.Models.Team", "Team")
                        .WithMany("PlayerTeam")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FootballCoachOnline.Models.Team", b =>
                {
                    b.HasOne("FootballCoachOnline.Models.Club", "Club")
                        .WithMany("Team")
                        .HasForeignKey("ClubId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FootballCoachOnline.Models.TeamCompetition", b =>
                {
                    b.HasOne("FootballCoachOnline.Models.Competition", "Competition")
                        .WithMany("TeamCompetition")
                        .HasForeignKey("CompetitionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FootballCoachOnline.Models.Team", "Team")
                        .WithMany("TeamCompetition")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FootballCoachOnline.Models.TeamStats", b =>
                {
                    b.HasOne("FootballCoachOnline.Models.Competition", "Competition")
                        .WithMany("TeamStats")
                        .HasForeignKey("CompetitionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FootballCoachOnline.Models.Team", "Team")
                        .WithMany("TeamStats")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FootballCoachOnline.Models.Test", b =>
                {
                    b.HasOne("FootballCoachOnline.Models.Player", "Player")
                        .WithMany("Test")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FootballCoachOnline.Models.Training", b =>
                {
                    b.HasOne("FootballCoachOnline.Models.Team", "Team")
                        .WithMany("Training")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("FootballCoachOnline.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("FootballCoachOnline.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FootballCoachOnline.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
