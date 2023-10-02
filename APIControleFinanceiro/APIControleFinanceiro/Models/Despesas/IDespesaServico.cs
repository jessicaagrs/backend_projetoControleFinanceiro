
namespace APIControleFinanceiro.Models.Despesas
{
    public interface IDespesaServico
    {
        public Task<List<Despesa>> ObterTodos();
        public Task<Despesa> Adicionar(Despesa receita);
        public void Remover(string receitaId);
        public Task<Despesa> Atualizar(Despesa receita);
    }
}
