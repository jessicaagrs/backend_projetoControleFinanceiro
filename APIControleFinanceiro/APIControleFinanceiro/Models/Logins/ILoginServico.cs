using APIControleFinanceiro.Models.Usuarios;

namespace APIControleFinanceiro.Models.Logins
{
    public interface ILoginServico
    {
        public Task<Usuario> VerificarUsuarioExiste(string email);
        public Task<bool> VerificarSenhaLogin(Login dados);
    }
}
