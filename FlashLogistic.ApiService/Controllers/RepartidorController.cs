using FlashLogistic.Application.DTOs.Repartidor;
using FlashLogistic.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlashLogistic.ApiService.Controllers;

[ApiController]
[Route("api/repartidores")]
public class RepartidorController : ControllerBase
{
    private readonly ILogger<PaqueteController> _logger;
    private readonly IRepartidorService _repartidorService;

    public RepartidorController(ILogger<PaqueteController> logger, IRepartidorService repartidorService)
    {
        _logger = logger;
        _repartidorService = repartidorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] RepartidorInputDTO input)
    {
        var data = await _repartidorService.ListRepartidoresAsync(input);
        return Ok(data);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var data = await _repartidorService.GetRepartidorAsync(id);
        return Ok(data);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePaquete(CreateRepartidorDTO input)
    {
        await _repartidorService.CreateRepartidorAsync(input);
        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdatePaquete(Guid id, CreateRepartidorDTO input)
    {
        await _repartidorService.UpdateRepartidorAsync(id, input);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeletePaquete(Guid id)
    {
        await _repartidorService.DeleteRepartidorAsync(id);
        return NoContent();
    }
}
