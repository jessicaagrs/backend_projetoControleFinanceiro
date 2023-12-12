using APIControleFinanceiro.Domain.Models.Usuarios;
using APIControleFinanceiro.Infra.Repositorios.Database;
using MongoDB.Driver;

namespace APIControleFinanceiro.Infra.Repositorios.Usuarios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly IMongoCollection<Usuario> _usuarioCollection;

        public UsuarioRepositorio(MongoDBContext mongoDbContext)
        {
            _usuarioCollection = mongoDbContext.Usuarios;
        }

        public async Task<List<Usuario>> GetUsuariosAsync() =>
            await _usuarioCollection.Find(x => true).ToListAsync();

        public async Task<Usuario> GetUsuarioPorIdAsync(string id) =>
            await _usuarioCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<Usuario> GetUsuarioEmailAsync(string email) =>
           await _usuarioCollection.Find(x => x.Email == email).FirstOrDefaultAsync();

        public async Task<Usuario> CreateUsuarioAsync(Usuario usuario)
        {
            await _usuarioCollection.InsertOneAsync(usuario);

            return usuario;
        }
        public async Task DeleteUsuarioAsync(string usuarioId)
        {
            await _usuarioCollection.DeleteOneAsync(x => x.Id == usuarioId);
        }

        public async Task<Usuario> UpdateUsuarioAsync(Usuario usuario)
        {
            await _usuarioCollection.ReplaceOneAsync(x => x.Id == usuario.Id, usuario);
            return usuario;
        }

    }
}
