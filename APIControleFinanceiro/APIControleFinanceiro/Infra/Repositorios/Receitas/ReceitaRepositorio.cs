using APIControleFinanceiro.Domain.Models.Receitas;
using MongoDB.Driver;
using APIControleFinanceiro.Infra.Repositorios.Database;

namespace APIControleFinanceiro.Infra.Repositorios.Receitas
{
    public class ReceitaRepositorio : IReceitaRepositorio
    {
        private readonly IMongoCollection<Receita> _receitaCollection;

        public ReceitaRepositorio(MongoDBContext mongoDbContext)
        {
            _receitaCollection = mongoDbContext.Receitas;
        }

        public async Task<List<Receita>> GetReceitasPaginacaoAsync(int numeroPagina, int quantidadePorPagina) =>
            await _receitaCollection.Find(x => true).Skip(numeroPagina * quantidadePorPagina).Limit(quantidadePorPagina).ToListAsync();

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
