
using APIControleFinanceiro.Controllers.Models;
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
        private readonly IUsuarioServico _usuarioServico;

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
        public async Task<IActionResult> Post([FromForm] UsuarioControllerModel dadosRequisicao)
        {
            try
            {
                var usuario = new Usuario
                {
                    Nome = dadosRequisicao.Nome,
                    Email = dadosRequisicao.Email,
                    Senha = dadosRequisicao.Senha,
                };

                usuario = _usuarioServico.SalvarFotoStorage(dadosRequisicao.Foto, usuario);

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
        public async Task<IActionResult> Put([FromForm] UsuarioControllerModel dadosRequisicao)
        {
            try
            {
                if (string.IsNullOrEmpty(dadosRequisicao.Id))
                    throw new Exception("Dados de usuário inválido, informar Id");

                var usuario = await _usuarioServico.ObterPorId(dadosRequisicao.Id);
                usuario.Nome = dadosRequisicao.Nome;
                usuario.Email = dadosRequisicao.Email;
                usuario.Senha = dadosRequisicao.Senha;
                usuario = _usuarioServico.SalvarFotoStorage(dadosRequisicao.Foto, usuario);

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
