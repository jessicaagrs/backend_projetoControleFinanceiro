using APIControleFinanceiro.Controllers.Models;

namespace APIControleFinanceiro.Models.Usuarios
{
    public interface IUsuarioServico
    {
        public Task<List<Usuario>> ObterTodos();
        public Task<Usuario> ObterPorId(string id);
        public Task<Usuario> Adicionar(Usuario usuario);
        public void Remover(string usuarioId);
        public Task<Usuario> Atualizar(Usuario usuario);
        public Usuario SalvarFotoStorage(IFormFile foto, Usuario usuario);
    }
}
