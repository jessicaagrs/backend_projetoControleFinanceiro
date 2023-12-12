namespace APIControleFinanceiro.Controllers.v1.Models
{
    public class UsuarioControllerModel
    {
        public string? Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public IFormFile? Foto { get; set; }
        public string NumeroCartaoCredito { get; set; } = string.Empty;
        public DateTime ValidadeCartaoCredito { get; set; } = DateTime.MinValue;
        public string BandeiraCartaoCredito { get; set; } = string.Empty;
        public string BancoCartaoCredito { get; set; } = string.Empty;
    }
}
