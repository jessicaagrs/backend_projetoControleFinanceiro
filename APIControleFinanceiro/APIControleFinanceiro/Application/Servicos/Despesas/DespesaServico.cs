using APIControleFinanceiro.Application.Helper;
using APIControleFinanceiro.Domain.Models.CategoriasDespesas;
using APIControleFinanceiro.Domain.Models.CategoriasReceitas;
using APIControleFinanceiro.Domain.Models.Despesas;
using APIControleFinanceiro.Domain.Models.Receitas;
using APIControleFinanceiro.Domain.Models.Usuarios;

namespace APIControleFinanceiro.Application.Servicos.Despesas
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

        private async Task VerificarCategoria(string categoriaId, int linha = 0)
        {
            var categorias = await _categoriaDespesaRepositorio.GetCategoriasDespesasAsync();
            var existeCategoria = categorias.Any(c => c.Id == categoriaId);

            var msg = linha == 0 ? "A categoria informada não existe" : $"A categoria informada não existe. Linha {linha}";

            if (!existeCategoria)
                throw new Exception("A categoria informada não existe");
        }

        private async Task VerificarUsuario(string usuarioId, int linha = 0)
        {
            var usuarios = await _usuarioRepositorio.GetUsuariosAsync();
            var existeUsuario = usuarios.Any(c => c.Id == usuarioId);

            var msg = linha == 0 ? "O usuário informado não existe" : $"O usuário informado não existe. Linha {linha}";

            if (!existeUsuario)
                throw new Exception("O usuário informado não existe");
        }

        public async Task<int> AdicionarLista(IFormFile file)
        {
            if (file == null)
                throw new Exception("Selecione um arquivo excel para importar os dados.");

            var excel = new ExcelHelper<Despesa>(file);
            var despesas = excel.GetValues();

            for (int i = 0; i < despesas.Count; i++)
            {
                var linha = i + 1;
                await VerificarCategoria(despesas[i].CategoriaId, linha);
                await VerificarUsuario(despesas[i].UsuarioId, linha);
            }

            var qtdItens = await _despesaRepositorio.CreateListDespesasAsync(despesas);

            return qtdItens;
        }
    }
}
