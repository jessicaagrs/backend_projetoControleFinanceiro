using APIControleFinanceiro.Domain.Models.Despesas;
using MongoDB.Driver;
using APIControleFinanceiro.Infra.Repositorios.Database;

namespace APIControleFinanceiro.Infra.Repositorios.Despesas
{
    public class DespesaRepositorio : IDespesaRepositorio
    {
        private readonly IMongoCollection<Despesa> _despesaCollection;

        public DespesaRepositorio(MongoDBContext mongoDbContext)
        {
            _despesaCollection = mongoDbContext.Despesas;
        }

        public async Task<List<Despesa>> GetDespesasPaginacaoAsync(int numeroPagina, int quantidadePorPagina) =>
            await _despesaCollection.Find(x => true).Skip(numeroPagina * quantidadePorPagina).Limit(quantidadePorPagina).ToListAsync();

        public async Task<List<Despesa>> GetDespesasAsync() =>
           await _despesaCollection.Find(x => true).ToListAsync();

        public async Task<Despesa> CreateDespesaAsync(Despesa despesa)
        {
            await _despesaCollection.InsertOneAsync(despesa);

            return despesa;
        }
        public async Task DeleteDespesaAsync(string despesaId)
        {
            await _despesaCollection.DeleteOneAsync(x => x.Id == despesaId);
        }

        public async Task<Despesa> UpdateDespesaAsync(Despesa despesa)
        {
            await _despesaCollection.ReplaceOneAsync(x => x.Id == despesa.Id, despesa);
            return despesa;
        }

    }
}
