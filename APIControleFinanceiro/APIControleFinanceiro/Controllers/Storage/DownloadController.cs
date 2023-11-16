using APIControleFinanceiro.Models.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIControleFinanceiro.Controllers.Storage
{
    [Route("controller")]
    [ApiController]
    public class DownloadController : ControllerBase
    {
        private readonly IUsuarioServico _usuarioServico;
        public DownloadController(IUsuarioServico usuarioServico)
        {
            _usuarioServico = usuarioServico;
        }

        [HttpPost()]
        [Authorize]
        [Route("/DownloadFotoUsuario/{idUsuario}")]
        [ApiVersion("1.0")]
        public async Task<IActionResult> Post(string idUsuario)
        {
            try
            {
                var usuario = await _usuarioServico.ObterPorId(idUsuario) ?? throw new Exception("Usuário não encontrado");

                var dataBytes = System.IO.File.ReadAllBytes(usuario.CaminhoFoto);

                string extensaoArquivo = Path.GetExtension(usuario.CaminhoFoto).ToLower();
              
                return File(dataBytes, $"image/{extensaoArquivo.Substring(1)}");
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    Message = "Erro ao fazer download da foto do usuário",
                    Error = ex.Message
                };

                return BadRequest(errorResponse);
            }
        }
    }
}
