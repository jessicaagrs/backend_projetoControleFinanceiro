namespace APIControleFinanceiro.Models.Receitas
{
    public interface IReceitaServico
    {
        public Task<List<Receita>> ObterTodos();
        public Task<Receita> Adicionar(Receita receita);
        public void Remover(string receitaId);
        public Task<Receita> Atualizar(Receita receita);
    }
}
