using APIControleFinanceiro.Models.Database;
using APIControleFinanceiro.Models.Usuarios;
using APIControleFinanceiro.Repositorios.Database;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace APIControleFinanceiro.Repositorios.Usuarios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly IMongoCollection<Usuario> _usuarioCollection;

        public UsuarioRepositorio(IOptions<UsuariosDatabaseSettings> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);

            _usuarioCollection = mongoDatabase.GetCollection<Usuario>(settings.Value.CollectionName);
        }

        public async Task<List<Usuario>> GetUsuariosAsync() =>
            await _usuarioCollection.Find(x => true).ToListAsync();

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
