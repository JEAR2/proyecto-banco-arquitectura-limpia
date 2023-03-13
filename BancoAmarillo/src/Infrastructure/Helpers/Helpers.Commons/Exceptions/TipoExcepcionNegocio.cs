using System.ComponentModel;

namespace Helpers.Commons.Exceptions
{
    /// <summary>
    /// ResponseError
    /// </summary>
    public enum TipoExcepcionNegocio
    {
        /// <summary>
        /// Tipo de exception no controlada
        /// </summary>
        [Description("Excepción de negocio no controlada")]
        ExceptionNoControlada = 555,

        /// <summary>
        /// No autorizado
        /// </summary>
        [Description("No autorizado")]
        ExceptioNoAutorizado = 556,

        /// <summary>
        /// Tipo de exception no controlada
        /// </summary>
        [Description("Cliente no encontrado")]
        ExceptionClienteNoEncontrado = 557,

        /// <summary>
        /// Tipo de exception no controlada
        /// </summary>
        [Description("Error en modelos")]
        ExceptionErrorEnModelo = 558,

        /// <summary>
        /// Tipo de exception no controlada
        /// </summary>
        [Description("Error en regla de negocio")]
        ExceptionReglaaNegocio = 559,


        /// <summary>
        /// Solo se permiten cuentas de ahorro o corrient
        /// </summary>
        [Description("Solo se permiten cuentas de ahorro o corriente")]
        ExceptioTipoDeCuentaInvalido = 560,

        /// <summary>
        /// Exception Saldo menor igual Cero Ahorros
        /// </summary>
        [Description("La cuenta de ahorros no debe de tener un saldo mayor o igual a Cero.")]
        ExceptionSaldoCeroAhorros = 561,

        /// <summary>
        /// Exception Saldo menor igual Cero Ahorros
        /// </summary>
        [Description("La cuenta con este Id no existe")]
        ExceptionCuentaNoExiste = 562,

        /// <summary>
        /// Exception Saldo menor igual Cero Ahorros
        /// </summary>
        [Description("La cuenta con este Id ya esta cancelada")]
        ExceptionCuentaCancelada = 563,

        /// Exception El Cliente no Existe
        /// </summary>
        [Description("El Cliente no Existe")]
        ExceptionClienteNoExiste = 564,

        /// <summary>
        /// Exception El Cliente ya tiene una cuenta marcada como GMF
        /// </summary>
        [Description("El Cliente ya tiene una cuenta marcada como GMF")]
        ExceptionClienteYaTieneGMF = 565,

        /// <summary>
        /// Exception "La cuenta con este Id ya esta Activa
        /// </summary>
        [Description("La cuenta con este Id ya esta Activa")]
        ExceptionCuentaActiva = 566,


        /// <summary>
        /// Exception La cuenta con este Id ya esta Inactiva
        /// </summary>
        [Description("La cuenta con este Id ya esta Inactiva")]
        ExceptionCuentaInactiva = 567,

        /// <summary>
        /// Excepción transacción invalida
        /// </summary>
        [Description("La transacción es de un tipo inválida")]
        ExcepcionTransaccionInvalida = 580,

        /// <summary>
        /// Excepción La cuenta emisora no existe
        /// </summary>
        [Description("La cuenta emisora no existe")]
        ExcepcionTransaccionCuentaEmisoraNoExiste = 581,

        /// <summary>
        /// Excepción La cuenta receptora no existe
        /// </summary>
        [Description("La cuenta emisora no existe")]
        ExcepcionTransaccionCuentaReceptoraNoExiste = 582,

        /// <summary>
        /// Excepción La cuenta receptora no existe
        /// </summary>
        [Description("El valor de la transacción debe ser mayor a cero")]
        ExcepcionValorTransaccion = 583,

        /// <summary>
        /// Excepción La cuenta emisora está inactiva
        /// </summary>
        [Description("La cuenta emisora debe estar activa para el envio de dinero")]
        ExcepcionCuentaEmisoraInactiva = 584,

        /// <summary>
        /// La transaccion no existe
        /// </summary>
        [Description("La transaccion no existe")]
        ExcepcionTransaccionNoExiste = 585,
    }

}