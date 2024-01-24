using APIControleFinanceiro.Application.Servicos.Autenticacoes;
using APIControleFinanceiro.Domain.Models.Autenticacoes;
using APIControleFinanceiro.Domain.Models.Logins;
using APIControleFinanceiro.Domain.Models.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIControleFinanceiro.Controllers.v1.Logins
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

        // POST api/Login
        /// <summary>
        /// Efetua login na aplicação.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     POST /Login
        ///     {
        ///         "emailLogin": "string",
        ///         "SenhaLogin": "string"
        ///     }
        ///
        /// </remarks>
        /// <param name="value"></param>
        /// <returns>Mensagem de sucesso e token para autorização</returns>
        /// <response code="200">Retorna mensagem e token</response>
        /// <response code="400">Se houver erro no login</response>
        /// <response code="401">Não autorizado</response>
        [HttpPost()]
        [ApiVersion("1.0")]
        public async Task<IActionResult> Login([FromBody] Login dados)
        {
            try
            {
                var usuario = await _loginServico.VerificarCredenciaisUsuario(dados);

                var token = AutenticacaoServico.GerarToken(usuario);

                usuario.TokenAcesso = token;
                usuario.Logado = true;

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

        // POST api/Logout
        /// <summary>
        /// Efetua logout na aplicação.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     POST /Logout
        ///     {
        ///         "token": "string",
        ///         "email": "string"
        ///     }
        ///
        /// </remarks>
        /// <param name="value"></param>
        /// <returns>Mensagem de logout efetuado</returns>
        /// <response code="200">Retorna mensagem de logout</response>
        /// <response code="400">Se houver erro no logout</response>
        /// <response code="401">Não autorizado</response>
        [HttpPost()]
        [Authorize]
        [Route("/Logout/")]
        [ApiVersion("1.0")]
        public async Task<IActionResult> Logout(TokenRevogado tokenRevogado)
        {
            try
            {
                var usuarioExiste = await _loginServico.VerificarEmailLogout(tokenRevogado.Email);

                if (usuarioExiste != null)
                {
                    usuarioExiste.Logado = false;
                    await _usuarioServico.Atualizar(usuarioExiste); 
                }

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
