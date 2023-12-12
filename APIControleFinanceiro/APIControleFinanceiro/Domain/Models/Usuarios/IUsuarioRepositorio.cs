namespace APIControleFinanceiro.Domain.Models.Usuarios
{
    public interface IUsuarioRepositorio
    {
        public Task<List<Usuario>> GetUsuariosAsync();
        public Task<Usuario> GetUsuarioPorIdAsync(string id);
        public Task<Usuario> GetUsuarioEmailAsync(string email);
        public Task<Usuario> CreateUsuarioAsync(Usuario usuario);
        public Task DeleteUsuarioAsync(string usuarioId);
        public Task<Usuario> UpdateUsuarioAsync(Usuario usuario);
    }
}
