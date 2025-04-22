using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Persistence
{
    /// <summary>
    /// Entity Framework Core database context
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<WorkItem> WorkItems { get; set; }
        public DbSet<WorkItemComment> WorkItemComments { get; set; }
        public DbSet<WorkItemHistory> WorkItemHistories { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.Owner)
                .WithMany()
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.Members)
                .WithOne()
                .HasForeignKey(pm => pm.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProjectMember>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(pm => pm.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Project>()
                .Ignore("UserId");

            modelBuilder.Entity<WorkItem>()
                .Property(w => w.Status)
                .HasConversion<string>()
                .HasMaxLength(10);

            modelBuilder.Entity<WorkItem>()
                .Property(w => w.Priority)
                .HasConversion<string>()
                .HasMaxLength(10);

            modelBuilder.Entity<WorkItem>()
                .HasMany(w => w.Comments)
                .WithOne()
                .HasForeignKey(c => c.WorkItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkItem>()
                .Navigation(w => w.Comments)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .AutoInclude();

            modelBuilder.Entity<WorkItem>()
                .Navigation(w => w.History)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .AutoInclude();
        }
    }
}