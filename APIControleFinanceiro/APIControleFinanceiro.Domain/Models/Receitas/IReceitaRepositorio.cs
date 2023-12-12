
namespace APIControleFinanceiro.Models.Receitas
{
    public interface IReceitaRepositorio
    {
        public Task<List<Receita>> GetReceitasPaginacaoAsync(int numeroPagina, int quantidadePorPagina);
        public Task<List<Receita>> GetReceitasAsync();
        public Task<Receita> CreateReceitaAsync(Receita receita);
        public Task DeleteReceitaAsync(string receitaId);
        public Task<Receita> UpdateReceitaAsync(Receita receita);
    }
}
