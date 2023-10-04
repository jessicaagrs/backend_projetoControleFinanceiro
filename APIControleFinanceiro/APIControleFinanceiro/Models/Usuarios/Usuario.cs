using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace APIControleFinanceiro.Models.Usuarios
{
    public class Usuario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public bool Habilitado { get; set; } = false;
    }

}
