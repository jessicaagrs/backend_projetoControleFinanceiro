
namespace APIControleFinanceiro.Models.Despesas
{
    public interface IDespesaServico
    {
        public Task<List<Despesa>> ObterTodos(int numeroPagina, int quantidadePorPagina);
        public Task<Despesa> Adicionar(Despesa receita);
        public void Remover(string receitaId);
        public Task<Despesa> Atualizar(Despesa receita);
    }
}
