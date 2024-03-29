﻿using APIControleFinanceiro.Domain.Models.Receitas;
using FluentValidation;

namespace APIControleFinanceiro.Domain.Validacoes.Receitas
{
    public class ReceitaValidacao : AbstractValidator<Receita>
    {
        public ReceitaValidacao()
        {
            RuleFor(r => r.Descricao)
                 .NotEmpty()
                 .WithMessage("A descrição é obrigatória");

            RuleFor(r => r.Origem)
                .NotEmpty()
                .WithMessage("A origem da receita é obrigatória");

            RuleFor(r => r.Valor)
                .Must(valor => valor != decimal.Zero)
                .WithMessage("O valor da receita não pode ser zero");

            RuleFor(r => r.Data)
                .Must(data => data != DateTime.MinValue)
                .WithMessage("A data da receita é obrigatória");

            RuleFor(r => r.CategoriaId)
                .NotEmpty()
                .WithMessage("Necessário vincular usuário a receita");

        }
    }
}
