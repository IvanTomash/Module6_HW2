using System.Net;
using Basket.Host.Data;
using Basket.Host.Models.Requests;
using Basket.Host.Models.Response;
using Basket.Host.Services;
using Basket.Host.Services.Interfaces;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Host.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.AllowClientPolicy)]
[Route(ComponentDefaults.DefaultRoute)]
public class BasketItemController : ControllerBase
{
    private readonly ILogger<BasketItemController> _logger;
    private readonly IBasketItemService _basketItemService;

    public BasketItemController(
        ILogger<BasketItemController> logger,
        IBasketItemService basketItemService)
    {
        _logger = logger;
        _basketItemService = basketItemService;
    }

    [HttpPost]
    public async Task<IActionResult> Add(CreateProductRequest request)
    {
        var result = await _basketItemService.Add(request.Name, request.Description, request.Price, request.AvailableStock, request.CatalogBrandId, request.CatalogTypeId, request.PictureFileName);
        return Ok(new AddItemResponse<int?> { Id = result });
    }
}