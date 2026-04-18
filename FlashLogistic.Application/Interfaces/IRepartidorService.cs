using FlashLogistic.Application.DTOs;
using FlashLogistic.Application.DTOs.Repartidor;

namespace FlashLogistic.Application.Interfaces;

public interface IRepartidorService
{
    public Task<PagedResult<RepartidorDTO>> ListRepartidoresAsync(RepartidorInputDTO input);
    public Task<RepartidorDTO> GetRepartidorAsync(Guid id);
    public Task UpdateRepartidorAsync(Guid id, CreateRepartidorDTO repartidor);
    public Task DeleteRepartidorAsync(Guid id);
    public Task CreateRepartidorAsync(CreateRepartidorDTO repartidor);
}
