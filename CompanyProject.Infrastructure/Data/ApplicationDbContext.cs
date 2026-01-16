using CompanyProject.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CompanyProject.Infrastructure.Data
{
    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ChangeHistory> ChangeHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Explicit PK (THIS FIXES YOUR ERROR)
            modelBuilder.Entity<ChangeHistory>()
                .HasKey(ch => ch.ChangeId);

            // Explicit relationships (recommended with Identity)
            modelBuilder.Entity<ChangeHistory>()
                .HasOne(ch => ch.Company)
                .WithMany()
                .HasForeignKey(ch => ch.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChangeHistory>()
                .HasOne(ch => ch.Project)
                .WithMany()
                .HasForeignKey(ch => ch.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
