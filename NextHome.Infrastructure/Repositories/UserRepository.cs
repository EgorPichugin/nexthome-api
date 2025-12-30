using Microsoft.EntityFrameworkCore;
using NextHome.Core.Entities;
using NextHome.Core.Interfaces;
using NextHome.Infrastructure.Persistence;

namespace NextHome.Infrastructure.Repositories;

/// <inheritdoc/>
public sealed class UserRepository(AppDbContext appDbContext) : IUserRepository
{
    /// <inheritdoc/>
    public async Task<User> Add(User user, CancellationToken cancellationToken = default)
    {
        appDbContext.Users.Add(user);
        await appDbContext.SaveChangesAsync(cancellationToken);
        return user;
    }

    /// <inheritdoc/>
    public Task<User?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        return appDbContext.Users.FirstOrDefaultAsync(
            user => user.Id == id,
            cancellationToken
        );
    }

    /// <inheritdoc/>
    public Task<User?> GetByEmail(string email, CancellationToken cancellationToken = default)
    {
        return appDbContext.Users.FirstOrDefaultAsync(
            user => user.Email == email,
            cancellationToken
        );
    }

    /// <inheritdoc/>
    public Task<List<User>> GetAll(CancellationToken cancellationToken = default)
    {
        return appDbContext.Users.ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task<bool> Exists(string email, CancellationToken cancellationToken = default)
    {
        return appDbContext.Users.AnyAsync(
            user => user.Email == email,
            cancellationToken
        );
    }

    /// <inheritdoc/>
    public async Task Update(User user, CancellationToken cancellationToken = default)
    {
        appDbContext.Users.Update(user);
        await appDbContext.SaveChangesAsync(cancellationToken);
    }
}