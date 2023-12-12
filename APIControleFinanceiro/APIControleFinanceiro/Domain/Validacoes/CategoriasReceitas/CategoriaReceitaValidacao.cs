using APIControleFinanceiro.Domain.Models.CategoriasReceitas;
using FluentValidation;

namespace APIControleFinanceiro.Domain.Validacoes.CategoriasReceitas
{
    public class CategoriaReceitaValidacao : AbstractValidator<CategoriaReceita>
    {
        public CategoriaReceitaValidacao()
        {
            RuleFor(c => c.Descricao)
                .NotEmpty()
                .WithMessage("A descrição é obrigatória");
        }
    }
}
