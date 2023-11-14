
using APIControleFinanceiro.Models.Logins;
using APIControleFinanceiro.Models.Usuarios;
using APIControleFinanceiro.Servicos.AutenticacaoServico;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Usuarios
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginServico _loginServico;
        private readonly IUsuarioServico _usuarioServico;

        public LoginController(ILoginServico loginServico, IUsuarioServico usuarioServico)
        {
            _loginServico = loginServico;
            _usuarioServico = usuarioServico;
        }

        [HttpPost()]
        [ApiVersion("1.0")]
        public async Task<IActionResult> Login([FromBody] Login dados)
        {
            try
            {
                var usuarioExiste = await _loginServico.VerificarUsuarioExiste(dados.EmailLogin);

                if (usuarioExiste.Habilitado)
                    throw new Exception("Usuário já está logado.");

                var senhaValida = await _loginServico.VerificarSenhaLogin(dados);

                if (!senhaValida)
                    throw new Exception("Senha inválida");

                var token = AutenticacaoServico.GerarToken(usuarioExiste);

                usuarioExiste.TokenAcesso = token;
                usuarioExiste.Habilitado = true;
                await _usuarioServico.Atualizar(usuarioExiste);

                return Ok(new
                {
                    Mensagem = "Usuário Logado com sucesso.",
                    Token = token,
                }); ;
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "Erro ao autenticar o usuário",
                    Error = ex.Message
                });
            }
        }

        [HttpPost()]
        [Authorize]
        [Route("/Logout/")]
        [ApiVersion("1.0")]
        public async Task<IActionResult> Logout(string email)
        {
            try
            {
                var usuarioExiste = await _loginServico.VerificarUsuarioExiste(email);
                usuarioExiste.Habilitado = false;
                await _usuarioServico.Atualizar(usuarioExiste);

                return Ok(new
                {
                    Mensagem = "Usuário foi desconectado"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "Erro ao desconectar o usuário",
                    Error = ex.Message
                });
            }
        }

        [HttpGet()]
        [Authorize]
        [Route("/VerificarToken/")]
        [ApiVersion("1.0")]
        public async Task<IActionResult> VerificarToken(string token)
        {
            try
            {
                var tokenValido = AutenticacaoServico.TokenEhValido(token);
                var usuarios = await _usuarioServico.ObterTodos();
                var usuarioLogado = usuarios?.FirstOrDefault(u => u.TokenAcesso == token);
                var usuarioHabilitado = tokenValido && usuarioLogado.Habilitado;


                if (!usuarioHabilitado)
                    throw new Exception("Token expirado ou usuário efetuou logout.");

                return Ok(new
                {
                    PermissaoAcesso = usuarioHabilitado
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "Erro ao obter o token",
                    Error = ex.Message
                });
            }
        }
    }
}
