using APIUsuarios.Application.DTOs;
using APIUsuarios.Application.Interfaces;
using APIUsuarios.Domain.Entities;

namespace APIUsuarios.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _repo;

    public UsuarioService(IUsuarioRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<UsuarioReadDto>> ListarAsync(CancellationToken ct)
    {
        var usuarios = await _repo.GetAllAsync(ct);

        return usuarios.Select(u => new UsuarioReadDto(
            u.Id,
            u.Nome,
            u.Email,
            u.DataNascimento,
            u.Telefone,
            u.Ativo,
            u.DataCriacao
        ));
    }

    public async Task<UsuarioReadDto?> ObterAsync(int id, CancellationToken ct)
    {
        var usuario = await _repo.GetByIdAsync(id, ct);
        if (usuario is null) return null;

        return new UsuarioReadDto(
            usuario.Id,
            usuario.Nome,
            usuario.Email,
            usuario.DataNascimento,
            usuario.Telefone,
            usuario.Ativo,
            usuario.DataCriacao
        );
    }

    public async Task<bool> EmailJaCadastradoAsync(string email, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        return await _repo.EmailExistsAsync(email.ToLower(), ct);
    }

    public async Task<UsuarioReadDto> CriarAsync(UsuarioCreateDto dto, CancellationToken ct)
    {
        // Normalizar email
        var emailNormalizado = dto.Email.Trim().ToLower();

        // Checar email duplicado
        if (await _repo.EmailExistsAsync(emailNormalizado, ct))
            throw new InvalidOperationException("Email já cadastrado.");

        // Checar idade (regra de negócio)
        if (dto.DataNascimento.AddYears(18) > DateTime.Today)
            throw new InvalidOperationException("Usuário deve ter pelo menos 18 anos.");

        // Montar a entidade
        var usuario = new Usuario
        {
            Nome = dto.Nome.Trim(),
            Email = emailNormalizado,
            Senha = dto.Senha, // ideal: hash aqui (ex: BCrypt). Ver observação no README.
            DataNascimento = dto.DataNascimento,
            Telefone = string.IsNullOrWhiteSpace(dto.Telefone) ? null : dto.Telefone.Trim(),
            Ativo = true,
            DataCriacao = DateTime.UtcNow
        };

        await _repo.AddAsync(usuario, ct);
        await _repo.SaveChangesAsync(ct);

        return new UsuarioReadDto(
            usuario.Id,
            usuario.Nome,
            usuario.Email,
            usuario.DataNascimento,
            usuario.Telefone,
            usuario.Ativo,
            usuario.DataCriacao
        );
    }

    public async Task<UsuarioReadDto> AtualizarAsync(int id, UsuarioUpdateDto dto, CancellationToken ct)
    {
        var usuario = await _repo.GetByIdAsync(id, ct);
        if (usuario is null)
            throw new KeyNotFoundException("Usuário não encontrado.");

        var emailNormalizado = dto.Email.Trim().ToLower();

        // Se alterou o email, verificar duplicidade
        if (!string.Equals(usuario.Email, emailNormalizado, StringComparison.OrdinalIgnoreCase)
            && await _repo.EmailExistsAsync(emailNormalizado, ct))
        {
            throw new InvalidOperationException("Email já cadastrado.");
        }

        // Checar idade mínima
        if (dto.DataNascimento.AddYears(18) > DateTime.Today)
            throw new InvalidOperationException("Usuário deve ter pelo menos 18 anos.");

        // Atualizar campos
        usuario.Nome = dto.Nome.Trim();
        usuario.Email = emailNormalizado;
        usuario.DataNascimento = dto.DataNascimento;
        usuario.Telefone = string.IsNullOrWhiteSpace(dto.Telefone) ? null : dto.Telefone.Trim();
        usuario.Ativo = dto.Ativo;
        usuario.DataAtualizacao = DateTime.UtcNow;

        await _repo.UpdateAsync(usuario, ct);
        await _repo.SaveChangesAsync(ct);

        return new UsuarioReadDto(
            usuario.Id,
            usuario.Nome,
            usuario.Email,
            usuario.DataNascimento,
            usuario.Telefone,
            usuario.Ativo,
            usuario.DataCriacao
        );
    }

    public async Task<bool> RemoverAsync(int id, CancellationToken ct)
    {
        var usuario = await _repo.GetByIdAsync(id, ct);
        if (usuario is null) return false;

        // Soft delete
        usuario.Ativo = false;
        usuario.DataAtualizacao = DateTime.UtcNow;

        await _repo.UpdateAsync(usuario, ct);
        await _repo.SaveChangesAsync(ct);

        return true;
    }
}