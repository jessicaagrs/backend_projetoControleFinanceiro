﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace APIControleFinanceiro.Domain.Models.Despesas
{
    public class Despesa
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string Local { get; set; } = string.Empty;
        public decimal Valor { get; set; } = decimal.MinValue;
        [BsonRepresentation(BsonType.ObjectId)]
        public string UsuarioId { get; set; } = string.Empty;
        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoriaId { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime Data { get; set; } = DateTime.MinValue;
    }
}
