﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace APIControleFinanceiro.Domain.Models.CategoriasDespesas
{
    public class CategoriaDespesa
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        [BsonRepresentation(BsonType.ObjectId)]
        public string UsuarioId { get; set; } = string.Empty;
    }
}
