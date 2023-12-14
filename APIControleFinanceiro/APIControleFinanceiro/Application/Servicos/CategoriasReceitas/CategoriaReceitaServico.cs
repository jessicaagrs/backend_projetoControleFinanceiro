using APIControleFinanceiro.Application.Helper;
using APIControleFinanceiro.Domain.Models.CategoriasDespesas;
using APIControleFinanceiro.Domain.Models.CategoriasReceitas;
using APIControleFinanceiro.Domain.Models.Receitas;
using APIControleFinanceiro.Domain.Models.Usuarios;

namespace APIControleFinanceiro.Application.Servicos.CategoriasReceitas
{
    public class CategoriaReceitaServico : ICategoriaReceitaServico
    {
        private readonly ICategoriaReceitaRepositorio _categoriaReceitaRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public CategoriaReceitaServico(ICategoriaReceitaRepositorio categoriaReceitaRepositorio, IUsuarioRepositorio usuarioRepositorio)
        {
            _categoriaReceitaRepositorio = categoriaReceitaRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
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

            var excel = new ExcelHelper<CategoriaReceita>(file);
            var categorias = excel.GetValues();

            for (int i = 0; i < categorias.Count; i++)
            {
                var linha = i + 1;
                await VerificarUsuario(categorias[i].UsuarioId, linha);
            }

            var qtdItens = await _categoriaReceitaRepositorio.CreateListCategoriasReceitasAsync(categorias);

            return qtdItens;
        }
    }
}
