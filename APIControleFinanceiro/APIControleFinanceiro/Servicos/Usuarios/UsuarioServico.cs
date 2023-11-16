using API.Controllers.Usuarios;
using APIControleFinanceiro.Controllers.Models;
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

        public Task<Usuario> ObterPorId(string id)
        {
            return _usuarioRepositorio.GetUsuarioPorIdAsync(id);
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

        public async Task<Usuario> Atualizar(Usuario usuario)
        {
            if (usuario == null)
                throw new Exception("Dados inválidos.");

            CriptografarSenha(usuario);
            await VerificarDuplicidadeEmail(usuario.Email, DatabaseStatus.Edicao.ToString());
            ValidarEstruturaEmail(usuario.Email);
            var usuarios = await _usuarioRepositorio.GetUsuariosAsync();
            var existeUsuario = usuarios.FirstOrDefault(u => u.Id == usuario.Id);

            if (existeUsuario == null)
                throw new Exception("O usuário não existe");
            var atualizacao = await _usuarioRepositorio.UpdateUsuarioAsync(usuario);

            return atualizacao;
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
            if (!usuario.Senha.StartsWith("$2a$"))
            {
                string senhaCriptografada = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);
                usuario.Senha = senhaCriptografada;
            }

            return usuario;
        }

        public Usuario SalvarFotoStorage(IFormFile foto, Usuario usuario)
        {
            if (foto != null)
            {
                string diretorioBase = Path.Combine("Storage", usuario.Email);
                if (!Directory.Exists(diretorioBase))
                {
                    Directory.CreateDirectory(diretorioBase);
                }
                var caminhoArquivo = Path.Combine(diretorioBase, foto.FileName);

                string extensaoArquivo = Path.GetExtension(caminhoArquivo.ToLower());

                if (extensaoArquivo != ".png" || extensaoArquivo != ".jpeg" || extensaoArquivo != ".jpg")
                    throw new Exception("Os formatos aceitos para foto são JPEG ou PNG.");

                using Stream arquivoStream = new FileStream(caminhoArquivo, FileMode.Create);
                foto.CopyTo(arquivoStream);

                if(usuario.CaminhoFoto != caminhoArquivo)
                    usuario.CaminhoFoto = caminhoArquivo;
            }

            return usuario;
        }

    }
}
