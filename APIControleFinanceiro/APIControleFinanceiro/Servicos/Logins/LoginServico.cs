using APIControleFinanceiro.Models.Logins;
using APIControleFinanceiro.Models.Usuarios;

namespace APIControleFinanceiro.Servicos.Logins
{
    public class LoginServico : ILoginServico
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        public LoginServico(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public async Task<Usuario> VerificarUsuarioExiste(string email)
        {
            var usuario = await _usuarioRepositorio.GetUsuarioEmailAsync(email);

            if (usuario == null)
                throw new Exception("Não foi encontrado usuário correspondente ao e-mail, revise os dados");

            return usuario;
        }

        public async Task<bool> VerificarSenhaLogin(Login Dados)
        {
            var encontrarUsuario = await _usuarioRepositorio.GetUsuarioEmailAsync(Dados.EmailLogin);
            var senhaValida =  BCrypt.Net.BCrypt.Verify(Dados.SenhaLogin, encontrarUsuario.Senha);

            return senhaValida;
        }
    }
}
