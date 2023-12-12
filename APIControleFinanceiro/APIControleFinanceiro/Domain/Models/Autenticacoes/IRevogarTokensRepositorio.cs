namespace APIControleFinanceiro.Domain.Models.Autenticacoes
{
    public interface IRevogarTokensRepositorio
    {
        public void AdicionarTokenRevogado(TokenRevogado tokenRevogado);
        public bool VerificarToken(string token);
    }
}
