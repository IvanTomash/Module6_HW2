using Basket.Host.Data;
using Basket.Host.Repositories.Interfaces;
using Basket.Host.Services.Interfaces;

namespace Basket.Host.Services;

public class BasketItemService : BaseDataService<ApplicationDbContext>, IBasketItemService
{
    private readonly IBasketItemRepository _basketItemRepository;

    public BasketItemService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        IBasketItemRepository basketItemRepository)
        : base(dbContextWrapper, logger)
    {
        _basketItemRepository = basketItemRepository;
    }

    public Task<int?> Add(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName)
    {
        return ExecuteSafeAsync(() => _basketItemRepository.Add(name, description, price, availableStock, catalogBrandId, catalogTypeId, pictureFileName));
    }
}
