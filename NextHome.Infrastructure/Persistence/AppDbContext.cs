using Microsoft.EntityFrameworkCore;
using NextHome.Core.Entities;

namespace NextHome.Infrastructure.Persistence;

/// <summary>
/// Database context.
/// </summary>
public interface IAppDbContext
{
    DbSet<User> Users { get; }
}

/// <summary>
/// Represents the application's database context, managing entity sets and configuration.
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
{
    public DbSet<User> Users => Set<User>();
}