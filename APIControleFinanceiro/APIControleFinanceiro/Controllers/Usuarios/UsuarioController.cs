
using APIControleFinanceiro.Models.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

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
        [Authorize()]
        [ApiVersion("1.0")]
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
        [ApiVersion("1.0")]
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
        [Authorize]
        [Route("/Usuarios/{usuarioId}")]
        [ApiVersion("1.0")]
        public IActionResult Delete(string usuarioId)
        {
            try
            {
                _usuarioServico.Remover(usuarioId);
                return Ok(new
                {
                    Mensagem = $"Usuario {usuarioId} removido com sucesso"
                });
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
        [Authorize]
        [ApiVersion("1.0")]
        public async Task<IActionResult> Put([FromBody] Usuario usuario)
        {
            try
            {
                var usuarioAtualizado = await _usuarioServico.Atualizar(usuario);
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
      
    }
}
