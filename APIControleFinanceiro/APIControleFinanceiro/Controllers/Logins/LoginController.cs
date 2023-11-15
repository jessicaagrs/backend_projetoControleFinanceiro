
using APIControleFinanceiro.Models.Autenticacoes;
using APIControleFinanceiro.Models.Logins;
using APIControleFinanceiro.Models.Usuarios;
using APIControleFinanceiro.Servicos.Autenticacoes;
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
        private readonly IRevogarTokensServico _revogarTokensServico;

        public LoginController(ILoginServico loginServico, IUsuarioServico usuarioServico, IRevogarTokensServico revogarTokensServico)
        {
            _loginServico = loginServico;
            _usuarioServico = usuarioServico;
            _revogarTokensServico = revogarTokensServico;
        }

        [HttpPost()]
        [ApiVersion("1.0")]
        public async Task<IActionResult> Login([FromBody] Login dados)
        {
            try
            {
                var usuario = await _loginServico.VerificarCredenciaisUsuario(dados);

                var token = AutenticacaoServico.GerarToken(usuario);

                usuario.TokenAcesso = token;

                await _usuarioServico.Atualizar(usuario);

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
        public async Task<IActionResult> Logout(TokenRevogado tokenRevogado)
        {
            try
            {
                var usuarioExiste = await _loginServico.VerificarEmailLogout(tokenRevogado.Email);
                _revogarTokensServico.RevogarTokens(tokenRevogado);

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
    }
}
