using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace APIControleFinanceiro.Domain.Models.Autenticacoes
{
    public class TokenRevogado
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public string? Id { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public DateTime Data {  get; set; } = DateTime.Now;
    }
}
