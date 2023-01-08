using BookRentalManager.Domain.Common;
using BookRentalManager.Infrastructure.Extensions;

namespace BookRentalManager.Infrastructure.Common;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
{
    private readonly AppDbContext _appDbContext;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
        _dbSet = appDbContext.Set<TEntity>();
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(
        int pageIndex,
        int totalItemsPerPage,
        CancellationToken cancellationToken = default
    )
    {
        return await _dbSet.ToReadOnlyPaginatedListAsync(
            cancellationToken,
            pageIndex,
            totalItemsPerPage
        );
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(
        int pageIndex,
        int totalItemsPerPage,
        Specification<TEntity> specification,
        CancellationToken cancellationToken = default
    )
    {
        return await _dbSet
            .Where(specification.ToExpression())
            .ToReadOnlyPaginatedListAsync(
                cancellationToken,
                pageIndex,
                totalItemsPerPage
            );
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(id, cancellationToken);
    }

    public async Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity);
        await SaveAsync(cancellationToken);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(entity);
        await SaveAsync(cancellationToken);
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Remove(entity);
        await SaveAsync(cancellationToken);
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await _appDbContext.SaveChangesAsync();
    }
}
