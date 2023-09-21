
using APIControleFinanceiro.Models.Usuarios;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Usuarios
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        private IUsuarioServico _usuarioServico;

        public UsuariosController(IUsuarioServico usuarioServico)
        {
            _usuarioServico = usuarioServico;
        }

        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            try
            {
                var usuarios = await _usuarioServico.ObterTodos();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    Message = "Erro ao consultar o usuário",
                    Error = ex.Message
                };

                return BadRequest(errorResponse);
            }
        }


        [HttpPost()]
        public async Task<IActionResult> Post([FromBody] Usuario usuario)
        {
            try
            {
                var novoUsuario = await _usuarioServico.Adicionar(usuario);
                return Ok(novoUsuario);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    Message = "Erro ao adicionar o usuário",
                    Error = ex.Message
                };

                return BadRequest(errorResponse);
            }
        }

        [HttpDelete()]
        [Route("/Usuarios/{usuarioId}")]
        public IActionResult Delete(string usuarioId)
        {
            try
            {
                _usuarioServico.Remover(usuarioId);
                return Ok($"Usuario {usuarioId} removido com sucesso");
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    Message = "Erro ao remover o usuário",
                    Error = ex.Message
                };

                return BadRequest(errorResponse);
            }
        }

        [HttpPut()]
        public async Task<IActionResult> Put(string usuarioId, [FromBody] Usuario usuario)
        {
            try
            {
                var usuarioAtualizado = await _usuarioServico.Atualizar(usuarioId, usuario);
                return Ok(usuarioAtualizado);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    Message = "Erro ao atualizar o usuário",
                    Error = ex.Message
                };

                return BadRequest(errorResponse);
            }
        }

        [HttpPost()]
        [Route("/Usuarios/Login")]
        public async Task<IActionResult> Login(Login dados)
        {
            try
            {
                await _usuarioServico.LogarUsuario(dados);
                return Ok("Login realizado com sucesso.");
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    Message = "Erro ao logar o usuário",
                    Error = ex.Message
                };

                return BadRequest(errorResponse);
            }
        }
    }
}
