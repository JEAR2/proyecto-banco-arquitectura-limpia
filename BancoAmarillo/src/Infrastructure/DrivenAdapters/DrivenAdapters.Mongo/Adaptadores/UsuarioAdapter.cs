using AutoMapper;
using credinet.exception.middleware.models;
using Domain.Model.Entidades;
using Domain.Model.Gateway;
using DrivenAdapters.Mongo.Entities;
using Helpers.Commons.Exceptions;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace DrivenAdapters.Mongo.Adaptadores
{
    /// <summary>
    /// <see cref=IAuthRepository"/>
    /// </summary>
    public class UsuarioAdapter : IAuthRepository
    {
        private readonly IContext _context;
        private readonly IMapper _mapper;
        private readonly FilterDefinitionBuilder<UsuarioEntity> _filtro;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        public UsuarioAdapter(IContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _filtro = Builders<UsuarioEntity>.Filter;
        }

        /// <summary>
        /// <see cref=IAuthRepository.IniciarSesionAsync(Usuario)"/>
        /// </summary>
        public async Task<Usuario> IniciarSesionAsync(Usuario usuario)
        {
            var usuarioEntity = await ObtenerUsuarioPorCorreo(usuario.Correo);
            if (usuarioEntity == null)
                throw new BusinessException("Usuario o contraseña incorrectos",
                    (int)TipoExcepcionNegocio.ExceptioNoAutorizado);
            if (!VerificarConstrasena(usuarioEntity.Clave, usuario.Clave))
                throw new BusinessException("Usuario o contraseña incorrectos",
                       (int)TipoExcepcionNegocio.ExceptioNoAutorizado);
            return _mapper.Map<Usuario>(usuarioEntity);
        }

        /// <summary>
        /// <see cref=IAuthRepository.RegistrarUsuario(Usuario)"/>
        /// </summary>
        public async Task<Usuario> RegistrarUsuario(Usuario usuario)
        {
            if (await ObtenerUsuarioPorCorreo(usuario.Correo) != null)
                throw new BusinessException("El usuario ya se encuentra registrado"
                    , (int)TipoExcepcionNegocio.ExceptioNoAutorizado);

            var hash = HashConstrasena(usuario.Clave);
            usuario.Clave = hash;
            var nuevoUsuario = _mapper.Map<UsuarioEntity>(usuario);
            await _context.Usuarios.InsertOneAsync(nuevoUsuario);

            return _mapper.Map<Usuario>(nuevoUsuario);
        }

        /// <summary>
        /// Encriptar contasena
        /// </summary>
        /// <param name="contrasena"></param>
        /// <returns></returns>
        private string HashConstrasena(string contrasena)
        {
            return BCrypt.Net.BCrypt.HashPassword(contrasena);
        }

        /// <summary>
        /// Verifica contrasena
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="contrasena"></param>
        /// <returns></returns>
        private bool VerificarConstrasena(string hash, string contrasena)
        {
            return BCrypt.Net.BCrypt.Verify(contrasena, hash);
        }

        private async Task<UsuarioEntity> ObtenerUsuarioPorCorreo(string correo)
        {
            var cursor = await _context.Usuarios.FindAsync(_filtro.Eq(u => u.Correo, correo));
            return cursor.FirstOrDefault();
        }
    }
}