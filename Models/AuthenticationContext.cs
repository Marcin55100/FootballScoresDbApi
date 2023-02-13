using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure.Internal;
using System;

namespace FootballScoresDbApi.Models
{
    public class AuthenticationContext : IdentityDbContext<ApplicationUser>
    {
        private readonly string _connectionString;


        public AuthenticationContext(DbContextOptions<AuthenticationContext> options) : base(options)
        {
            var extension = options.FindExtension<MySqlOptionsExtension>();
            if (!string.IsNullOrEmpty(extension?.ConnectionString))
            {
                _connectionString = extension.ConnectionString;
            }
        }

        public virtual DbSet<Team> Teams { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured && !string.IsNullOrEmpty(_connectionString))
            {
                optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString));
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>()
                .HasMany(x => x.Teams)
                .WithMany(x => x.Users)
                .UsingEntity(x=> x.ToTable("UsersTeams"));
            base.OnModelCreating(builder);
        }
    }
}
