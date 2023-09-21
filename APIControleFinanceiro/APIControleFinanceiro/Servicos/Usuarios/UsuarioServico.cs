using APIControleFinanceiro.Models.Usuarios;
using System.Text.RegularExpressions;
using static APIControleFinanceiro.Models.Database.DatabaseSettings;

namespace APIControleFinanceiro.Servicos.Usuarios
{
    public class UsuarioServico : IUsuarioServico
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public UsuarioServico(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public Task<List<Usuario>> ObterTodos()
        {
            return _usuarioRepositorio.GetUsuariosAsync();
        }

        public async Task<Usuario> Adicionar(Usuario usuario)
        {
            if (usuario == null)
                throw new Exception("Dados inválidos.");

            CriptografarSenha(usuario);
            await VerificarDuplicidadeEmail(usuario.Email, DatabaseStatus.Insercao.ToString());
            ValidarEstruturaEmail(usuario.Email);
            return await _usuarioRepositorio.CreateUsuarioAsync(usuario);
        }

        public void Remover(string usuarioId)
        {
            if (string.IsNullOrEmpty(usuarioId))
                throw new Exception("O Id do usuário é inválido");

            _usuarioRepositorio.DeleteUsuarioAsync(usuarioId);
        }

        public async Task<Usuario> Atualizar(string usuarioId, Usuario usuario)
        {
            if (usuario == null || string.IsNullOrEmpty(usuarioId))
                throw new Exception("Dados inválidos.");

            CriptografarSenha(usuario);
            await VerificarDuplicidadeEmail(usuario.Email, DatabaseStatus.Edicao.ToString());
            ValidarEstruturaEmail(usuario.Email);
            var atualizacao = await _usuarioRepositorio.UpdateUsuarioAsync(usuarioId, usuario);

            return atualizacao;
        }

        public async Task LogarUsuario(Login dados)
        {
            var senhaValida = await VerificarSenhaLogin(dados.SenhaLogin, dados.EmailLogin);

            if (!senhaValida)
                throw new Exception("A senha não é válida.");

            var usuario = await _usuarioRepositorio.GetUsuarioEmailAsync(dados.EmailLogin);

            if (usuario is null)
                throw new Exception("Não foi encontrado usuário com email correspondente.");
        }

        private void ValidarEstruturaEmail(string email)
        {
            string regex = @"^([a-z]){1,}([a-z0-9._-]){1,}([@]){1}([a-z]){2,}([.]){1}([a-z]){2,}([.]?){1}([a-z]?){2,}$";

            bool isEmailValid = Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);

            if (!isEmailValid)
                throw new Exception("Email inválido, por favor revise a informação.");

        }

        private async Task VerificarDuplicidadeEmail(string email, string status)
        {
            var existeEmail = await _usuarioRepositorio.GetUsuarioEmailAsync(email);
            if (existeEmail != null && status != DatabaseStatus.Edicao.ToString())
                throw new Exception("Email já cadastrado por outro usuário");
        }

        private Usuario CriptografarSenha(Usuario usuario)
        {
            string senhaCriptografada = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);
            usuario.Senha = senhaCriptografada;
            return usuario;
        }

        private async Task<bool> VerificarSenhaLogin(string senha, string email)
        {
            var encontrarUsuario = await _usuarioRepositorio.GetUsuarioEmailAsync(email);

             return BCrypt.Net.BCrypt.Verify(senha, encontrarUsuario.Senha);
        }
    }
}
