using System.Net;
using Basket.Host.Services.Interfaces;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Host.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
[Route(ComponentDefaults.DefaultRoute)]
public class BasketBffController : ControllerBase
{
    private readonly ILogger<BasketBffController> _logger;

    public BasketBffController(ILogger<BasketBffController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public IActionResult LogMessage(string message)
    {
        _logger.LogInformation(message);
        return Ok();
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public IActionResult LogUserId()
    {
        _logger.LogInformation($"User Id {User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value}");
        return Ok();
    }
}