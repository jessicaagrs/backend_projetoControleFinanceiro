using APIControleFinanceiro.Domain.Models.CategoriasDespesas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APIControleFinanceiro.Controllers.v1.CategoriasDespesas
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriasDespesasController : ControllerBase
    {
        private ICategoriaDespesaServico _categoriaDespesaServico;

        public CategoriasDespesasController(ICategoriaDespesaServico categoriaDespesaServico)
        {
            _categoriaDespesaServico = categoriaDespesaServico;
        }

        // GET api/CategoriasDespesas
        /// <summary>
        /// Consulta categorias de despesas criadas.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Lista de categorias de despesas criadas.</returns>
        /// <response code="200">Retorna lista dos itens</response>
        /// <response code="400">Se houver erro</response>
        /// <response code="401">Não autorizado</response>
        [HttpGet()]
        [Authorize]
        [ApiVersion("1.0")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var categorias = await _categoriaDespesaServico.ObterTodos();
                return Ok(categorias);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    Message = "Erro ao consultar categoria de despesa",
                    Error = ex.Message
                };

                return BadRequest(errorResponse);
            }
        }

        // POST api/CategoriasDespesas
        /// <summary>
        /// Cria uma categoria de despesa.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     POST /CategoriasDespesas
        ///     {
        ///         "descricao": "string"
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
        public async Task<IActionResult> Post([FromBody] CategoriaDespesa categoriaDespesa)
        {
            try
            {
                var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                categoriaDespesa.UsuarioId = usuarioId;
                var novaCategoria = await _categoriaDespesaServico.Adicionar(categoriaDespesa);
                return Ok(novaCategoria);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    Message = "Erro ao adicionar categoria de despesa",
                    Error = ex.Message
                };

                return BadRequest(errorResponse);
            }
        }

        // DELETE api/CategoriasDespesas/{categoriaId}
        /// <summary>
        /// Excluir uma categoria de despesa.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Id da categoria de despesa excluída.</returns>
        /// <response code="200">Retorna item excluído com sucesso</response>
        /// <response code="400">Se houver erro</response>
        /// <response code="401">Não autorizado</response>
        [HttpDelete()]
        [Authorize]
        [Route("/CategoriasDespesas/{categoriaId}")]
        [ApiVersion("1.0")]
        public IActionResult Delete(string categoriaId)
        {
            try
            {
                _categoriaDespesaServico.Remover(categoriaId);
                return Ok(new
                {
                    Mensagem = $"Categoria de despesa {categoriaId} removida com sucesso"
                });
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    Message = "Erro ao remover categoria de despesa",
                    Error = ex.Message
                };

                return BadRequest(errorResponse);
            }
        }

        // PUT api/CategoriasDespesas
        /// <summary>
        /// Atualiza uma categoria de despesa.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     POST /CategoriasDespesas
        ///     {
        ///         "id": "string",
        ///         "descricao": "string"
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
        public async Task<IActionResult> Put([FromBody] CategoriaDespesa categoriaDespesa)
        {
            try
            {
                var categoriaAtualizada = await _categoriaDespesaServico.Atualizar(categoriaDespesa);
                return Ok(categoriaAtualizada);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    Message = "Erro ao atualizar categoria de despesa",
                    Error = ex.Message
                };

                return BadRequest(errorResponse);
            }
        }

        // POST api/CategoriasDespesas/Importar
        /// <summary>
        /// Importação em lote de categorias de despesa.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     POST /CategoriasDespesas/Importar
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
        [Route("/CategoriasDespesas/Importar")]
        [ApiVersion("1.0")]
        public async Task<IActionResult> PostCsv(IFormFile arquivoCsv)
        {
            try
            {
                var resultado = await _categoriaDespesaServico.AdicionarLista(arquivoCsv);
                return Ok($"Total de {resultado} categorias adicionadas");
            }
            catch (Exception ex)
            {   
                var errorResponse = new
                {
                    Message = "Erro ao importar categorias de despesa",
                    Error = ex.Message
                };

                return BadRequest(errorResponse);
            }
        }
    }
}
