using FlashLogistic.Domain.Entities;
using FlashLogistic.Domain.Enums;

namespace FlashLogistic.Domain.Repositories;

public interface IPaqueteRepository
{
    public Task<bool> ExistCodeAsync(string code);
    public Task<Paquete?> GetAsync(Guid id);
    public Task<List<Paquete>> ListAsync(EstadoPaquete? estadoPaquete, int page = 1, int size = 10);
    public Task UpdateAsync(Paquete paquete);
    public Task DeleteAsync(Guid id);
    public Task CreateAsync(Paquete paquete);
}
