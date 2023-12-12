using APIControleFinanceiro.Domain.Models.CategoriasDespesas;
using MongoDB.Driver;
using APIControleFinanceiro.Infra.Repositorios.Database;

namespace APIControleFinanceiro.Infra.Repositorios.CategoriasDespesas
{
    public class CategoriaDespesaRepositorio : ICategoriaDespesaRepositorio
    {
        private readonly IMongoCollection<CategoriaDespesa> _categoriaDespesaCollection;

        public CategoriaDespesaRepositorio(MongoDBContext mongoDbContext)
        {
            _categoriaDespesaCollection = mongoDbContext.CategoriaDespesas;
        }

        public async Task<List<CategoriaDespesa>> GetCategoriasDespesasAsync() =>
            await _categoriaDespesaCollection.Find(x => true).ToListAsync();

        public async Task<CategoriaDespesa> CreateCategoriaDespesaAsync(CategoriaDespesa categoriaDespesa)
        {
            await _categoriaDespesaCollection.InsertOneAsync(categoriaDespesa);

            return categoriaDespesa;
        }
        public async Task DeleteCategoriaDespesaAsync(string categoriaDespesaId)
        {
            await _categoriaDespesaCollection.DeleteOneAsync(x => x.Id == categoriaDespesaId);
        }

        public async Task<CategoriaDespesa> UpdateCategoriaDespesaAsync(CategoriaDespesa categoriaDespesa)
        {
            await _categoriaDespesaCollection.ReplaceOneAsync(x => x.Id == categoriaDespesa.Id, categoriaDespesa);
            return categoriaDespesa;
        }

    }
}
