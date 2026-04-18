using AutoMapper;
using FlashLogistic.Application.DTOs;
using FlashLogistic.Application.DTOs.Paquetes;
using FlashLogistic.Application.Interfaces;
using FlashLogistic.Domain.Entities;
using FlashLogistic.Domain.Enums;
using FlashLogistic.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlashLogistic.Application.Services;

internal class PaqueteService : IPaqueteService
{
    private readonly ILogger<PaqueteService> _logger;
    private readonly IPaqueteRepository _paqueteRepository;
    private readonly IRepartidorRepository _repartidorRepository;
    private readonly IMapper _mapper;

    public PaqueteService(
        IPaqueteRepository paqueteRepository,
        IRepartidorRepository repartidorRepository,
        IMapper mapper,
        ILogger<PaqueteService> logger)
    {
        _paqueteRepository = paqueteRepository;
        _repartidorRepository = repartidorRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PaqueteDTO> GetPaqueteAsync(Guid id)
    {
        _logger.LogInformation("Consultando el paquete con ID: {Id}", id);
        var paquete = await FindPaqueteAsync(id);

        return _mapper.Map<PaqueteDTO>(paquete);
    }

    public async Task<PagedResult<PaqueteDTO>> ListPaquetesAsync(PaquetesInputDTO input)
    {
        _logger.LogInformation("Consultando paquetes pagina: {page} tamaño: {size}", input.PageNumber, input.PageSize);
        var paquetes = await _paqueteRepository.ListAsync(input.EstadoPaquete, input.PageNumber, input.PageSize);

        var output = _mapper.Map<List<PaqueteDTO>>(paquetes);

        return PagedResult<PaqueteDTO>.Create(output, input.PageNumber, input.PageSize);
    }

    public async Task UpdatePaqueteAsync(Guid id, UpdatePaqueteDTO input)
    {
        var paquete = await FindPaqueteAsync(id);

        //No se puede modificar un paquete ya entregado
        if (paquete.Estado == EstadoPaquete.Entregado)
            throw new ArgumentException("No se puede modificar un paquete entregado");

        //un paquete no puede cambiar prioridad si ya fue entregado
        paquete.AsignarPrioridad(input.Prioridad);

        var paqueteToUpdate = new Paquete(
            id,
            input.Descripcion,
            input.Peso,
            paquete.Codigo, //El codigo no se debe actualizar
            input.Estado,
            input.Prioridad,
            paquete.RepartidorId //El repartidor no se debe actualizar
        );

        await _paqueteRepository.UpdateAsync(paqueteToUpdate);
    }

    public async Task DeletePaqueteAsync(Guid id)
    {
        var paquete = await FindPaqueteAsync(id);
        await _paqueteRepository.DeleteAsync(id);
    }

    public async Task CreatePaqueteAsync(CreatePaqueteDTO input)
    {
        await CheckDuplicateCodeAsync(input.Codigo);

        var paquete = new Paquete(input.Descripcion, input.Peso, input.Codigo, input.Prioridad);
        await _paqueteRepository.CreateAsync(paquete);
    }

    public async Task AsignarRepartidorAsync(Guid paqueteId, Guid repartidorId)
    {
        var paquete = await FindPaqueteAsync(paqueteId);
        var repartidor = await _repartidorRepository.GetWithPackageAsync(repartidorId)
                     ?? throw new KeyNotFoundException("Repartidor no encontrado");

        if (repartidor.Paquetes.Count >= 3)
            throw new ArgumentException("El repartidor ya tiene 3 paquetes asignados");

        paquete.AsignarRepartidor(repartidor);
        await _paqueteRepository.UpdateAsync(paquete);
    }

    private async Task<Paquete> FindPaqueteAsync(Guid id)
    {
        var paquete = await _paqueteRepository.GetAsync(id)
            ?? throw new KeyNotFoundException("No encontrado");
        return paquete;
    }

    private async Task CheckDuplicateCodeAsync(string codigo)
    {
        var duplicateCode = await _paqueteRepository.ExistCodeAsync(codigo);
        if (duplicateCode)
            throw new ArgumentException($"El código de paquete '{codigo}' ya existe en el sistema.");
    }
}
