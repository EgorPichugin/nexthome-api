using Microsoft.EntityFrameworkCore;
using NextHome.Core.Entities;
using NextHome.Core.Interfaces;
using NextHome.Infrastructure.Persistence;

namespace NextHome.Infrastructure.Repositories;

/// <inhetitdoc />
public class ChallengeCardRepository(AppDbContext appDbContext)
    : IChallengeCardRepository
{
    /// <inhetitdoc />
    public async Task<ChallengeCardEntity> Add(
        ChallengeCardEntity challengeCardEntity,
        CancellationToken cancellationToken = default)
    {
        appDbContext.ChallengeCards.Add(challengeCardEntity);
        await appDbContext.SaveChangesAsync(cancellationToken);
        return challengeCardEntity;
    }

    /// <inhetitdoc />
    public Task<List<ChallengeCardEntity>> GetChallengeCardsByUserId(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return appDbContext.ChallengeCards
            .Where(card => card.User.Id == userId)
            .ToListAsync(cancellationToken);
    }

    /// <inhetitdoc />
    public Task<ChallengeCardEntity?> GetById(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return appDbContext.ChallengeCards
            .FirstOrDefaultAsync(card => card.Id == id, cancellationToken);
    }
    
    /// <inhetitdoc />
    public async Task Update(
        ChallengeCardEntity challengeCardEntity,
        CancellationToken cancellationToken = default)
    {
        appDbContext.ChallengeCards.Update(challengeCardEntity);
        await appDbContext.SaveChangesAsync(cancellationToken);
    }
    
    /// <inhetitdoc />
    public async Task Delete(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await appDbContext.ChallengeCards
            .FindAsync([id], cancellationToken);

        if (entity != null)
        {
            appDbContext.ChallengeCards.Remove(entity);
            await appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}