using Domain.Model.Entidades;
using System.Threading.Tasks;

namespace Domain.Model.Gateway
{
    /// <summary>
    /// IAuth
    /// </summary>
    public interface IAuthRepository
    {
        /// <summary>
        /// Iniciar sesión
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        Task<Usuario> IniciarSesionAsync(Usuario usuario);

        /// <summary>
        /// Crear cliente
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        Task<Usuario> RegistrarUsuario(Usuario usuario);
    }
}