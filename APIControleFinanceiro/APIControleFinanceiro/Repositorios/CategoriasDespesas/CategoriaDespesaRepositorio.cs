using APIControleFinanceiro.Models.CategoriasDespesas;
using MongoDB.Driver;
using APIControleFinanceiro.Repositorios.Database;

namespace APIControleFinanceiro.Repositorios.CategoriasDespesas
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
