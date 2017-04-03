using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FootballCoachOnline.Models
{
    public partial class ZavrsniDBContext : DbContext
    {
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
        public virtual DbSet<User> User { get; set; }

        public ZavrsniDBContext(DbContextOptions<ZavrsniDBContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Club>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Nickname).HasMaxLength(50);
            });

            modelBuilder.Entity<Competition>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Match>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.HasOne(d => d.Competition)
                    .WithMany(p => p.Match)
                    .HasForeignKey(d => d.CompetitionId)
                    .HasConstraintName("FK_Match_Competition");

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

            modelBuilder.Entity<MatchScore>(entity =>
            {
                entity.Property(e => e.Score1).HasDefaultValueSql("0");

                entity.Property(e => e.Score2).HasDefaultValueSql("0");

                entity.HasOne(d => d.Match)
                    .WithMany(p => p.MatchScore)
                    .HasForeignKey(d => d.MatchId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_MatchScore_Match");
            });

            modelBuilder.Entity<MatchStats>(entity =>
            {
                entity.Property(e => e.App).HasDefaultValueSql("0");

                entity.Property(e => e.Goals).HasDefaultValueSql("0");

                entity.Property(e => e.RedCard).HasDefaultValueSql("0");

                entity.Property(e => e.Sub).HasDefaultValueSql("0");

                entity.Property(e => e.YellowCard).HasDefaultValueSql("0");

                entity.HasOne(d => d.Match)
                    .WithMany(p => p.MatchStats)
                    .HasForeignKey(d => d.MatchId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_MatchStats_Match");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.MatchStats)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_MatchStats_Player");
            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Nationality)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasDefaultValueSql("N'Hrvat'");

                entity.Property(e => e.NaturalPosition).HasMaxLength(20);

                entity.Property(e => e.Physical).HasDefaultValueSql("0");

                entity.Property(e => e.PlaceOfBirth).HasMaxLength(50);

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PlayerStats>(entity =>
            {
                entity.Property(e => e.Apps).HasDefaultValueSql("0");

                entity.Property(e => e.Goals).HasDefaultValueSql("0");

                entity.Property(e => e.GoalsConceded).HasDefaultValueSql("0");

                entity.Property(e => e.RedCards).HasDefaultValueSql("0");

                entity.Property(e => e.Year).HasColumnType("date");

                entity.Property(e => e.YellowCards).HasDefaultValueSql("0");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.PlayerStats)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_PlayerStats_Player");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.PlayerStats)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_PlayerStats_Team");
            });

            modelBuilder.Entity<PlayerTeam>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.TeamId })
                    .HasName("PK_PlayerTeam");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.PlayerTeam)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_PlayerTeam_Player");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.PlayerTeam)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_PlayerTeam_Team");
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.Property(e => e.Age)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Club)
                    .WithMany(p => p.Team)
                    .HasForeignKey(d => d.ClubId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Team_Club");

                entity.HasOne(d => d.Coach)
                    .WithMany(p => p.Team)
                    .HasForeignKey(d => d.CoachId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Team_User");
            });

            modelBuilder.Entity<TeamCompetition>(entity =>
            {
                entity.HasKey(e => new { e.TeamId, e.CompetitionId })
                    .HasName("PK_TeamCompetition");

                entity.HasOne(d => d.Competition)
                    .WithMany(p => p.TeamCompetition)
                    .HasForeignKey(d => d.CompetitionId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_TeamCompetiiton_Competition");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.TeamCompetition)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_TeamCompetition_Team");
            });

            modelBuilder.Entity<TeamStats>(entity =>
            {
                entity.Property(e => e.Draws).HasDefaultValueSql("0");

                entity.Property(e => e.GamesPlayed).HasDefaultValueSql("0");

                entity.Property(e => e.GoalsConceded).HasDefaultValueSql("0");

                entity.Property(e => e.GoalsScored).HasDefaultValueSql("0");

                entity.Property(e => e.Losses).HasDefaultValueSql("0");

                entity.Property(e => e.Penalties).HasDefaultValueSql("0");

                entity.Property(e => e.PenaltiesConceded).HasDefaultValueSql("0");

                entity.Property(e => e.TeamId).HasDefaultValueSql("0");

                entity.Property(e => e.Wins).HasDefaultValueSql("0");

                entity.HasOne(d => d.Competition)
                    .WithMany(p => p.TeamStats)
                    .HasForeignKey(d => d.CompetitionId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_TeamStats_Competition");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.TeamStats)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_TeamStats_Team");
            });

            modelBuilder.Entity<Test>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.Test)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Test_Player");
            });

            modelBuilder.Entity<Training>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.Training)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Training_Team");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Mail)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }
    }
}