using APIControleFinanceiro.Controllers.v1.Models;
using APIControleFinanceiro.Domain.Models.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIControleFinanceiro.Controllers.v1.Usuarios
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

        // GET api/Usuarios
        /// <summary>
        /// Consulta usuário cadastrado.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>O usuário cadastrado.</returns>
        /// <response code="200">Retorna um objeto com dados</response>
        /// <response code="400">Se houver erro</response>
        /// <response code="401">Não autorizado</response>
        [HttpGet()]
        [Authorize()]
        [Route("/Usuarios/{email}")]
        [ApiVersion("1.0")]
        public async Task<IActionResult> Get(string email)
        {
            try
            {
                var usuarios = await _usuarioServico.ObterPorEmail(email);
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

        // POST api/Usuarios
        /// <summary>
        /// Cria um usuário.
        /// </summary>
        /// <remarks>
        /// Preencher campos:
        ///
        ///     POST /Usuarios
        ///     
        ///     Nome: string
        ///     Email: string
        ///     Senha: string
        ///     Foto: png ou jpeg
        ///     Numero Cartão: inteiro com 16 dígitos
        ///     Validade Cartão: 01/01/2000 (sempre usar dia 01)
        ///     Bandeira: string
        ///     Banco: string
        ///
        /// </remarks>
        /// <param name="value"></param>
        /// <returns>Um novo item criado</returns>
        /// <response code="200">Retorna o novo item criado</response>
        /// <response code="400">Se o item não for criado</response>
        /// <response code="401">Não autorizado</response>
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
                    NumeroCartaoCredito = dadosRequisicao.NumeroCartaoCredito,
                    ValidadeCartaoCredito = dadosRequisicao.ValidadeCartaoCredito,
                    BandeiraCartaoCredito = dadosRequisicao.BandeiraCartaoCredito,
                    BancoCartaoCredito = dadosRequisicao.BancoCartaoCredito
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

        // DELETE api/Usuarios/{usuarioId}
        /// <summary>
        /// Excluir um usuário.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Id do usuário excluído.</returns>
        /// <response code="200">Retorna item excluído com sucesso</response>
        /// <response code="400">Se houver erro</response>
        /// <response code="401">Não autorizado</response>
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

        // PUT api/Usuarios
        /// <summary>
        /// Atualiza um usuário.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     POST /Usuarios
        ///     
        ///     Id: string
        ///     Nome: string
        ///     Email: string
        ///     Senha: string
        ///     Foto: png ou jpeg
        ///     Numero Cartão: inteiro com 16 dígitos
        ///     Validade Cartão: 01/01/2000 (sempre usar dia 01)
        ///     Bandeira: string
        ///     Banco: string
        ///
        /// </remarks>
        /// <param name="value"></param>
        /// <returns>Um novo item atualizado</returns>
        /// <response code="200">Retorna o novo item atualizado</response>
        /// <response code="400">Se o item não for atualizado</response>
        /// <response code="401">Não autorizado</response>
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
