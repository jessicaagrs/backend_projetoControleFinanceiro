using APIControleFinanceiro.Domain.Models.Despesas;
using APIControleFinanceiro.Domain.Models.Receitas;
using APIControleFinanceiro.Infra.Repositorios.Despesas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APIControleFinanceiro.Controllers.v1.Despesas
{
    [ApiController]
    [Route("[controller]")]
    public class DespesasController : ControllerBase
    {
        private IDespesaServico _despesaServico;

        public DespesasController(IDespesaServico despesaServico)
        {
            _despesaServico = despesaServico;
        }

        // GET api/Despesa
        /// <summary>
        /// Consulta despesas criadas.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Lista de despesas criadas.</returns>
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
                var despesas = await _despesaServico.ObterTodos(numeroPagina, quantidadePorPagina);
                return Ok(despesas);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    Message = "Erro ao consultar despesas",
                    Error = ex.Message
                };

                return BadRequest(errorResponse);
            }
        }

        // POST api/Despesa
        /// <summary>
        /// Cria uma despesa.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     POST /Despesa
        ///     {
        ///         "descricao": "string",
        ///         "local": "string",
        ///         "valor": 0,
        ///         "usuarioId": "string",
        ///         "categoriaId": "string",
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

        public async Task<IActionResult> Post([FromBody] Despesa despesa)
        {
            try
            {
                var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                despesa.UsuarioId = usuarioId;
                var novaDespesa = await _despesaServico.Adicionar(despesa);
                return Ok(novaDespesa);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    Message = "Erro ao adicionar despesa",
                    Error = ex.Message
                };

                return BadRequest(errorResponse);
            }
        }

        // DELETE api/Despesas/{despesaId}
        /// <summary>
        /// Excluir uma despesa.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Id da despesa excluída.</returns>
        /// <response code="200">Retorna item excluído com sucesso</response>
        /// <response code="400">Se houver erro</response>
        /// <response code="401">Não autorizado</response>
        [HttpDelete()]
        [Authorize]
        [ApiVersion("1.0")]

        [Route("/Despesas/{despesaId}")]
        public IActionResult Delete(string despesaId)
        {
            try
            {
                _despesaServico.Remover(despesaId);
                return Ok(new
                {
                    Mensagem = $"Despesa {despesaId} removida com sucesso"
                });
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    Message = "Erro ao remover despesa",
                    Error = ex.Message
                };

                return BadRequest(errorResponse);
            }
        }

        // PUT api/Despesa
        /// <summary>
        /// Atualiza uma despesa.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     POST /Despesa
        ///     {
        ///         "id": "string",
        ///         "descricao": "string",
        ///         "local": "string",
        ///         "valor": 0,
        ///         "usuarioId": "string",
        ///         "categoriaId": "string",
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
        public async Task<IActionResult> Put([FromBody] Despesa despesa)
        {
            try
            {
                var despesaAtualizada = await _despesaServico.Atualizar(despesa);
                return Ok(despesaAtualizada);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    Message = "Erro ao atualizar despesa",
                    Error = ex.Message
                };

                return BadRequest(errorResponse);
            }
        }

        // POST api/Despesas/Importar
        /// <summary>
        /// Importação em lote de despesas.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     POST /Despesas/Importar
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
        [Route("/Despesas/Importar")]
        [ApiVersion("1.0")]
        public async Task<IActionResult> PostCsv(IFormFile arquivoCsv)
        {
            try
            {
                var resultado = await _despesaServico.AdicionarLista(arquivoCsv);
                return Ok($"Total de {resultado} despesas adicionadas");
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    Message = "Erro ao importar despesas",
                    Error = ex.Message
                };

                return BadRequest(errorResponse);
            }
        }
    }
}
