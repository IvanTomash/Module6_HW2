using Basket.Host.Data;
using Basket.Host.Data.Entities;
using Basket.Host.Repositories.Interfaces;
using Basket.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Basket.Host.Repositories;

public class BasketItemRepository : IBasketItemRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<BasketItemRepository> _logger;

    public BasketItemRepository(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BasketItemRepository> logger)
    {
        _dbContext = dbContextWrapper.DbContext;
        _logger = logger;
    }

    public async Task<int?> Add(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName)
    {
        var item = await _dbContext.AddAsync(new CatalogItem
        {
            CatalogBrandId = catalogBrandId,
            CatalogTypeId = catalogTypeId,
            Description = description,
            Name = name,
            PictureFileName = pictureFileName,
            Price = price,
        });

        await _dbContext.SaveChangesAsync();

        return item.Entity.Id;
    }

    public async Task<PaginatedItems<CatalogItem>> GetByPageAsync(int pageIndex, int pageSize, int? brandFilter, int? typeFilter)
    {
        IQueryable<CatalogItem> query = _dbContext.CatalogItems;

        if (brandFilter.HasValue)
        {
            query = query.Where(w => w.CatalogBrandId == brandFilter.Value);
        }

        if (typeFilter.HasValue)
        {
            query = query.Where(w => w.CatalogTypeId == typeFilter.Value);
        }

        var totalItems = await query.LongCountAsync();

        var itemsOnPage = await query.OrderBy(c => c.Name)
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedItems<CatalogItem>() { TotalCount = totalItems, Data = itemsOnPage };
    }
}
