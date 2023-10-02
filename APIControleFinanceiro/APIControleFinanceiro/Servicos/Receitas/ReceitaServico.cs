using APIControleFinanceiro.Models.CategoriasReceitas;
using APIControleFinanceiro.Models.Receitas;
using APIControleFinanceiro.Models.Usuarios;

namespace APIControleFinanceiro.Repositorios.Receitas
{
    public class ReceitaServico : IReceitaServico
    {
        private readonly IReceitaRepositorio _receitaRepositorio;
        private readonly ICategoriaReceitaRepositorio _categoriaReceitaRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public ReceitaServico(IReceitaRepositorio receitaRepositorio, ICategoriaReceitaRepositorio categoriaReceitaRepositorio, IUsuarioRepositorio usuarioRepositorio)
        {
            _receitaRepositorio = receitaRepositorio;
            _categoriaReceitaRepositorio = categoriaReceitaRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
        }

        public Task<List<Receita>> ObterTodos()
        {
            return _receitaRepositorio.GetReceitasAsync();
        }

        public async Task<Receita> Adicionar(Receita receita)
        {
            if (receita == null)
                throw new Exception("Dados inválidos.");

            await VerificarCategoria(receita.CategoriaId);
            await VerificarUsuario(receita.UsuarioId);
            return await _receitaRepositorio.CreateReceitaAsync(receita);
        }

        public void Remover(string receitaId)
        {
            if (string.IsNullOrEmpty(receitaId))
                throw new Exception("O Id da receita é inválido");

            _receitaRepositorio.DeleteReceitaAsync(receitaId);
        }

        public async Task<Receita> Atualizar(Receita receita)
        {
            if (receita == null)
                throw new Exception("Dados inválidos.");

            var receitas = await _receitaRepositorio.GetReceitasAsync();
            var existeReceita = receitas.FirstOrDefault(c => c.Id == receita.Id);

            if (existeReceita == null)
                throw new Exception("A receita informada não existe");

            var atualizacao = await _receitaRepositorio.UpdateReceitaAsync(receita);

            return atualizacao;
        }

        private async Task VerificarCategoria(string categoriaId)
        {
            var categorias = await _categoriaReceitaRepositorio.GetCategoriasReceitasAsync();
            var existeCategoria = categorias.Any(c => c.Id == categoriaId);

            if (!existeCategoria)
                throw new Exception("A categoria informada não existe");
        }

        private async Task VerificarUsuario(string usuarioId)
        {
            var usuarios = await _usuarioRepositorio.GetUsuariosAsync();
            var existeUsuario = usuarios.Any(c => c.Id == usuarioId);

            if (!existeUsuario)
                throw new Exception("O usuário informado não existe");
        }

    }
}
