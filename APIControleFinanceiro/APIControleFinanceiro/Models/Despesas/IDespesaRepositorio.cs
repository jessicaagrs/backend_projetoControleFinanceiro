
namespace APIControleFinanceiro.Models.Despesas
{
    public interface IDespesaRepositorio
    {
        public Task<List<Despesa>> GetReceitasAsync();
        public Task<Despesa> CreateReceitaAsync(Despesa receita);
        public Task DeleteReceitaAsync(string receitaId);
        public Task<Despesa> UpdateReceitaAsync(Despesa receita);
    }
}
