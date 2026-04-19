using FlashLogistic.Application.DTOs.Paquetes;
using FlashLogistic.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlashLogistic.ApiService.Controllers;

[ApiController]
[Route("api/paquetes")]
public class PaqueteController : ControllerBase
{
    private readonly ILogger<PaqueteController> _logger;
    private readonly IPaqueteService _paqueteService;

    public PaqueteController(ILogger<PaqueteController> logger, IPaqueteService paqueteService)
    {
        _logger = logger;
        _paqueteService = paqueteService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaquetesInputDTO input)
    {
        var data = await _paqueteService.ListPaquetesAsync(input);
        return Ok(data);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var data = await _paqueteService.GetPaqueteAsync(id);
        return Ok(data);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePaquete(CreatePaqueteDTO input)
    {
        await _paqueteService.CreatePaqueteAsync(input);
        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdatePaquete(Guid id, UpdatePaqueteDTO input)
    {
        await _paqueteService.UpdatePaqueteAsync(id, input);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeletePaquete(Guid id)
    {
        await _paqueteService.DeletePaqueteAsync(id);
        return NoContent();
    }

    [HttpPut("{id:guid}/repartidor")]
    public async Task<IActionResult> AssignRepartidor(Guid id, [FromBody] AssignRepartidorDTO input)
    {
        await _paqueteService.AsignarRepartidorAsync(id, input.RepartidorId);
        return NoContent();
    }
}
