using APIControleFinanceiro.Models.CategoriasDespesas;
using FluentValidation;

namespace APIControleFinanceiro.Validacoes.CategoriasDespesas
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
