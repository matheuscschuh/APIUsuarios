using APIUsuarios.Application.DTOs;
using APIUsuarios.Application.Interfaces;
using APIUsuarios.Application.Services;
using APIUsuarios.Application.Validators;
using APIUsuarios.Infrastructure.Persistence;
using APIUsuarios.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DbContext (SQLite)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=app.db"));

// DI - Repository & Service
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

// FluentValidation - registra validadores e habilita validação automática
builder.Services.AddValidatorsFromAssemblyContaining<UsuarioCreateDtoValidator>();
builder.Services.AddFluentValidationAutoValidation(); // requer FluentValidation.AspNetCore

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();


app.UseHttpsRedirection();

// Endpoints
app.MapGet("/usuarios", async (IUsuarioService service, CancellationToken ct) =>
{
    var usuarios = await service.ListarAsync(ct);
    return Results.Ok(usuarios);
});

app.MapGet("/usuarios/{id:int}", async (int id, IUsuarioService service, CancellationToken ct) =>
{
    var usuario = await service.ObterAsync(id, ct);
    return usuario is null ? Results.NotFound() : Results.Ok(usuario);
});

app.MapPost("/usuarios", async (UsuarioCreateDto dto, IUsuarioService service, IValidator<UsuarioCreateDto> validator, CancellationToken ct) =>
{
    var validation = await validator.ValidateAsync(dto, ct);
    if (!validation.IsValid) return Results.BadRequest(validation.Errors);

    try
    {
        var created = await service.CriarAsync(dto, ct);
        return Results.Created($"/usuarios/{created.Id}", created);
    }
    catch (InvalidOperationException ex) when (ex.Message.Contains("Email"))
    {
        return Results.Conflict(new { message = ex.Message });
    }
    catch (Exception ex)
    {
        // log ex se quiser
        return Results.StatusCode(500);
    }
});

app.MapPut("/usuarios/{id:int}", async (int id, UsuarioUpdateDto dto, IUsuarioService service, IValidator<UsuarioUpdateDto> validator, CancellationToken ct) =>
{
    var validation = await validator.ValidateAsync(dto, ct);
    if (!validation.IsValid) return Results.BadRequest(validation.Errors);

    try
    {
        var updated = await service.AtualizarAsync(id, dto, ct);
        return Results.Ok(updated);
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound();
    }
    catch (InvalidOperationException ex) when (ex.Message.Contains("Email"))
    {
        return Results.Conflict(new { message = ex.Message });
    }
    catch (Exception)
    {
        return Results.StatusCode(500);
    }
});

app.MapDelete("/usuarios/{id:int}", async (int id, IUsuarioService service, CancellationToken ct) =>
{
    var ok = await service.RemoverAsync(id, ct);
    return ok ? Results.NoContent() : Results.NotFound();
});

app.Run();
