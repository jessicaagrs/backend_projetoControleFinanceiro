using APIControleFinanceiro.Domain.Models.CategoriasDespesas;
using FluentValidation;

namespace APIControleFinanceiro.Domain.Validacoes.CategoriasDespesas
{
    public class CategoriaDespesaValidacao : AbstractValidator<CategoriaDespesa>
    {
        public CategoriaDespesaValidacao()
        {
            RuleFor(c => c.Descricao)
                .NotEmpty()
                .WithMessage("A descrição é obrigatória");
        }
    }
}
