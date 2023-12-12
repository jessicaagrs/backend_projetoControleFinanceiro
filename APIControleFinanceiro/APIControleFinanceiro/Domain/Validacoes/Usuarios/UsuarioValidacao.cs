using APIControleFinanceiro.Domain.Models.Usuarios;
using FluentValidation;

namespace APIControleFinanceiro.Domain.Validacoes.Usuarios
{
    public class UsuarioValidacao : AbstractValidator<Usuario>
    {
        public UsuarioValidacao()
        {
            RuleFor(u => u.Nome)
                .NotEmpty()
                .Length(3, 100).WithMessage("Nome deve conter de 3 a 100 caracteres");

            RuleFor(u => u.Email)
                .NotEmpty()
                .WithMessage("O e-mail é obrigatório");

            RuleFor(u => u.Senha)
                .NotEmpty()
                .Length(5, 10).WithMessage("A senha deve conter de 5 a 10 caracteres");

            RuleFor(u => u.NumeroCartaoCredito)
                .NotEmpty()
                .Length(16).WithMessage("O número do cartão de crédito deve conter 16 dígitos");

            RuleFor(r => r.ValidadeCartaoCredito)
                .NotEmpty()
                .WithMessage("Informe uma data para validade do cartão de crédito");

            RuleFor(r => r.BandeiraCartaoCredito)
                .NotEmpty()
                .WithMessage("Informe uma bandeira válida para Cartão de Crédito");

            RuleFor(r => r.BancoCartaoCredito)
                .NotEmpty()
                .WithMessage("Informe um banco válido para Cartão de Crédito");
        }
    }
}
