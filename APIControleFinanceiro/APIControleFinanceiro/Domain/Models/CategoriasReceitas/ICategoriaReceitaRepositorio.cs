
namespace APIControleFinanceiro.Domain.Models.CategoriasReceitas
{
    public interface ICategoriaReceitaRepositorio
    {
        public Task<List<CategoriaReceita>> GetCategoriasReceitasAsync();
        public Task<CategoriaReceita> CreateCategoriaReceitaAsync(CategoriaReceita categoriaReceita);
        public Task DeleteCategoriaReceitaAsync(string categoriaReceitaId);
        public Task<CategoriaReceita> UpdateCategoriaReceitaAsync(CategoriaReceita categoriaReceita);
    }
}
