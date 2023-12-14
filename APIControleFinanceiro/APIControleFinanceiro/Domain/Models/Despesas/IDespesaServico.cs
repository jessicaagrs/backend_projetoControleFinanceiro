
namespace APIControleFinanceiro.Domain.Models.Despesas
{
    public interface IDespesaServico
    {
        public Task<List<Despesa>> ObterTodos(int numeroPagina, int quantidadePorPagina);
        public Task<Despesa> Adicionar(Despesa receita);
        public void Remover(string receitaId);
        public Task<Despesa> Atualizar(Despesa receita);
        public Task<int> AdicionarLista(IFormFile file);
    }
}
