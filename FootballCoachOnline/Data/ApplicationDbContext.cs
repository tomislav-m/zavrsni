using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FootballCoachOnline.Models;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FootballCoachOnline.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationRole> ApplicationRole { get; set; }
        public DbSet<ApplicationUser> User { get; set; }
        public virtual DbSet<Club> Club { get; set; }
        public virtual DbSet<Competition> Competition { get; set; }
        public virtual DbSet<Match> Match { get; set; }
        public virtual DbSet<MatchScore> MatchScore { get; set; }
        public virtual DbSet<MatchStats> MatchStats { get; set; }
        public virtual DbSet<Player> Player { get; set; }
        public virtual DbSet<PlayerStats> PlayerStats { get; set; }
        public virtual DbSet<PlayerTeam> PlayerTeam { get; set; }
        public virtual DbSet<Team> Team { get; set; }
        public virtual DbSet<TeamCompetition> TeamCompetition { get; set; }
        public virtual DbSet<TeamStats> TeamStats { get; set; }
        public virtual DbSet<Test> Test { get; set; }
        public virtual DbSet<Training> Training { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("User");
            builder.Entity<IdentityRole>().ToTable("Role");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRole");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserToken");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaim");

            builder.Entity<Match>(entity =>
            {
                entity.HasOne(d => d.Team1)
                    .WithMany(p => p.MatchTeam1)
                    .HasForeignKey(d => d.Team1Id)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Match_Team");

                entity.HasOne(d => d.Team2)
                    .WithMany(p => p.MatchTeam2)
                    .HasForeignKey(d => d.Team2Id)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Match_Team1");
            });

            builder.Entity<PlayerTeam>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.TeamId })
                    .HasName("PK_PlayerTeam");
            });

            builder.Entity<TeamCompetition>(entity =>
            {
                entity.HasKey(e => new { e.TeamId, e.CompetitionId })
                    .HasName("PK_TeamCompetition");
            });
        }
    }
}
