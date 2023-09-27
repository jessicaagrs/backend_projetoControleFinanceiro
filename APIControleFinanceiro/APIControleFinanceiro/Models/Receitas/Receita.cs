using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.RegularExpressions;

namespace APIControleFinanceiro.Models.Receitas
{
    public class Receita
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; } = decimal.MinValue;

        [BsonRepresentation(BsonType.ObjectId)]
        public string UsuarioId { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoriaId { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public void Validar(Receita receita)
        {
            if (string.IsNullOrEmpty(receita.Descricao))
                throw new Exception("A descrição é obrigatória");

            if (receita.Valor == decimal.Zero)
                throw new Exception("O valor da receita é obrigatório");

            if (!Regex.IsMatch(receita.CategoriaId, "^[0-9a-fA-F]{24}$"))
                throw new Exception("Categoria obrigatória.");
        }
    }
}
