using APIControleFinanceiro.Domain.Models;

namespace APIControleFinanceiro.Domain.Models.Usuarios
{
    public interface IUsuarioServico
    {
        public Task<List<Usuario>> ObterTodos();
        public Task<Usuario> ObterPorId(string id);
        public Task<Usuario> ObterPorEmail(string id);
        public Task<Usuario> Adicionar(Usuario usuario);
        public void Remover(string usuarioId);
        public Task<Usuario> Atualizar(Usuario usuario);
        public Usuario SalvarFotoStorage(IFormFile foto, Usuario usuario);
    }
}
