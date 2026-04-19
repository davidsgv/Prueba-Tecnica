using FlashLogistic.Application.DTOs;
using FlashLogistic.Application.DTOs.Paquetes;

namespace FlashLogistic.Application.Interfaces;

public interface IPaqueteService
{
    public Task<PagedResult<PaqueteDTO>> ListPaquetesAsync(PaquetesInputDTO input);
    public Task<PaqueteDTO> GetPaqueteAsync(Guid id);
    public Task UpdatePaqueteAsync(Guid id, UpdatePaqueteDTO paquete);
    public Task DeletePaqueteAsync(Guid id);
    public Task CreatePaqueteAsync(CreatePaqueteDTO paquete);
    public Task AsignarRepartidorAsync(Guid paqueteId, Guid repartidorId);
}
