using System.Linq;
using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories
{
    public class CatalogBrandRepository : ICatalogBrandRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<CatalogBrandRepository> _logger;

        public CatalogBrandRepository(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<CatalogBrandRepository> logger)
        {
            _dbContext = dbContextWrapper.DbContext;
            _logger = logger;
        }

        public async Task<PaginatedItems<CatalogBrand>> GetByPageAsync(int pageIndex, int pageSize)
        {
            var totalItems = await _dbContext.CatalogBrands
                .LongCountAsync();

            var itemsOnPage = await _dbContext.CatalogBrands
                .OrderBy(c => c.Id)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedItems<CatalogBrand>() { TotalCount = totalItems, Data = itemsOnPage };
        }

        public async Task<int?> Add(string brand)
        {
            var item = await _dbContext.AddAsync(new CatalogBrand
            {
                Brand = brand
            });

            await _dbContext.SaveChangesAsync();

            return item.Entity.Id;
        }

        public async Task<int?> Delete(int id)
        {
            var item = await _dbContext.CatalogBrands.FindAsync(id);

            if (item != null)
            {
                var removedItem = _dbContext.CatalogBrands.Remove(item);

                await _dbContext.SaveChangesAsync();

                return removedItem.Entity.Id;
            }

            return 0;
        }

        public async Task<int?> Update(int id, string brand)
        {
            var item = await _dbContext.CatalogBrands.FindAsync(id);
            if (item != null)
            {
                item.Id = id;
                item.Brand = brand;

                await _dbContext.SaveChangesAsync();

                return item.Id;
            }

            return 0;
        }
    }
}
