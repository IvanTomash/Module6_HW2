using Catalog.Host.Data.Entities;
using Catalog.Host.Data;

namespace Catalog.Host.Repositories.Interfaces
{
    public interface ICatalogBrandRepository
    {
        Task<PaginatedItems<CatalogBrand>> GetByPageAsync();
        Task<int?> Add(string brand);
        Task<int?> Delete(int id);
        Task<int?> Update(int id, string brand);
    }
}
