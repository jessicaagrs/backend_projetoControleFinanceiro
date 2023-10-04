using APIControleFinanceiro.Models.Despesas;
using MongoDB.Driver;
using APIControleFinanceiro.Repositorios.Database;

namespace APIControleFinanceiro.Repositorios.Despesas
{
    public class DespesaRepositorio : IDespesaRepositorio
    {
        private readonly IMongoCollection<Despesa> _despesaCollection;

        public DespesaRepositorio(MongoDBContext mongoDbContext)
        {
            _despesaCollection = mongoDbContext.Despesas;
        }

        public async Task<List<Despesa>> GetReceitasAsync() =>
            await _despesaCollection.Find(x => true).ToListAsync();

        public async Task<Despesa> CreateReceitaAsync(Despesa despesa)
        {
            await _despesaCollection.InsertOneAsync(despesa);

            return despesa;
        }
        public async Task DeleteReceitaAsync(string despesaId)
        {
            await _despesaCollection.DeleteOneAsync(x => x.Id == despesaId);
        }

        public async Task<Despesa> UpdateReceitaAsync(Despesa despesa)
        {
            await _despesaCollection.ReplaceOneAsync(x => x.Id == despesa.Id, despesa);
            return despesa;
        }

    }
}
