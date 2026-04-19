using AutoMapper;
using FlashLogistic.Application.DTOs;
using FlashLogistic.Application.DTOs.Repartidor;
using FlashLogistic.Application.Interfaces;
using FlashLogistic.Domain.Entities;
using FlashLogistic.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlashLogistic.Application.Services;

internal class RepartidorService : IRepartidorService
{
    private readonly ILogger<RepartidorService> _logger;
    private readonly IRepartidorRepository _repartidorRepository;
    private readonly IMapper _mapper;

    public RepartidorService(
        IRepartidorRepository repartidorRepository,
        IMapper mapper,
        ILogger<RepartidorService> logger)
    {
        _repartidorRepository = repartidorRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<RepartidorDTO> GetRepartidorAsync(Guid id)
    {
        _logger.LogInformation("Consultando el repartidor con ID: {Id}", id);
        var repartidor = await FindRepartidorAsync(id);

        return _mapper.Map<RepartidorDTO>(repartidor);
    }

    public async Task<PagedResult<RepartidorDTO>> ListRepartidoresAsync(RepartidorInputDTO input)
    {
        _logger.LogInformation("Consultando paquetes pagina: {page} tamaño: {size}", input.PageNumber, input.PageSize);
        var paquetes = await _repartidorRepository.ListAsync(input.PageNumber, input.PageSize);

        var output = _mapper.Map<List<RepartidorDTO>>(paquetes);

        return PagedResult<RepartidorDTO>.Create(output, input.PageNumber, input.PageSize);
    }

    public async Task UpdateRepartidorAsync(Guid id, CreateRepartidorDTO input)
    {
        var repartidor = await FindRepartidorAsync(id);

        var repartidorToUpdate = new Repartidor(id, input.Nombre);
        await _repartidorRepository.UpdateAsync(repartidorToUpdate);
    }

    public async Task DeleteRepartidorAsync(Guid id)
    {
        var paquete = await FindRepartidorAsync(id);
        await _repartidorRepository.DeleteAsync(id);
    }

    public async Task CreateRepartidorAsync(CreateRepartidorDTO input)
    {
        await CheckDuplicateNameAsync(input.Nombre);

        var repartidor = new Repartidor(input.Nombre);
        await _repartidorRepository.CreateAsync(repartidor);
    }

    private async Task<Repartidor> FindRepartidorAsync(Guid id)
    {
        var repartidor = await _repartidorRepository.GetAsync(id)
            ?? throw new KeyNotFoundException("No encontrado");
        return repartidor;
    }

    private async Task CheckDuplicateNameAsync(string codigo)
    {
        var duplicateCode = await _repartidorRepository.ExistNombreAsync(codigo);
        if (duplicateCode)
            throw new ArgumentException($"El nombre del repartidor '{codigo}' ya existe en el sistema.");
    }
}
