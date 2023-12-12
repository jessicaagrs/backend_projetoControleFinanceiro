namespace APIControleFinanceiro.Domain.Models.Receitas
{
    public interface IReceitaServico
    {
        public Task<List<Receita>> ObterTodos(int numeroPagina, int quantidadePorPagina);
        public Task<Receita> Adicionar(Receita receita);
        public void Remover(string receitaId);
        public Task<Receita> Atualizar(Receita receita);
    }
}
