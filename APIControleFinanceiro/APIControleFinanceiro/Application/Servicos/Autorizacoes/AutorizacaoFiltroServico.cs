using APIControleFinanceiro.Domain.Models.Autenticacoes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace APIControleFinanceiro.Application.Servicos.Autorizacoes
{
    public class AutorizacaoFiltroServico : IAuthorizationFilter
    {
        private readonly IRevogarTokensServico _revogarTokensServico;

        public AutorizacaoFiltroServico(IRevogarTokensServico revogarTokensServico)
        {
            _revogarTokensServico = revogarTokensServico;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (_revogarTokensServico.TokenRevogado(token))
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
