using Microsoft.AspNetCore.Mvc.RazorPages;
using MVC.Dtos;
using MVC.Models.Enums;
using MVC.Models.Responses;
using MVC.Services.Interfaces;
using MVC.ViewModels;

namespace MVC.Services;

public class CatalogService : ICatalogService
{
    private readonly IOptions<AppSettings> _settings;
    private readonly IHttpClientService _httpClient;
    private readonly ILogger<CatalogService> _logger;

    public CatalogService(IHttpClientService httpClient, ILogger<CatalogService> logger, IOptions<AppSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings;
        _logger = logger;
    }

    public async Task<Catalog> GetCatalogItems(int page, int take, int? brand, int? type)
    {
        var filters = new Dictionary<CatalogTypeFilter, int>();

        if (brand.HasValue)
        {
            filters.Add(CatalogTypeFilter.Brand, brand.Value);
        }
        
        if (type.HasValue)
        {
            filters.Add(CatalogTypeFilter.Type, type.Value);
        }
        
        var result = await _httpClient.SendAsync<Catalog, PaginatedItemsRequest<CatalogTypeFilter>>($"{_settings.Value.CatalogUrl}/items",
           HttpMethod.Post, 
           new PaginatedItemsRequest<CatalogTypeFilter>()
            {
                PageIndex = page,
                PageSize = take,
                Filters = filters
            });

        return result;
    }

    public async Task<IEnumerable<SelectListItem>> GetBrands()
    {
        var brands = await _httpClient.SendAsync<ItemsResponse<CatalogBrand>, PaginatedItemsRequest<CatalogTypeFilter>>($"{_settings.Value.CatalogUrl}/GetBrands", HttpMethod.Post, null);
        var list = brands.Data.ToList();
        var result = new List<SelectListItem>();
        foreach (var item in list)
        {
            result.Add(new SelectListItem(item.Brand, item.Id.ToString()));
        }

        return result;
    }

    public async Task<IEnumerable<SelectListItem>> GetTypes()
    {
        var types = await _httpClient.SendAsync<ItemsResponse<CatalogType>, PaginatedItemsRequest<CatalogTypeFilter>>($"{_settings.Value.CatalogUrl}/GetTypes", HttpMethod.Post, null);
        var list = types.Data.ToList();
        var result = new List<SelectListItem>();
        foreach (var item in list)
        {
            result.Add(new SelectListItem(item.Type, item.Id.ToString()));
        }

        return result;
    }
}
