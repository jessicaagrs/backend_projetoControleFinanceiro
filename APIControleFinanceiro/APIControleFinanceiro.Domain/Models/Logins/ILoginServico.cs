using APIControleFinanceiro.Models.Usuarios;

namespace APIControleFinanceiro.Models.Logins
{
    public interface ILoginServico
    {
        public Task<Usuario> VerificarCredenciaisUsuario(Login dados);
        public Task<Usuario> VerificarEmailLogout(string email);
    }
}
