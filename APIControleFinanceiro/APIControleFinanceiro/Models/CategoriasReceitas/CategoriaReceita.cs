using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace APIControleFinanceiro.Models.CategoriasReceitas
{
    public class CategoriaReceita
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
    }
}
