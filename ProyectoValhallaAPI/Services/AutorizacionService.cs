using ProyectoValhallaAPI.Models;
using ProyectoValhallaAPI.Models.Customs;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProyectoValhallaAPI.Services
{
    public class AutorizacionService : IAutorizacionService
    {
        private readonly ValhallaDbContext _context;
        private readonly IConfiguration _configuration;

        public AutorizacionService(ValhallaDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private string GenerarToken(string idUsuario)
        {
            var key = _configuration.GetValue<string>("JwtSettings:key");
            //Convertimos esa llave en un array
            var keyBytes = Encoding.ASCII.GetBytes(key!);
            //Los Claim contienen la informacion del usuario para  nuestro Token
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, idUsuario));
            //Crear credencial para el token
            var credencialesToken = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),
                //Nivel de encriptamiento
                SecurityAlgorithms.HmacSha256
                );
            //Crear el detalle del token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(90),
                SigningCredentials = credencialesToken
            };
            //Crear los controladores de JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);
            //Obtener el Token
            string tokenCreado = tokenHandler.WriteToken(tokenConfig);
            return tokenCreado;
        }

        private string GenerarRefreshToken()
        {
            var byteArray = new byte[64];
            var refreshToken = "";

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(byteArray);
                refreshToken = Convert.ToBase64String(byteArray);
            }
            return refreshToken;
        }

        private async Task<AutorizacionResponse> GuardarHistorialRefreshToken(int idUsuario, string token, string refreshToken)
        {
            var historialRefreshToken = new HistorialRefreshToken
            {
                IdUsuario = idUsuario,
                Token = token,
                RefreshToken = refreshToken,
                FechaCreacion = DateTime.UtcNow,
                FechaExpiracion = DateTime.UtcNow.AddMinutes(2)
            };

            await _context.HistorialRefreshTokens.AddAsync(historialRefreshToken);
            await _context.SaveChangesAsync();

            return new AutorizacionResponse { Token = token, RefreshToken = refreshToken, Resultado = true, Mensaje = "Ok" };
        }

        public async Task<AutorizacionResponse> DevolverToken(AutorizacionRequest autorizacion)
        {
            //Validar si el usuario Existe
            var usuario_encontrado = _context.Usuarios.FirstOrDefault(x => x.Username == autorizacion.Username && x.CorreoElectronico == autorizacion.CorreoElectronico &&
               x.Password == autorizacion.Password
            );
            if (usuario_encontrado == null)
            {
                return await Task.FromResult<AutorizacionResponse>(null);
            }
            string tokenCreado = GenerarToken(usuario_encontrado.IdUsuario.ToString());

            //return new AutorizacionResponse() { Token = tokenCreado, Resultado = true, Mensaje = "Ok" };

            string refreshTokenCreado = GenerarRefreshToken();

            return await GuardarHistorialRefreshToken(usuario_encontrado.IdUsuario, tokenCreado, refreshTokenCreado);
        }

        public async Task<AutorizacionResponse> DevolverRefreshToken(RefreshTokenRequest refreshTokenRequest, int idUsuario)
        {

            var refreshTokenEncontrado = _context.HistorialRefreshTokens.FirstOrDefault(token =>
                token.Token == refreshTokenRequest.TokenExpirado &&
                token.RefreshToken == refreshTokenRequest.RefreshToken &&
                token.IdUsuario == idUsuario
            );

            if (refreshTokenEncontrado == null)
            {
                return new AutorizacionResponse { Resultado = false, Mensaje = "No existe Refresh - Token" };
            }

            var refreshTokenCreado = GenerarRefreshToken();
            var tokenCreado = GenerarToken(idUsuario.ToString());

            return await GuardarHistorialRefreshToken(idUsuario, tokenCreado, refreshTokenCreado);
        }
    }
}
