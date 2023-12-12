
namespace APIControleFinanceiro.Domain.Models.CategoriasReceitas
{
    public interface ICategoriaReceitaServico
    {
        public Task<List<CategoriaReceita>> ObterTodos();
        public Task<CategoriaReceita> Adicionar(CategoriaReceita categoriaReceita);
        public void Remover(string categoriaReceitaId);
        public Task<CategoriaReceita> Atualizar(CategoriaReceita categoriaReceita);
    }
}
