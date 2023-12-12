namespace APIControleFinanceiro.Domain.Models.CategoriasDespesas
{
    public interface ICategoriaDespesaServico
    {
        public Task<List<CategoriaDespesa>> ObterTodos();
        public Task<CategoriaDespesa> Adicionar(CategoriaDespesa categoriaDespesa);
        public void Remover(string categoriaDespesaId);
        public Task<CategoriaDespesa> Atualizar(CategoriaDespesa categoriaDespesa);
    }
}
