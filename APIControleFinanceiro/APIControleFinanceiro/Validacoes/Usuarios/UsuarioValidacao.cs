using APIControleFinanceiro.Models.Usuarios;
using FluentValidation;

namespace APIControleFinanceiro.Validacoes.Usuarios
{
    public class UsuarioValidacao : AbstractValidator<Usuario>
    {
        public UsuarioValidacao()
        {
            RuleFor(u => u.Nome)
                .NotEmpty()
                .Length(3, 100).WithMessage("Nome deve conter de 3 a 100 caracteres");
        }
    }
}
