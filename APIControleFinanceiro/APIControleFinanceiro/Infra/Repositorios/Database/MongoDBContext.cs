using APIControleFinanceiro.Domain.Models.Autenticacoes;
using APIControleFinanceiro.Domain.Models.CategoriasDespesas;
using APIControleFinanceiro.Domain.Models.CategoriasReceitas;
using APIControleFinanceiro.Domain.Models.Database;
using APIControleFinanceiro.Domain.Models.Despesas;
using APIControleFinanceiro.Domain.Models.Receitas;
using APIControleFinanceiro.Domain.Models.Usuarios;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace APIControleFinanceiro.Infra.Repositorios.Database
{
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;

        public MongoDBContext(IOptions<DatabaseSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<Usuario> Usuarios => _database.GetCollection<Usuario>("usuarios");
        public IMongoCollection<Receita> Receitas => _database.GetCollection<Receita>("receitas");
        public IMongoCollection<Despesa> Despesas => _database.GetCollection<Despesa>("despesas");
        public IMongoCollection<CategoriaDespesa> CategoriaDespesas => _database.GetCollection<CategoriaDespesa>("categoriasDespesas");
        public IMongoCollection<CategoriaReceita> CategoriaReceitas => _database.GetCollection<CategoriaReceita>("categoriasReceitas");
        public IMongoCollection<TokenRevogado> TokensRevogados => _database.GetCollection<TokenRevogado>("tokensRevogados");
    }
}
