using APIControleFinanceiro.Domain.Models.Receitas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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


        [HttpPost()]
        [Authorize]
        [ApiVersion("1.0")]
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
    }
}
