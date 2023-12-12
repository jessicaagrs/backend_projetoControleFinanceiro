using APIControleFinanceiro.Domain.Models.Autenticacoes;
using APIControleFinanceiro.Infra.Repositorios.Database;
using MongoDB.Driver;

namespace APIControleFinanceiro.Infra.Repositorios.Autenticacoes
{
    public class RevogarTokensRepositorio : IRevogarTokensRepositorio
    {
        private readonly IMongoCollection<TokenRevogado> _tokenRevogadoCollection;

        public RevogarTokensRepositorio(MongoDBContext mongoDbContext)
        {
            _tokenRevogadoCollection = mongoDbContext.TokensRevogados;
        }
        public async void AdicionarTokenRevogado(TokenRevogado tokenRevogado)
        {
            await _tokenRevogadoCollection.InsertOneAsync(tokenRevogado);
        }

        public bool VerificarToken(string token)
        {
            return _tokenRevogadoCollection.Find(x => x.Token == token).Any();
        }

    }
}
