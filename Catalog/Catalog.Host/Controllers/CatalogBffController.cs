using System.Net;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Response;
using Catalog.Host.Services.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Controllers;

[ApiController]
[Route(ComponentDefaults.DefaultRoute)]
public class CatalogBffController : ControllerBase
{
    private readonly ILogger<CatalogBffController> _logger;
    private readonly ICatalogService _catalogService;

    public CatalogBffController(
        ILogger<CatalogBffController> logger,
        ICatalogService catalogService)
    {
        _logger = logger;
        _catalogService = catalogService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(PaginatedItemsResponse<CatalogItemDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Items(PaginatedItemsRequest request)
    {
        var result = await _catalogService.GetCatalogItemsAsync(request.PageSize, request.PageIndex);
        return Ok(result);
    }

    [HttpPost("{id}")]
    public async Task<ActionResult<CatalogItemDto>> GetById(int id)
    {
        var paginatedCatalogItemsResponse = await _catalogService.GetCatalogItemsAsync(100, 0);
        var items = paginatedCatalogItemsResponse.Data.ToList<CatalogItemDto>();
        try
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Id == id)
                {
                    return Ok(items[i]);
                }
            }
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status404NotFound);
        }

        return StatusCode(StatusCodes.Status404NotFound);
    }

    [HttpPost("{brand}")]
    public async Task<ActionResult<CatalogItemDto>> GetByBrand(string brand, PaginatedItemsRequest request)
    {
        var paginatedCatalogItemsResponse = await _catalogService.GetCatalogItemsAsync(request.PageSize, request.PageIndex);
        var items = paginatedCatalogItemsResponse.Data.ToList<CatalogItemDto>();
        var result = new List<CatalogItemDto>();

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].CatalogBrand.Brand == brand)
            {
                result.Add(items[i]);
            }
        }

        if (result == null || result.Count == 0)
        {
            return StatusCode(StatusCodes.Status404NotFound);
        }

        return Ok(result);
    }

    [HttpPost("{type}")]
    public async Task<ActionResult<CatalogItemDto>> GetByType(string type, PaginatedItemsRequest request)
    {
        var paginatedCatalogItemsResponse = await _catalogService.GetCatalogItemsAsync(request.PageSize, request.PageIndex);
        var items = paginatedCatalogItemsResponse.Data.ToList<CatalogItemDto>();
        var result = new List<CatalogItemDto>();

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].CatalogType.Type == type)
            {
                result.Add(items[i]);
            }
        }

        if (result == null || result.Count == 0)
        {
            return StatusCode(StatusCodes.Status404NotFound);
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<List<CatalogBrandDto>>> GetBrands(PaginatedItemsRequest request)
    {
        var result = await _catalogService.GetCatalogBrandsAsync(request.PageSize, request.PageIndex);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<List<CatalogTypeDto>>> GetTypes(PaginatedItemsRequest request)
    {
        var result = await _catalogService.GetCatalogTypesAsync(request.PageSize, request.PageIndex);
        return Ok(result);
    }
}