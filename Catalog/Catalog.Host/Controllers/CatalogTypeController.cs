using System.Net;
using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Response;
using Catalog.Host.Services.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Controllers;

[ApiController]
[Route(ComponentDefaults.DefaultRoute)]
public class CatalogTypeController : ControllerBase
{
    private readonly ILogger<CatalogTypeController> _logger;
    private readonly ICatalogTypeService _catalogTypeService;

    public CatalogTypeController(
        ILogger<CatalogTypeController> logger,
        ICatalogTypeService catalogTypeService)
    {
        _logger = logger;
        _catalogTypeService = catalogTypeService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(AddItemResponse<int?>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Add(CreateTypeRequest request)
    {
        var result = await _catalogTypeService.Add(request.Type);
        return Ok(new AddItemResponse<int?>() { Id = result });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _catalogTypeService.Delete(id);

        if (result == null)
        {
            return NotFound(result);
        }

        return Ok(new AddItemResponse<int?>() { Id = result });
    }

    [HttpPost]
    [ProducesResponseType(typeof(AddItemResponse<int?>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(UpdateTypeRequest request)
    {
        var result = await _catalogTypeService.Update(request.Id, request.Type);

        if (result == null)
        {
            return NotFound(result);
        }

        return Ok(new AddItemResponse<int?>() { Id = result });
    }
}