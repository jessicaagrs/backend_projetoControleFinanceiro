namespace APIControleFinanceiro.Models.Usuarios
{
    public interface IUsuarioServico
    {
        public Task<List<Usuario>> ObterTodos();
        public Task<Usuario> Adicionar(Usuario usuario);
        public void Remover(string usuarioId);
        public Task<Usuario> Atualizar(string usuarioId, Usuario usuario);
        public Task LogarUsuario(Login dados);
    }
}
