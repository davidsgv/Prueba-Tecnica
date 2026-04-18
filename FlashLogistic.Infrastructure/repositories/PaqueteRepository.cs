using FlashLogistic.Domain.Entities;
using FlashLogistic.Domain.Enums;
using FlashLogistic.Domain.Repositories;
using FlashLogistic.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlashLogistic.Infrastructure.repositories;

internal class PaqueteRepository : IPaqueteRepository
{
    private readonly ApplicationDbContext _context;

    public PaqueteRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<bool> ExistCodeAsync(string code)
    {
        return await _context.Paquetes.AnyAsync(x => x.Codigo == code);
    }

    public async Task<Paquete?> GetAsync(Guid id)
    {
        return await _context.Paquetes
            .AsNoTracking().Include(x => x.Repartidor)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Paquete>> ListAsync(EstadoPaquete? estadoPaquete, int page = 1, int size = 10)
    {
        var query = _context.Paquetes
            .AsNoTracking();

        if (estadoPaquete.HasValue)
        {
            query = query.Where(p => p.Estado == estadoPaquete.Value);
        }

        return await query
            .Include(x => x.Repartidor)
            .OrderByDescending(p => p.Codigo)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();
    }

    public async Task CreateAsync(Paquete paquete)
    {
        await _context.Paquetes.AddAsync(paquete);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Paquete paquete)
    {
        _context.Paquetes.Update(paquete);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var paquete = await _context.Paquetes.FindAsync(id);

        if (paquete != null)
        {
            _context.Paquetes.Remove(paquete);
            await _context.SaveChangesAsync();
        }
    }
}
