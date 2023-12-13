using APIControleFinanceiro.Application.Helper;
using APIControleFinanceiro.Domain.Models.CategoriasDespesas;

namespace APIControleFinanceiro.Application.Servicos.CategoriasDespesas
{
    public class CategoriaDespesaServico : ICategoriaDespesaServico
    {
        private readonly ICategoriaDespesaRepositorio _categoriaDespesaRepositorio;

        public CategoriaDespesaServico(ICategoriaDespesaRepositorio categoriaDespesaRepositorio)
        {
            _categoriaDespesaRepositorio = categoriaDespesaRepositorio;
        }

        public Task<List<CategoriaDespesa>> ObterTodos()
        {
            return _categoriaDespesaRepositorio.GetCategoriasDespesasAsync();
        }

        public async Task<CategoriaDespesa> Adicionar(CategoriaDespesa categoriaDespesa)
        {
            if (categoriaDespesa == null)
                throw new Exception("Dados inválidos.");

            return await _categoriaDespesaRepositorio.CreateCategoriaDespesaAsync(categoriaDespesa);
        }

        public void Remover(string categoriaDespesaId)
        {
            if (string.IsNullOrEmpty(categoriaDespesaId))
                throw new Exception("O Id da categoria de despesa é inválido");

            _categoriaDespesaRepositorio.DeleteCategoriaDespesaAsync(categoriaDespesaId);
        }

        public async Task<CategoriaDespesa> Atualizar(CategoriaDespesa categoriaDespesa)
        {
            if (categoriaDespesa == null)
                throw new Exception("Dados inválidos.");

            var categorias = await _categoriaDespesaRepositorio.GetCategoriasDespesasAsync();
            var existeCategoria = categorias.FirstOrDefault(c => c.Id == categoriaDespesa.Id);

            if (existeCategoria == null)
                throw new Exception("A categoria informada não existe");

            var atualizacao = await _categoriaDespesaRepositorio.UpdateCategoriaDespesaAsync(categoriaDespesa);

            return atualizacao;
        }

        public async Task<int> AdicionarLista(IFormFile file)
        {
            if (file == null)
                throw new Exception("Selecione um arquivo excel para importar os dados.");

            var excel = new ExcelHelper<CategoriaDespesa>(file);
            var categorias = excel.GetValues();

            var qtdItens = await _categoriaDespesaRepositorio.CreateListCategoriasDespesasAsync(categorias);

            return qtdItens;
        }
    }
}
