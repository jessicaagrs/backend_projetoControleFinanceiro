using APIControleFinanceiro.Domain.Models.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIControleFinanceiro.Controllers.v1.Storage
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

        // POST api/DownloadFotoUsuario/{idUsuario}
        /// <summary>
        /// Download da foto de usuário.
        /// </summary>
        /// <remarks>
        /// Preencher campo:
        ///
        ///     POST /DownloadFotoUsuario/{idUsuario}
        ///     
        ///     Id do usuário: string
        ///
        /// </remarks>
        /// <param name="value"></param>
        /// <returns>A foto cadastrada pelo usuário</returns>
        /// <response code="200">Retorna a foto</response>
        /// <response code="400">Se a foto não puder ser obtida</response>
        /// <response code="401">Não autorizado</response>
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
