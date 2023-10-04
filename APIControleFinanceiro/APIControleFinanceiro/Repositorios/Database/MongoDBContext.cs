﻿using APIControleFinanceiro.Models.CategoriasDespesas;
using APIControleFinanceiro.Models.CategoriasReceitas;
using APIControleFinanceiro.Models.Database;
using APIControleFinanceiro.Models.Despesas;
using APIControleFinanceiro.Models.Receitas;
using APIControleFinanceiro.Models.Usuarios;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace APIControleFinanceiro.Repositorios.Database
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
        public IMongoCollection<IdentityRole> Roles => _database.GetCollection<IdentityRole>("roles");
        public IMongoCollection<Receita> Receitas => _database.GetCollection<Receita>("receitas");
        public IMongoCollection<Despesa> Despesas => _database.GetCollection<Despesa>("despesas");
        public IMongoCollection<CategoriaDespesa> CategoriaDespesas => _database.GetCollection<CategoriaDespesa>("categoriasDespesas");
        public IMongoCollection<CategoriaReceita> CategoriaReceitas => _database.GetCollection<CategoriaReceita>("categoriasReceitas");
    }
}