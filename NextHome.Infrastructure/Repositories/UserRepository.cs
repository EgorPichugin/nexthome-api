using Microsoft.EntityFrameworkCore;
using NextHome.Core.Entities;
using NextHome.Core.Interfaces;
using NextHome.Infrastructure.Persistence;

namespace NextHome.Infrastructure.Repositories;

/// <inheritdoc/>
public sealed class UserRepository(AppDbContext appDbContext) : IUserRepository
{
    /// <inheritdoc/>
    public async Task<UserEntity> Add(UserEntity userEntity, CancellationToken cancellationToken = default)
    {
        appDbContext.Users.Add(userEntity);
        await appDbContext.SaveChangesAsync(cancellationToken);
        return userEntity;
    }

    /// <inheritdoc/>
    public Task<UserEntity?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        return appDbContext.Users.FirstOrDefaultAsync(
            user => user.Id == id,
            cancellationToken
        );
    }

    /// <inheritdoc/>
    public Task<UserEntity?> GetByEmail(string email, CancellationToken cancellationToken = default)
    {
        return appDbContext.Users.FirstOrDefaultAsync(
            user => user.Email == email,
            cancellationToken
        );
    }

    /// <inheritdoc/>
    public Task<List<UserEntity>> GetAll(CancellationToken cancellationToken = default)
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
    public async Task Update(UserEntity userEntity, CancellationToken cancellationToken = default)
    {
        appDbContext.Users.Update(userEntity);
        await appDbContext.SaveChangesAsync(cancellationToken);
    }

    
}