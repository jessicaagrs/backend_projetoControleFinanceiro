using APIControleFinanceiro.Models.Receitas;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using APIControleFinanceiro.Repositorios.Database;

namespace APIControleFinanceiro.Repositorios.Receitas
{
    public class ReceitaRepositorio : IReceitaRepositorio
    {
        private readonly IMongoCollection<Receita> _receitaCollection;

        public ReceitaRepositorio(IOptions<ReceitaDatabaseSettings> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);

            _receitaCollection = mongoDatabase.GetCollection<Receita>(settings.Value.CollectionName);
        }

        public async Task<List<Receita>> GetReceitasAsync() =>
            await _receitaCollection.Find(x => true).ToListAsync();

        public async Task<Receita> CreateReceitaAsync(Receita receita)
        {
            await _receitaCollection.InsertOneAsync(receita);

            return receita;
        }
        public async Task DeleteReceitaAsync(string receitaId)
        {
            await _receitaCollection.DeleteOneAsync(x => x.Id == receitaId);
        }

        public async Task<Receita> UpdateReceitaAsync(Receita receita)
        {
            await _receitaCollection.ReplaceOneAsync(x => x.Id == receita.Id, receita);
            return receita;
        }

    }
}
