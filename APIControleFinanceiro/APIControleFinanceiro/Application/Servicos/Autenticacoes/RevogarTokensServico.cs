using APIControleFinanceiro.Domain.Models.Autenticacoes;

namespace APIControleFinanceiro.Application.Servicos.Autenticacoes
{
    public class RevogarTokensServico : IRevogarTokensServico
    {
        private readonly IRevogarTokensRepositorio _revogarTokenRepositorio;

        public RevogarTokensServico(IRevogarTokensRepositorio revogarTokensRepositorio)
        {
            _revogarTokenRepositorio = revogarTokensRepositorio;
        }
        public void RevogarTokens(TokenRevogado tokenRevogado)
        {
            _revogarTokenRepositorio.AdicionarTokenRevogado(tokenRevogado);
        }

        public bool TokenRevogado(string token)
        {
            return _revogarTokenRepositorio.VerificarToken(token);
        }
    }
}
