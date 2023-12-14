using APIControleFinanceiro.Domain.Models.CategoriasReceitas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APIControleFinanceiro.Controllers.v1.CategoriasReceitas
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriasReceitasController : ControllerBase
    {
        private ICategoriaReceitaServico _categoriaReceitaServico;

        public CategoriasReceitasController(ICategoriaReceitaServico categoriaReceitaServico)
        {
            _categoriaReceitaServico = categoriaReceitaServico;
        }

        // GET api/CategoriasReceitas
        /// <summary>
        /// Consulta categorias de receitas criadas.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Lista de categorias de receitas criadas.</returns>
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
                var categorias = await _categoriaReceitaServico.ObterTodos();
                return Ok(categorias);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    Message = "Erro ao consultar categoria de receita",
                    Error = ex.Message
                };

                return BadRequest(errorResponse);
            }
        }

        // POST api/CategoriasReceitas
        /// <summary>
        /// Cria uma categoria de receita.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     POST /CategoriasReceitas
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
        public async Task<IActionResult> Post([FromBody] CategoriaReceita categoriaReceita)
        {
            try
            {
                var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                categoriaReceita.UsuarioId = usuarioId;
                var novaCategoria = await _categoriaReceitaServico.Adicionar(categoriaReceita);
                return Ok(novaCategoria);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    Message = "Erro ao adicionar categoria de receita",
                    Error = ex.Message
                };

                return BadRequest(errorResponse);
            }
        }

        // DELETE api/CategoriasReceitas/{categoriaId}
        /// <summary>
        /// Excluir uma categoria de receita.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Id da categoria de receita excluída.</returns>
        /// <response code="200">Retorna item excluído com sucesso</response>
        /// <response code="400">Se houver erro</response>
        /// <response code="401">Não autorizado</response>
        [HttpDelete()]
        [Authorize]
        [Route("/CategoriasReceitas/{categoriaId}")]
        [ApiVersion("1.0")]
        public IActionResult Delete(string categoriaId)
        {
            try
            {
                _categoriaReceitaServico.Remover(categoriaId);
                return Ok(new
                {
                    Mensagem = $"Categoria de receita {categoriaId} removida com sucesso"
                });
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    Message = "Erro ao remover categoria de receita",
                    Error = ex.Message
                };

                return BadRequest(errorResponse);
            }
        }

        // PUT api/CategoriasReceitas
        /// <summary>
        /// Atualiza uma categoria de receita.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     POST /CategoriasReceitas
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
        public async Task<IActionResult> Put([FromBody] CategoriaReceita categoriaReceita)
        {
            try
            {
                var categoriaAtualizada = await _categoriaReceitaServico.Atualizar(categoriaReceita);
                return Ok(categoriaAtualizada);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    Message = "Erro ao atualizar categoria de receita",
                    Error = ex.Message
                };

                return BadRequest(errorResponse);
            }
        }

        // POST api/CategoriasReceitas/Importar
        /// <summary>
        /// Importação em lote de categorias de receita.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     POST /CategoriasReceitas/Importar
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
        [Route("/CategoriasReceitas/Importar")]
        [ApiVersion("1.0")]
        public async Task<IActionResult> PostCsv(IFormFile arquivoCsv)
        {
            try
            {
                var resultado = await _categoriaReceitaServico.AdicionarLista(arquivoCsv);
                return Ok($"Total de {resultado} categorias adicionadas");
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    Message = "Erro ao importar categorias de receita",
                    Error = ex.Message
                };

                return BadRequest(errorResponse);
            }
        }
    }
}
