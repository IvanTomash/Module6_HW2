using Basket.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Basket.Host.Services;

public class DbContextWrapper<T> : IDbContextWrapper<T>
    where T : DbContext
{
    private readonly T _dbContext;

    public DbContextWrapper(
        IDbContextFactory<T> dbContextFactory)
    {
        this._dbContext = dbContextFactory.CreateDbContext();
    }

    public T DbContext => this._dbContext;

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return this._dbContext.Database.BeginTransactionAsync(cancellationToken);
    }
}
