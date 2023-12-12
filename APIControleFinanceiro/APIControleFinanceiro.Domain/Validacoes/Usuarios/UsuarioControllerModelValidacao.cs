using APIControleFinanceiro.Controllers.Models;
using FluentValidation;

namespace APIControleFinanceiro.Validacoes.Usuarios
{
    public class UsuarioControllerModelValidacao : AbstractValidator<UsuarioControllerModel>
    {
        public UsuarioControllerModelValidacao()
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
        }
    }
}
