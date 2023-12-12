using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoValhallaAPI.Models.Customs;
using ProyectoValhallaAPI.Services;
using System.IdentityModel.Tokens.Jwt;

namespace ProyectoValhallaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacionController : ControllerBase
    {
        private readonly IAutorizacionService _autorizacionService;

        public AutenticacionController(IAutorizacionService autorizacionService)
        {
            _autorizacionService = autorizacionService;
        }

        [HttpPost]
        [Route("Autenticar")]

        public async Task<IActionResult> Autenticar([FromBody] AutorizacionRequest autorizacion)
        {
            var resultado_autorizacion = await _autorizacionService.DevolverToken(autorizacion);
            if (resultado_autorizacion == null)
            {
                return Unauthorized();
            }

            return Ok(resultado_autorizacion);
        }

        [HttpPost]
        [Route("ObtenerRefreshToken")]
        public async Task<IActionResult> ObtenerRefreshToken([FromBody] RefreshTokenRequest request)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenExpiradoSupuestamente = tokenHandler.ReadJwtToken(request.TokenExpirado);

            if (tokenExpiradoSupuestamente.ValidTo > DateTime.UtcNow)
            {
                return BadRequest(new AutorizacionResponse { Resultado = false, Mensaje = "Token No ha Expirado" });
            }

            string idUsuario = tokenExpiradoSupuestamente.Claims.First(x =>
                x.Type == JwtRegisteredClaimNames.NameId).Value.ToString();

            var autorizacionResponse = await _autorizacionService.DevolverRefreshToken(request, int.Parse(idUsuario));

            if (autorizacionResponse.Resultado)
            {
                return Ok(autorizacionResponse);
            }
            else
            {
                return BadRequest(autorizacionResponse);
            }
        }
    }
}
