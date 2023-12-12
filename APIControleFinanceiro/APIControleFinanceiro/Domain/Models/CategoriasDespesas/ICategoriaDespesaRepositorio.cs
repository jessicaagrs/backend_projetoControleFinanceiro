namespace APIControleFinanceiro.Domain.Models.CategoriasDespesas
{
    public interface ICategoriaDespesaRepositorio
    {
        public Task<List<CategoriaDespesa>> GetCategoriasDespesasAsync();
        public Task<CategoriaDespesa> CreateCategoriaDespesaAsync(CategoriaDespesa categoriaDespesa);
        public Task DeleteCategoriaDespesaAsync(string categoriaDespesaId);
        public Task<CategoriaDespesa> UpdateCategoriaDespesaAsync(CategoriaDespesa categoriaDespesa);
    }
}
