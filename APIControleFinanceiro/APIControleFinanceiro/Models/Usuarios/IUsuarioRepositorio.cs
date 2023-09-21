namespace APIControleFinanceiro.Models.Usuarios
{
    public interface IUsuarioRepositorio
    {
        public Task<List<Usuario>> GetUsuariosAsync();
        public Task<Usuario> GetUsuarioEmailAsync(string email);
        public Task<Usuario> CreateUsuarioAsync(Usuario usuario);
        public Task DeleteUsuarioAsync(string usuarioId);
        public Task<Usuario> UpdateUsuarioAsync(string usuarioId, Usuario usuario);
    }
}
