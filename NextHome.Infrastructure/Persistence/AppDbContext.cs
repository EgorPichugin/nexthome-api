using Microsoft.EntityFrameworkCore;
using NextHome.Core.Entities;

namespace NextHome.Infrastructure.Persistence;

/// <summary>
/// Database context.
/// </summary>
public interface IAppDbContext
{
    DbSet<UserEntity> Users { get; }
}

/// <summary>
/// Represents the application's database context, managing entity sets and configuration.
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
{
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<ExperienceCardEntity> ExperienceCards => Set<ExperienceCardEntity>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ExperienceCardEntity>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.UserId)
                .IsRequired();

            entity.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(4000);

            entity.HasOne(x => x.User)
                .WithMany(u => u.ExperienceCards)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}