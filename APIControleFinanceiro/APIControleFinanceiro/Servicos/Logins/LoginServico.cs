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

        public async Task<Usuario> VerificarCredenciaisUsuario(Login dados)
        {
            var usuario = await _usuarioRepositorio.GetUsuarioEmailAsync(dados.EmailLogin);

            if (usuario == null)
                throw new Exception("Não foi encontrado usuário correspondente ao e-mail, revise os dados");

            var senhaValida = VerificarSenhaLogin(dados, usuario);

            if (!senhaValida)
                throw new Exception("Senha inválida");

            return usuario;
        }

        private static bool VerificarSenhaLogin(Login Dados, Usuario usuario)
        {
            var senhaValida =  BCrypt.Net.BCrypt.Verify(Dados.SenhaLogin, usuario.Senha);

            return senhaValida;
        }

        public async Task<Usuario> VerificarEmailLogout(string email)
        {
            var usuario = await _usuarioRepositorio.GetUsuarioEmailAsync(email);

            if (usuario == null)
                throw new Exception("Não foi encontrado usuário correspondente ao e-mail, revise os dados");

            return usuario;
        }


    }
}
