using APIControleFinanceiro.Domain.Models.CategoriasReceitas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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


        [HttpPost()]
        [Authorize]
        [ApiVersion("1.0")]
        public async Task<IActionResult> Post([FromBody] CategoriaReceita categoriaReceita)
        {
            try
            {
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
    }
}
