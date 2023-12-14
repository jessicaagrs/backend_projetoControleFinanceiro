using APIControleFinanceiro.Application.Helper;
using APIControleFinanceiro.Domain.Models.CategoriasDespesas;
using APIControleFinanceiro.Domain.Models.Usuarios;

namespace APIControleFinanceiro.Application.Servicos.CategoriasDespesas
{
    public class CategoriaDespesaServico : ICategoriaDespesaServico
    {
        private readonly ICategoriaDespesaRepositorio _categoriaDespesaRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public CategoriaDespesaServico(ICategoriaDespesaRepositorio categoriaDespesaRepositorio, IUsuarioRepositorio usuarioRepositorio)
        {
            _categoriaDespesaRepositorio = categoriaDespesaRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
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

        private async Task VerificarUsuario(string usuarioId, int linha)
        {
            var usuarios = await _usuarioRepositorio.GetUsuariosAsync();
            var existeUsuario = usuarios.Any(c => c.Id == usuarioId);

            if (!existeUsuario)
                throw new Exception($"O usuário informado não existe. Linha {linha}");
        }

        public async Task<int> AdicionarLista(IFormFile file)
        {
            if (file == null)
                throw new Exception("Selecione um arquivo excel para importar os dados.");

            var excel = new ExcelHelper<CategoriaDespesa>(file);
            var categorias = excel.GetValues();

            for (int i = 0; i < categorias.Count; i++)
            {
                var linha = i + 1;
                await VerificarUsuario(categorias[i].UsuarioId, linha);
            }

            var qtdItens = await _categoriaDespesaRepositorio.CreateListCategoriasDespesasAsync(categorias);

            return qtdItens;
        }
    }
}
