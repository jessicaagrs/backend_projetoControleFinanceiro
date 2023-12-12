 using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace APIControleFinanceiro.Domain.Models.Usuarios
{
    public class Usuario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public string? TokenAcesso { get; set; } = string.Empty;
        public string? CaminhoFoto { get; set; } = string.Empty;
        public string NumeroCartaoCredito {  get; set; } = string.Empty;
        public DateTime ValidadeCartaoCredito { get; set; } = DateTime.MinValue;
        public string BandeiraCartaoCredito { get; set; } = string.Empty;
        public string BancoCartaoCredito { get; set; } = string.Empty;
    }
}
