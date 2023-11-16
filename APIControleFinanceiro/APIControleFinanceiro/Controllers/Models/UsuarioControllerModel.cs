namespace APIControleFinanceiro.Controllers.Models
{
    public class UsuarioControllerModel
    {
        public string? Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public IFormFile? Foto { get; set; }
    }
}
