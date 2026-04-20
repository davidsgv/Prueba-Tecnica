using FlashLogistic.Domain.Entities;
using FlashLogistic.Domain.Enums;

namespace FlashLogistic.Domain.Repositories;

public interface IRepartidorRepository
{
    public Task<bool> ExistNombreAsync(string nombre);
    public Task<Repartidor?> GetAsync(Guid id);
    public Task<int> GetPackageCountAsync(Guid idRepartidor, EstadoPaquete estado);
    public Task<List<Repartidor>> ListAsync(int page = 1, int size = 10);
    public Task UpdateAsync(Repartidor paquete);
    public Task DeleteAsync(Guid id);
    public Task CreateAsync(Repartidor paquete);
}
