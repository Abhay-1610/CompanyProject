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
            modelBuilder.Entity<ChangeHistory>()
                .HasKey(ch => ch.ChangeId);

            modelBuilder.Entity<ChangeHistory>()
           .HasOne<Company>()
           .WithMany()
           .HasForeignKey(ch => ch.CompanyId)
           .OnDelete(DeleteBehavior.NoAction);

            // 🔴 IMPORTANT: NO CASCADE FROM PROJECT
            modelBuilder.Entity<ChangeHistory>()
                .HasOne<Project>()
                .WithMany()
                .HasForeignKey(ch => ch.ProjectId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
