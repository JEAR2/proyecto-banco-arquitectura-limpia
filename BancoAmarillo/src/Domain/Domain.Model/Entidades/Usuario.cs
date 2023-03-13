using Domain.Model.Entidades.Enums;

namespace Domain.Model.Entidades
{
    /// <summary>
    /// Usuario
    /// </summary>
    public class Usuario
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Correo
        /// </summary>
        public string Correo { get; set; }

        /// <summary>
        /// Clave
        /// </summary>
        public string Clave { get; set; }

        /// <summary>
        /// Rol
        /// </summary>
        public Rol Rol { get; set; }
    }
}