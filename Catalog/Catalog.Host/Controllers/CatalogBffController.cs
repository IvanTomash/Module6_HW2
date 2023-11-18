using System.Net;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Enums;
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
    public async Task<IActionResult> Items(PaginatedItemsRequest<CatalogTypeFilter> request)
    {
        var result = await _catalogService.GetCatalogItemsAsync(request.PageSize, request.PageIndex, request.Filters);
        return Ok(result);
    }

    [HttpPost("{id}")]
    public async Task<ActionResult<CatalogItemDto>> GetById(int id, PaginatedItemsRequest<CatalogTypeFilter> request)
    {
        var paginatedCatalogItemsResponse = await _catalogService.GetCatalogItemsAsync(request.PageSize, request.PageIndex, request.Filters);

        if (paginatedCatalogItemsResponse == null)
        {
            return StatusCode(StatusCodes.Status404NotFound);
        }

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
    public async Task<ActionResult<CatalogItemDto>> GetByBrand(string brand, PaginatedItemsRequest<CatalogTypeFilter> request)
    {
        var paginatedCatalogItemsResponse = await _catalogService.GetCatalogItemsAsync(request.PageSize, request.PageIndex, request.Filters);

        if (paginatedCatalogItemsResponse == null)
        {
            return StatusCode(StatusCodes.Status404NotFound);
        }

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
    public async Task<ActionResult<CatalogItemDto>> GetByType(string type, PaginatedItemsRequest<CatalogTypeFilter> request)
    {
        var paginatedCatalogItemsResponse = await _catalogService.GetCatalogItemsAsync(request.PageSize, request.PageIndex, request.Filters);

        if (paginatedCatalogItemsResponse == null)
        {
            return StatusCode(StatusCodes.Status404NotFound);
        }

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
    [ProducesResponseType(typeof(List<CatalogBrandDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<List<CatalogBrandDto>>> GetBrands()
    {
        var result = await _catalogService.GetCatalogBrandsAsync();
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(List<CatalogTypeDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<List<CatalogTypeDto>>> GetTypes()
    {
        var result = await _catalogService.GetCatalogTypesAsync();
        return Ok(result);
    }
}