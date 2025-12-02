using System;
using FluentValidation;
using APIUsuarios.Application.DTOs;

namespace APIUsuarios.Application.Validators;

public class UsuarioCreateDtoValidator : AbstractValidator<UsuarioCreateDto>
{
    public UsuarioCreateDtoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MinimumLength(3).WithMessage("O nome deve ter no mínimo 3 caracteres.")
            .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O email é obrigatório.")
            .EmailAddress().WithMessage("O email informado é inválido.");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres.");

        RuleFor(x => x.DataNascimento)
            .NotEmpty().WithMessage("A data de nascimento é obrigatória.")
            .Must(BeAtLeast18).WithMessage("Usuário deve ter pelo menos 18 anos.")
            .LessThan(DateTime.UtcNow).WithMessage("A data de nascimento deve ser no passado.");

        RuleFor(x => x.Telefone)
            .MaximumLength(20).WithMessage("O telefone deve ter no máximo 20 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.Telefone));
    }

    private bool BeAtLeast18(DateTime dataNascimento)
    {
        var today = DateTime.UtcNow.Date;
        var age = today.Year - dataNascimento.Year;
        if (dataNascimento.Date > today.AddYears(-age)) age--;
        return age >= 18;
    }
}


