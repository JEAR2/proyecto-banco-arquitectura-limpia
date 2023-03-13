using Domain.Model.Entidades;
using System.Threading.Tasks;

namespace Domain.UseCase.Auth
{
    /// <summary>
    /// Interface IAuthUseCase
    /// </summary>
    public interface IAuthUseCase
    {
        /// <summary>
        /// Iniciar sesión
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        Task<AccesToken> IniciarSesion(Usuario usuario);

        /// <summary>
        /// Crear cliente
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        Task<AccesToken> CrearUsuario(Usuario usuario);
    }
}