using FlashLogistic.Domain.Entities;
using FlashLogistic.Domain.Repositories;
using FlashLogistic.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlashLogistic.Infrastructure.repositories;

internal class RepartidorRepository : IRepartidorRepository
{
    private readonly ApplicationDbContext _context;

    public RepartidorRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Repartidor?> GetAsync(Guid id)
    {
        return await _context.Repartidor
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Repartidor?> GetWithPackageAsync(Guid id)
    {
        return await _context.Repartidor
            .AsNoTracking()
            .Include(x => x.Paquetes)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Repartidor>> ListAsync(int page = 1, int size = 10)
    {
        var query = _context.Repartidor
            .AsNoTracking();

        return await query
            .OrderByDescending(p => p.Nombre)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();
    }

    public async Task CreateAsync(Repartidor repartidor)
    {
        await _context.Repartidor.AddAsync(repartidor);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Repartidor repartidor)
    {
        _context.Repartidor.Update(repartidor);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var repartidor = await _context.Repartidor.FindAsync(id);

        if (repartidor != null)
        {
            _context.Repartidor.Remove(repartidor);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistNombreAsync(string nombre)
    {
        return await _context.Repartidor.AnyAsync(x => x.Nombre == nombre);
    }
}
