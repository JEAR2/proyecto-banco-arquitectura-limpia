using Domain.Model.Entidades;
using Domain.Model.Gateway;
using Helpers.ObjectsUtils;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Domain.UseCase.Auth
{
    /// <summary>
    /// <see cref="IAuthUseCase"/>
    /// </summary>
    public class AuthUseCase : IAuthUseCase
    {
        private readonly IOptions<ConfiguradorAppSettings> _options;
        private readonly IAuthRepository _auth;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        /// <param name="auth"></param>
        public AuthUseCase(IOptions<ConfiguradorAppSettings> options, IAuthRepository auth)
        {
            _options = options;
            _auth = auth;
        }

        /// <summary>
        /// <see cref="IAuthUseCase.CrearUsuario(Usuario)"/>
        /// </summary>
        public async Task<AccesToken> CrearUsuario(Usuario usuario)
        {
            var nuevoUsuario = await _auth.RegistrarUsuario(usuario);

            var token = GenerarToken(nuevoUsuario);
            return token;
        }

        /// <summary>
        /// <see cref="IAuthUseCase.IniciarSesion(Usuario)"/>
        /// </summary>
        public async Task<AccesToken> IniciarSesion(Usuario usuario)
        {
            var UsuarioValido = await _auth.IniciarSesionAsync(usuario);

            return GenerarToken(UsuarioValido);
        }

        private AccesToken GenerarToken(Usuario usuario)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Role,usuario.Rol.ToString()),
                new Claim("id",usuario.Id),
                new Claim("correo", usuario.Correo)
            };
            var clave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.KeyJwt));
            var credenciales = new SigningCredentials(clave, SecurityAlgorithms.HmacSha256);
            var expiracion = DateTime.UtcNow.AddDays(5);
            var token = new JwtSecurityToken(
                issuer: _options.Value.DomainName,
                audience: "localhost",
                claims,
                expires: expiracion,
                signingCredentials: credenciales);

            return new() { AccessToken = new JwtSecurityTokenHandler().WriteToken(token) };
        }
    }
}