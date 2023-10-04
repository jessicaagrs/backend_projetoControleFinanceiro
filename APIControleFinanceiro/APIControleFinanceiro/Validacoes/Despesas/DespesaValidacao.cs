using APIControleFinanceiro.Models.Despesas;
using FluentValidation;

namespace APIControleFinanceiro.Validacoes.Despesas
{
    public class DespesaValidacao : AbstractValidator<Despesa>
    {
        public DespesaValidacao()
        {
            RuleFor(d => d.Descricao)
                .NotEmpty()
                .WithMessage("A descrição é obrigatória");

            RuleFor(d => d.Valor)
                .Must(valor => valor != decimal.Zero)
                .WithMessage("O valor da despesa não pode ser zero");

            RuleFor(d => d.Data)
                .Must(data => data != DateTime.MinValue)
                .WithMessage("A data da despesa é obrigatória");

        }
    }
}
