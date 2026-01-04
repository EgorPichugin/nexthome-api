using Microsoft.EntityFrameworkCore;
using NextHome.Core.Entities;
using NextHome.Core.Interfaces;
using NextHome.Infrastructure.Persistence;

namespace NextHome.Infrastructure.Repositories;

public class ExperienceCardRepository(AppDbContext appDbContext) : IExperienceCardRepository
{
    public async Task<ExperienceCardEntity> Add(ExperienceCardEntity experienceCardEntity,
        CancellationToken cancellationToken = default)
    {
        appDbContext.ExperienceCards.Add(experienceCardEntity);
        await appDbContext.SaveChangesAsync(cancellationToken);
        return experienceCardEntity;
    }

    public Task<List<ExperienceCardEntity>> GetExperienceCardsByUserId(Guid userId,
        CancellationToken cancellationToken = default)
    {
        return appDbContext.ExperienceCards.Where(card => card.User.Id == userId).ToListAsync(cancellationToken);
    }

    public Task<ExperienceCardEntity?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        return appDbContext.ExperienceCards.FirstOrDefaultAsync(card => card.Id == id, cancellationToken);
    }

    public async Task Update(ExperienceCardEntity experienceCardEntity, CancellationToken cancellationToken = default)
    {
        appDbContext.ExperienceCards.Update(experienceCardEntity);
        await appDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await appDbContext.ExperienceCards.FindAsync([id], cancellationToken);
        if (entity != null)
        {
            appDbContext.ExperienceCards.Remove(entity);
            await appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}