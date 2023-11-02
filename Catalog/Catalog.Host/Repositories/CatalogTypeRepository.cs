using System.Linq;
using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories
{
    public class CatalogTypeRepository : ICatalogTypeRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<CatalogTypeRepository> _logger;

        public CatalogTypeRepository(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<CatalogTypeRepository> logger)
        {
            _dbContext = dbContextWrapper.DbContext;
            _logger = logger;
        }

        public async Task<PaginatedItems<CatalogType>> GetByPageAsync(int pageIndex, int pageSize)
        {
            var totalItems = await _dbContext.CatalogTypes
                .LongCountAsync();

            var itemsOnPage = await _dbContext.CatalogTypes
                .OrderBy(c => c.Id)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedItems<CatalogType>() { TotalCount = totalItems, Data = itemsOnPage };
        }

        public async Task<int?> Add(string type)
        {
            var item = await _dbContext.AddAsync(new CatalogType
            {
                Type = type
            });

            await _dbContext.SaveChangesAsync();

            return item.Entity.Id;
        }

        public async Task<int?> Delete(int id)
        {
            var item = await _dbContext.CatalogTypes.FindAsync(id);

            if (item != null)
            {
                var removedItem = _dbContext.CatalogTypes.Remove(item);

                await _dbContext.SaveChangesAsync();

                return removedItem.Entity.Id;
            }

            return null;
        }

        public async Task<int?> Update(int id, string type)
        {
            var item = await _dbContext.CatalogTypes.FindAsync(id);
            if (item != null)
            {
                item.Id = id;
                item.Type = type;

                await _dbContext.SaveChangesAsync();

                return item.Id;
            }

            return null;
        }
    }
}
