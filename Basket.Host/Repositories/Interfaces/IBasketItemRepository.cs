using Basket.Host.Data;
using Basket.Host.Data.Entities;

namespace Basket.Host.Repositories.Interfaces;

public interface IBasketItemRepository
{
    Task<PaginatedItems<CatalogItem>> GetByPageAsync(int pageIndex, int pageSize, int? brandFilter, int? typeFilter);

    Task<int?> Add(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName);
}
