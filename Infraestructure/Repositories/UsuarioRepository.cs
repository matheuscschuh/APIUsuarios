using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using APIUsuarios.Application.Interfaces;
using APIUsuarios.Domain.Entities;
using APIUsuarios.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace APIUsuarios.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Usuario>> GetAllAsync(CancellationToken ct)
    {
        // normalmente filtramos por Ativo==true em Listar (a decisão pode ser na service)
        return await _context.Usuarios
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<Usuario?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Id == id, ct);
    }

    public async Task<Usuario?> GetByEmailAsync(string email, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(email)) return null;
        var normalized = email.Trim().ToLowerInvariant();
        return await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == normalized, ct);
    }

    public async Task AddAsync(Usuario usuario, CancellationToken ct)
    {
        await _context.Usuarios.AddAsync(usuario, ct);
    }

    public Task UpdateAsync(Usuario usuario, CancellationToken ct)
    {
        _context.Usuarios.Update(usuario);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(Usuario usuario, CancellationToken ct)
    {
        // Implementação padrão: remoção física.
        _context.Usuarios.Remove(usuario);
        return Task.CompletedTask;
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        var normalized = email.Trim().ToLowerInvariant();
        return await _context.Usuarios
            .AsNoTracking()
            .AnyAsync(u => u.Email == normalized, ct);
    }

    public Task<int> SaveChangesAsync(CancellationToken ct)
    {
        // aqui não precisa de async/await, apenas retornamos a Task do EF
        return _context.SaveChangesAsync(ct);
    }
}
