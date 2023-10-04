
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
        private readonly IUsuarioServico _usuarioServico;
        private readonly ILoginServico _loginServico;

        public LoginController(IUsuarioServico usuarioServico, ILoginServico loginServico)
        {
            _usuarioServico = usuarioServico;
            _loginServico = loginServico;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Login dados)
        {
            try
            {
                var usuarioExiste = await _loginServico.VerificarUsuarioExiste(dados.EmailLogin);
                var senhaValida = await _loginServico.VerificarSenhaLogin(dados);

                if (!senhaValida)
                    throw new Exception("Senha inválida");

                var token = AutenticacaoServico.GerarToken(usuarioExiste);

                return Ok(new
                {
                    Token = token,
                    Usuario = usuarioExiste
                });
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


    }
}
