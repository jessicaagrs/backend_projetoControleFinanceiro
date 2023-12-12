using APIControleFinanceiro.Domain.Models.Usuarios;

namespace APIControleFinanceiro.Domain.Models.Logins
{
    public interface ILoginServico
    {
        public Task<Usuario> VerificarCredenciaisUsuario(Login dados);
        public Task<Usuario> VerificarEmailLogout(string email);
    }
}
