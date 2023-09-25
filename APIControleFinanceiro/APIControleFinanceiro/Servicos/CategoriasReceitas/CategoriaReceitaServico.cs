using APIControleFinanceiro.Models.CategoriasReceitas;
using APIControleFinanceiro.Repositorios.CategoriasReceitas;
using System.Text.RegularExpressions;
using static APIControleFinanceiro.Models.Database.DatabaseSettings;

namespace APIControleFinanceiro.Repositorios.CategoriasReceitas
{
    public class CategoriaReceitaServico : ICategoriaReceitaServico
    {
        private readonly ICategoriaReceitaRepositorio _categoriaReceitaRepositorio;

        public CategoriaReceitaServico(ICategoriaReceitaRepositorio categoriaReceitaRepositorio)
        {
            _categoriaReceitaRepositorio = categoriaReceitaRepositorio;
        }

        public Task<List<CategoriaReceita>> ObterTodos()
        {
            return _categoriaReceitaRepositorio.GetCategoriasReceitasAsync();
        }

        public async Task<CategoriaReceita> Adicionar(CategoriaReceita categoriaReceita)
        {
            if (categoriaReceita == null)
                throw new Exception("Dados inválidos.");

            return await _categoriaReceitaRepositorio.CreateCategoriaReceitaAsync(categoriaReceita);
        }

        public void Remover(string categoriaReceitaId)
        {
            if (string.IsNullOrEmpty(categoriaReceitaId))
                throw new Exception("O Id da categoria de receita é inválido");

            _categoriaReceitaRepositorio.DeleteCategoriaReceitaAsync(categoriaReceitaId);
        }

        public async Task<CategoriaReceita> Atualizar(CategoriaReceita categoriaReceita)
        {
            if (categoriaReceita == null)
                throw new Exception("Dados inválidos.");

            var categorias = await _categoriaReceitaRepositorio.GetCategoriasReceitasAsync();
            var existeCategoria = categorias.FirstOrDefault(c => c.Id == categoriaReceita.Id);

            if (existeCategoria == null)
                throw new Exception("A categoria informada não existe");

            var atualizacao = await _categoriaReceitaRepositorio.UpdateCategoriaReceitaAsync(categoriaReceita);

            return atualizacao;
        }
       
    }
}
