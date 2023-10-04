using APIControleFinanceiro.Models.Despesas;
using APIControleFinanceiro.Repositorios.Despesas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Despesas
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

        [HttpGet()]
        [Authorize]

        public async Task<IActionResult> Get()
        {
            try
            {
                var despesas = await _despesaServico.ObterTodos();
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


        [HttpPost()]
        [Authorize]

        public async Task<IActionResult> Post([FromBody] Despesa despesa)
        {
            try
            {
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

        [HttpDelete()]
        [Authorize]

        [Route("/Despesas/{despesaId}")]
        public IActionResult Delete(string despesaId)
        {
            try
            {
                _despesaServico.Remover(despesaId);
                return Ok($"Despesa {despesaId} removida com sucesso");
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

        [HttpPut()]
        [Authorize]

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
    }
}
