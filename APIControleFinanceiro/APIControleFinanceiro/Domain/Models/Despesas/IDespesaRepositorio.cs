namespace APIControleFinanceiro.Domain.Models.Despesas
{
    public interface IDespesaRepositorio
    {
        public Task<List<Despesa>> GetDespesasPaginacaoAsync(int numeroPagina, int quantidadePorPagina);
        public Task<List<Despesa>> GetDespesasAsync();
        public Task<Despesa> CreateDespesaAsync(Despesa receita);
        public Task DeleteDespesaAsync(string receitaId);
        public Task<Despesa> UpdateDespesaAsync(Despesa receita);
        public Task<int> CreateListDespesasAsync(List<Despesa> despesas);
    }
}
