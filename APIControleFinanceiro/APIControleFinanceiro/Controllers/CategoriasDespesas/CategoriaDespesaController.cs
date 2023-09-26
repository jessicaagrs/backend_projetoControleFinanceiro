
using APIControleFinanceiro.Models.CategoriasDespesas;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.CategoriasDespesas
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

        [HttpGet()]
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


        [HttpPost()]
        public async Task<IActionResult> Post([FromBody] CategoriaDespesa categoriaDespesa)
        {
            try
            {
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

        [HttpDelete()]
        [Route("/CategoriasDespesas/{categoriaId}")]
        public IActionResult Delete(string categoriaId)
        {
            try
            {
                _categoriaDespesaServico.Remover(categoriaId);
                return Ok($"Categoria de despesa {categoriaId} removida com sucesso");
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

        [HttpPut()]
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
    }
}
