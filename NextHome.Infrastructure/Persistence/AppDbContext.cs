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
    /// <summary>
    /// Gets the DbSet for the User entity.
    /// </summary>
    public DbSet<UserEntity> Users => Set<UserEntity>();
    
    /// <summary>
    /// Gets the DbSet for the ExperienceCard entity.
    /// </summary>
    public DbSet<ExperienceCardEntity> ExperienceCards => Set<ExperienceCardEntity>();
    
    /// <summary>
    /// Gets the DbSet for the ChallengeCard entity.
    /// </summary>
    public DbSet<ChallengeCardEntity> ChallengeCards => Set<ChallengeCardEntity>();
    
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
        
        modelBuilder.Entity<ChallengeCardEntity>(entity =>
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
                .WithMany(u => u.ChallengeCards)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}