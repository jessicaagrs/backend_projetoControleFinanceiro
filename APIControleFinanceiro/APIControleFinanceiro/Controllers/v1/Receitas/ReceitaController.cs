using APIControleFinanceiro.Domain.Models.Receitas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APIControleFinanceiro.Controllers.v1.Receitas
{
    [ApiController]
    [Route("[controller]")]
    public class ReceitasController : ControllerBase
    {
        private IReceitaServico _receitaServico;

        public ReceitasController(IReceitaServico receitaServico)
        {
            _receitaServico = receitaServico;
        }

        // GET api/Receita
        /// <summary>
        /// Consulta receitas criadas.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Lista de receitas criadas.</returns>
        /// <response code="200">Retorna lista dos itens</response>
        /// <response code="400">Se houver erro</response>
        /// <response code="401">Não autorizado</response>
        [HttpGet()]
        [Authorize]
        [ApiVersion("1.0")]
        public async Task<IActionResult> Get(int numeroPagina, int quantidadePorPagina)
        {
            try
            {
                var receitas = await _receitaServico.ObterTodos(numeroPagina, quantidadePorPagina);
                return Ok(receitas);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    Message = "Erro ao consultar receitas",
                    Error = ex.Message
                };

                return BadRequest(errorResponse);
            }
        }

        // POST api/Receita
        /// <summary>
        /// Cria uma receita.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     POST /Receita
        ///     {
        ///         "descricao": "string",
        ///         "valor": 0,
        ///         "categoriaId": "string"
        ///         "data": "2023-12-12T14:02:55.118Z"
        ///     }
        ///
        /// </remarks>
        /// <param name="value"></param>
        /// <returns>Um novo item criado</returns>
        /// <response code="200">Retorna o novo item criado</response>
        /// <response code="400">Se o item não for criado</response>
        /// <response code="401">Não autorizado</response>
        [HttpPost()]
        [Authorize]
        [ApiVersion("1.0")]
        public async Task<IActionResult> Post([FromBody] Receita receita)
        {
            try
            {
                var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                receita.UsuarioId = usuarioId;
                var novaReceita = await _receitaServico.Adicionar(receita);
                return Ok(novaReceita);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    Message = "Erro ao adicionar receita",
                    Error = ex.Message
                };

                return BadRequest(errorResponse);
            }
        }

        // DELETE api/Receita/{receitaId}
        /// <summary>
        /// Excluir uma receita.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Id da receita excluída.</returns>
        /// <response code="200">Retorna item excluído com sucesso</response>
        /// <response code="400">Se houver erro</response>
        /// <response code="401">Não autorizado</response>
        [HttpDelete()]
        [Authorize]
        [Route("/Receitas/{receitaId}")]
        [ApiVersion("1.0")]
        public IActionResult Delete(string receitaId)
        {
            try
            {
                _receitaServico.Remover(receitaId);
                return Ok(new
                {
                    Mensagem = $"Receita {receitaId} removida com sucesso"
                });
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    Message = "Erro ao remover receita",
                    Error = ex.Message
                };

                return BadRequest(errorResponse);
            }
        }

        // PUT api/Receita
        /// <summary>
        /// Atualiza uma receita.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     POST /Receita
        ///     {
        ///         "id": "string",
        ///         "descricao": "string",
        ///         "valor": 0,
        ///         "categoriaId": "string"
        ///         "data": "2023-12-12T14:02:55.118Z"
        ///     }
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
        public async Task<IActionResult> Put([FromBody] Receita receita)
        {
            try
            {
                var receitaAtualizada = await _receitaServico.Atualizar(receita);
                return Ok(receitaAtualizada);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    Message = "Erro ao atualizar receita",
                    Error = ex.Message
                };

                return BadRequest(errorResponse);
            }
        }

        // POST api/Receitas/Importar
        /// <summary>
        /// Importação em lote de receitas.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     POST /Receitas/Importar
        ///     
        ///     Arquivo formato .csv
        ///
        /// </remarks>
        /// <param name="value"></param>
        /// <returns>Novos itens criados.</returns>
        /// <response code="200">Retorna a quantidade de itens adicionados</response>
        /// <response code="400">Se houver erro na importação</response>
        /// <response code="401">Não autorizado</response>
        [HttpPost()]
        [Authorize]
        [Route("/Receitas/Importar")]
        [ApiVersion("1.0")]
        public async Task<IActionResult> PostCsv(IFormFile arquivoCsv)
        {
            try
            {
                var resultado = await _receitaServico.AdicionarLista(arquivoCsv);
                return Ok($"Total de {resultado} receitas adicionadas");
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    Message = "Erro ao importar receitas",
                    Error = ex.Message
                };

                return BadRequest(errorResponse);
            }
        }
    }
}
