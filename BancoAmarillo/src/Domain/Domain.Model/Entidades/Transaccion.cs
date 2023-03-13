using System;
using credinet.exception.middleware.models;
using System.Transactions;
using Domain.Model.Entidades.Enums;
using Helpers.Commons.Exceptions;
using Helpers.ObjectsUtils.Extensions;

namespace Domain.Model.Entidades
{
    /// <summary>
    /// Clase Transacción
    /// </summary>
    public class Transaccion
    {

        /// <summary>
        /// Id de la Transacción
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// IdEmisor
        /// </summary>
        public string IdCuentaEmisora { get; set; }

        /// <summary>
        /// IdReceptor
        /// </summary>
        public string IdCuentaReceptora { get; set; }

        /// <summary>
        /// Tipo de Transacción realizada
        /// </summary>
        public TipoTransaccion TipoTransaccion { get; set; }

        /// <summary>
        /// valor de la transacción
        /// </summary>
        public float Valor { get; set; }

        /// <summary>
        /// Fecha del movimiento
        /// </summary>
        public DateTime FechaMovimiento { get; set; }

        /// <summary>
        /// Tipo de movimiento realizado
        /// </summary>
        public TipoMovimiento TipoMovimiento { get; set; }

        /// <summary>
        /// Validar valor
        /// </summary>
        /// <param name="valor"></param>
        /// <exception cref="BusinessException"></exception>
        public void ValidarValorTransaccion(float valor)
        {
            if (valor <= 0)
            {
                throw new BusinessException(TipoExcepcionNegocio.ExcepcionValorTransaccion.GetDescription(),
                     (int)TipoExcepcionNegocio.ExcepcionValorTransaccion);
            }
        }

        /// <summary>
        /// Validar estado cuenta emisora
        /// </summary>
        /// <param name="cuentaEmisora"></param>
        /// <exception cref="BusinessException"></exception>
        public void ValidarEmisorEstado(Cuenta cuentaEmisora)
        {
            if(cuentaEmisora.EstadoCuenta == EstadoCuenta.INACTIVA)
            {
                throw new BusinessException(TipoExcepcionNegocio.ExcepcionCuentaEmisoraInactiva.GetDescription(),
                     (int)TipoExcepcionNegocio.ExcepcionCuentaEmisoraInactiva);
            }
        }
    }
}