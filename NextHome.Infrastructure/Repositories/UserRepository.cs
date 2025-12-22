using Microsoft.EntityFrameworkCore;
using NextHome.Core.Entities;
using NextHome.Core.Interfaces;
using NextHome.Infrastructure.Persistence;

namespace NextHome.Infrastructure.Repositories;

/// <inheritdoc/>
public class UserRepository(AppDbContext appDbContext) : IUserRepository
{
    /// <inheritdoc/>
    public async Task Add(User user, CancellationToken cancellationToken = default)
    {
        appDbContext.Users.Add(user);
        await appDbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task<User?> GetByEmail(string email, CancellationToken cancellationToken = default)
    {
        return appDbContext.Users.FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
    }
    
    /// <inheritdoc/>
    public Task<List<User>> GetAll(CancellationToken cancellationToken = default)
    {
        return appDbContext.Users.ToListAsync(cancellationToken);
    }
    
    /// <inheritdoc/>
    public Task<bool> Exists(string email, CancellationToken cancellationToken = default)
    {
        return appDbContext.Users.AnyAsync(user => user.Email == email, cancellationToken);
    }
}