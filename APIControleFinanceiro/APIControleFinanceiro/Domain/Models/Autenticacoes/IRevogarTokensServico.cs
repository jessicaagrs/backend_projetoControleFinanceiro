namespace APIControleFinanceiro.Domain.Models.Autenticacoes
{
    public interface IRevogarTokensServico
    {
        public void RevogarTokens(TokenRevogado tokenRevogado);
        public bool TokenRevogado(string token);
    }
}
