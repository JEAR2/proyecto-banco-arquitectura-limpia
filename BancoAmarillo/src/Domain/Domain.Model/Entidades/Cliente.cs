using credinet.exception.middleware.models;
using Domain.Model.Entidades.Enums;
using Helpers.Commons.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Model.Entidades
{
    /// <summary>
    /// Clase Cliente
    /// </summary>
    public class Cliente
    {
        /// <summary>
        /// Id de CLiente
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Tipo de Identificación
        /// </summary>
        public TipoIdentificacion TipoIdentificacion { get; set; }

        /// <summary>
        /// Número de Identificación
        /// </summary>
        public string NumeroIdentificacion { get; set; }

        /// <summary>
        /// Nombre del Cliente
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Apellido del Cliente
        /// </summary>
        public string Apellido { get; set; }

        /// <summary>
        /// Correo del Cliente
        /// </summary>
        public string Correo { get; set; }

        /// <summary>
        /// Fecha de Nacimiento del Cliente
        /// </summary>
        public DateTime FechaNacimiento { get; set; }

        /// <summary>
        /// Fecha de Creación del Cliente
        /// </summary>
        public DateTime FechaCreacion { get; set; }

        /// <summary>
        /// Fecha de Modificación del Cliente
        /// </summary>
        public DateTime FechaModificacion { get; set; }

        /// <summary>
        /// Usuario que realiza la modificación
        /// </summary>
        public string UsuarioModificacion { get; set; }

        /// <summary>
        /// Estado del Cliente
        /// </summary>
        public EstadoCliente Estado { get; set; }

        /// <summary>
        /// Cuenta asignadas al Cliente
        /// </summary>
        public List<Cuenta> Cuentas { get; set; }

        /// <summary>
        /// Regla de negocio
        /// </summary>
        /// <exception cref="BusinessException"></exception>
        public void ValidarGMF()
        {
            var gmf = Cuentas.Find(Cuenta => Cuenta.GMF);
            if (gmf != null)
                throw new BusinessException($"Solo se permite una cuenta con GMF {gmf.NumeroCuenta}",
                       (int)TipoExcepcionNegocio.ExceptionReglaaNegocio);
        }

        public void ValidarCuentasCanceladas()
        {
            var cuentaActiva = Cuentas.FirstOrDefault(cuenta => cuenta.EstadoCuenta != EstadoCuenta.CANCELADA);
            if (cuentaActiva != null)
                throw new BusinessException($"No se puede eliminar un cliente con cuentas activas: Cuenta activa {cuentaActiva.NumeroCuenta}",
                    (int)TipoExcepcionNegocio.ExceptionReglaaNegocio);
        }
    }
}