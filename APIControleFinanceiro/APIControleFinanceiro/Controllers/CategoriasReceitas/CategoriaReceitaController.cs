
using APIControleFinanceiro.Models.CategoriasReceitas;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Usuarios
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
        [Route("/CategoriasReceitas/{categoriaId}")]
        public IActionResult Delete(string categoriaId)
        {
            try
            {
                _categoriaReceitaServico.Remover(categoriaId);
                return Ok($"Usuario {categoriaId} removido com sucesso");
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
