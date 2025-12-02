using System.ComponentModel.DataAnnotations;

namespace APIUsuarios.Domain.Entities;

public class Usuario
{   
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Senha { get; set; } = string.Empty;
    [Required]
    public DateTime DataNascimento { get; set; }
    [Required]
    public string? Telefone { get; set; }
    [Required]
    public bool Ativo { get; set; } = true;
    
    public DateTime DataCriacao { get; set; }
    
    public DateTime? DataAtualizacao { get; set; }
}