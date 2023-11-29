namespace Basket.Host.Services.Interfaces;

public interface IBasketItemService
{
    Task<int?> Add(
        string name,
        string description,
        decimal price,
        int availableStock,
        int catalogBrandId,
        int catalogTypeId,
        string pictureFileName);
}
