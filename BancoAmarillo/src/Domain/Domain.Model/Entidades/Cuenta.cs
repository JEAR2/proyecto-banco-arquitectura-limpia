using System;
using System.Collections.Generic;
using credinet.exception.middleware.models;
using Domain.Model.Entidades.Enums;
using Helpers.Commons.Exceptions;

namespace Domain.Model.Entidades
{
    public class Cuenta
    {

        /// <summary>
        ///
        /// </summary>
        private const float MAXIMO_SOBREGIRO = 3000000;


        /// <summary>
        ///
        /// </summary>
        private const float IMPUESTO_RETIRO = (float)0.004;

        /// <summary>
        /// Id Cuenta
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Numero de Cuenta
        /// </summary>
        public string NumeroCuenta { get; set; }
        /// <summary>
        /// Id del Cliente
        /// </summary>
        public string IdCliente { get; set; }

        /// <summary>
        /// Tipo de cuenta
        /// </summary>
        public TipoCuenta TipoCuenta { get; set; }

        /// <summary>
        /// Estado de la cuenta
        /// </summary>
        public EstadoCuenta EstadoCuenta { get; set; }

        /// <summary>
        /// Saldo de la cuenta
        /// </summary>
        public float Saldo { get; set; }

        /// <summary>
        /// Saldo disponible
        /// </summary>
        public float SaldoDisponible { get; set; }

        /// <summary>
        /// Gravamen al Movimiento Financiero
        /// </summary>
        public bool GMF { get; set; }

        /// <summary>
        /// Fecha de Creación de la Cuenta
        /// </summary>
        public DateTime FechaCreacion { get; set; }

        /// <summary>
        /// Fecha de Modificación de la Cuenta
        /// </summary>
        public DateTime FechaModificacion { get; set; }

        /// <summary>
        /// Transacciones realizadas
        /// </summary>
        public List<Transaccion> Transacciones { get; set; }

        /// <summary>
        /// Regla de negocio
        /// </summary>
        /// <exception cref="BusinessException"></exception>
        public void ValidarMinimoAhorros()
        {
            if (TipoCuenta == TipoCuenta.AHORRO && Saldo < 0)
                throw new BusinessException("El saldo mínimo es 0", (int)TipoExcepcionNegocio.ExceptionReglaaNegocio);
        }

        /// <summary>
        /// Regla de negocio sobregiro
        /// </summary>
        /// <param name="valorSobregiro"></param>
        /// <exception cref="BusinessException"></exception>
        public void ValidarRetiro(float valorRetiro)
        {
            var impuestoRetiro = IMPUESTO_RETIRO;
            if (GMF)
                impuestoRetiro = (float)0;

            var maximoSobregiro = (Saldo + MAXIMO_SOBREGIRO) * ( 1 - impuestoRetiro);
            if (TipoCuenta == TipoCuenta.CORRIENTE && valorRetiro > maximoSobregiro)
                throw new BusinessException($"El máximo sobregiro para tu cuenta es de {maximoSobregiro}",
                    (int)TipoExcepcionNegocio.ExceptionReglaaNegocio);

            var maximoRetiro = (Saldo * (1 - impuestoRetiro));
            if (TipoCuenta == TipoCuenta.AHORRO && valorRetiro > maximoRetiro)
                throw new BusinessException($"No se pueden realizar sobregiros a la cuenta de ahorros, el saldo disponible es {SaldoDisponible} ",
                 (int)TipoExcepcionNegocio.ExceptionReglaaNegocio);
        }

        /// <summary>
        /// Realizar Consignación
        /// </summary>
        /// <param name="valorconSignacion"></param>
        /// <exception cref="BusinessException"></exception>
        public void RealizarConsignacion(float valorconSignacion)
        {
            if (valorconSignacion <= 0)
                throw new BusinessException($"El valor de Consignación no puede ser negativo",
                    (int)TipoExcepcionNegocio.ExceptionReglaaNegocio);

            Saldo = Saldo + valorconSignacion;
            ActualizarSaldoDisponible();

        }

        /// <summary>
        /// Realizar Retiro
        /// </summary>
        /// <param name="valorRetiro"></param>
        /// <exception cref="BusinessException"></exception>
        public void RealizarRetiro(float valorRetiro)
        {
            var impuestoRetiro = IMPUESTO_RETIRO;
            if (GMF)
                impuestoRetiro = (float)0;

            if (valorRetiro <= 0)
                throw new BusinessException($"El valor de Retiro no puede ser negativo",
                    (int)TipoExcepcionNegocio.ExceptionReglaaNegocio);

            ValidarRetiro(valorRetiro);
            Saldo = (float)(Saldo - (float)(valorRetiro * (1 + impuestoRetiro)));
            ActualizarSaldoDisponible();

        }

        /// <summary>
        /// Actualizar SaldoDisponible
        /// </summary>
        public void ActualizarSaldoDisponible()
        {
            var impuestoRetiro = IMPUESTO_RETIRO;
            if (GMF)
                impuestoRetiro = (float)0;

            if (TipoCuenta == TipoCuenta.AHORRO)
                SaldoDisponible = (float)(Saldo - (Saldo * impuestoRetiro));

            if (TipoCuenta == TipoCuenta.CORRIENTE)
                SaldoDisponible = (float)((Saldo + MAXIMO_SOBREGIRO) * (1 - impuestoRetiro));

        }

        /// <summary>
        /// Regla de negocio cancela cuenta
        /// </summary>
        /// <exception cref="BusinessException"></exception>
        public void CancelarCuenta()
        {
            ValidarEstado();
            if (Saldo < 0 || Saldo >= 1)
                throw new BusinessException($"La cuenta tiene un saldo de {Saldo}",
                      (int)TipoExcepcionNegocio.ExceptionReglaaNegocio);
            EstadoCuenta = EstadoCuenta.CANCELADA;
        }

        /// <summary>
        /// Validar Estado no es activo
        /// </summary>
        /// <exception cref="BusinessException"></exception>
        public void ValidarEstado()
        {
            if (EstadoCuenta != EstadoCuenta.ACTIVA)
                throw new BusinessException($"La cuenta se encuentra en estado {EstadoCuenta.ToString()}",
                   (int)TipoExcepcionNegocio.ExceptionReglaaNegocio);
        }

        /// <summary>
        /// Validar Estado no es Cancelada
        /// </summary>
        /// <exception cref="BusinessException"></exception>
        public void ValidarEstadoCancelada()
        {
            if (EstadoCuenta == EstadoCuenta.CANCELADA)
                throw new BusinessException($"La cuenta se encuentra Cancelada",
                   (int)TipoExcepcionNegocio.ExceptionReglaaNegocio);
        }


    }
}