using APIControleFinanceiro.Models.CategoriasReceitas;
using MongoDB.Driver;
using APIControleFinanceiro.Repositorios.Database;

namespace APIControleFinanceiro.Repositorios.CategoriasReceitas
{
    public class CategoriaReceitaRepositorio : ICategoriaReceitaRepositorio
    {
        private readonly IMongoCollection<CategoriaReceita> _categoriaReceitaCollection;

        public CategoriaReceitaRepositorio(MongoDBContext mongoDbContext)
        {
            _categoriaReceitaCollection = mongoDbContext.CategoriaReceitas;
        }

        public async Task<List<CategoriaReceita>> GetCategoriasReceitasAsync() =>
            await _categoriaReceitaCollection.Find(x => true).ToListAsync();

        public async Task<CategoriaReceita> CreateCategoriaReceitaAsync(CategoriaReceita categoriaReceita)
        {
            await _categoriaReceitaCollection.InsertOneAsync(categoriaReceita);

            return categoriaReceita;
        }
        public async Task DeleteCategoriaReceitaAsync(string categoriaId)
        {
            await _categoriaReceitaCollection.DeleteOneAsync(x => x.Id == categoriaId);
        }

        public async Task<CategoriaReceita> UpdateCategoriaReceitaAsync(CategoriaReceita categoriaReceita)
        {
            await _categoriaReceitaCollection.ReplaceOneAsync(x => x.Id == categoriaReceita.Id, categoriaReceita);
            return categoriaReceita;
        }

    }
}
