using APIControleFinanceiro.Models.CategoriasDespesas;
using APIControleFinanceiro.Models.Despesas;
using APIControleFinanceiro.Models.Usuarios;

namespace APIControleFinanceiro.Repositorios.Despesas
{
    public class DespesaServico : IDespesaServico
    {
        private readonly IDespesaRepositorio _despesaRepositorio;
        private readonly ICategoriaDespesaRepositorio _categoriaDespesaRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public DespesaServico(IDespesaRepositorio despesaRepositorio, ICategoriaDespesaRepositorio categoriaDespesaRepositorio, IUsuarioRepositorio usuarioRepositorio)
        {
            _despesaRepositorio = despesaRepositorio;
            _categoriaDespesaRepositorio = categoriaDespesaRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
        }

        public Task<List<Despesa>> ObterTodos(int numeroPagina, int quantidadePorPagina)
        {
            return _despesaRepositorio.GetDespesasPaginacaoAsync(numeroPagina, quantidadePorPagina);
        }

        public async Task<Despesa> Adicionar(Despesa despesa)
        {
            if (despesa == null)
                throw new Exception("Dados inválidos.");

            await VerificarCategoria(despesa.CategoriaId);
            await VerificarUsuario(despesa.UsuarioId);
            return await _despesaRepositorio.CreateDespesaAsync(despesa);
        }

        public void Remover(string despesaId)
        {
            if (string.IsNullOrEmpty(despesaId))
                throw new Exception("O Id da despesa é inválido");

            _despesaRepositorio.DeleteDespesaAsync(despesaId);
        }

        public async Task<Despesa> Atualizar(Despesa despesa)
        {
            if (despesa == null)
                throw new Exception("Dados inválidos.");

            var despesas = await _despesaRepositorio.GetDespesasAsync();
            var existeDespesa = despesas.FirstOrDefault(c => c.Id == despesa.Id);

            if (existeDespesa == null)
                throw new Exception("A despesa informada não existe");

            var atualizacao = await _despesaRepositorio.UpdateDespesaAsync(despesa);

            return atualizacao;
        }

        private async Task VerificarCategoria(string categoriaId)
        {
            var categorias = await _categoriaDespesaRepositorio.GetCategoriasDespesasAsync();
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
