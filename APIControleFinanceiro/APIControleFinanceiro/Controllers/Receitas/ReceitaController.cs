using APIControleFinanceiro.Models.Receitas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Receitas
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

        [HttpGet()]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            try
            {
                var receitas = await _receitaServico.ObterTodos();
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


        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] Receita receita)
        {
            try
            {
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

        [HttpDelete()]
        [Authorize]
        [Route("/Receitas/{receitaId}")]
        public IActionResult Delete(string receitaId)
        {
            try
            {
                _receitaServico.Remover(receitaId);
                return Ok($"Receita {receitaId} removida com sucesso");
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

        [HttpPut()]
        [Authorize]
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
    }
}
