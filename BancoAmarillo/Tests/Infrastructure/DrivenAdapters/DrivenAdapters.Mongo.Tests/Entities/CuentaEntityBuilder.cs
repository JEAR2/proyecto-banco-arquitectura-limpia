using Domain.Model.Entidades;
using Domain.Model.Entidades.Enums;
using Domain.Model.Tests.Builders;
using DrivenAdapters.Mongo.Entities;

namespace DrivenAdapters.Mongo.Tests.Entities
{
    public class CuentaEntityBuilder
    {
        private string _id;
        private string _numeroCuenta;
        private string _idCliente;
        private TipoCuenta _tipoCuenta;
        private EstadoCuenta _estadoCuenta;
        private float _saldo;
        private float _saldoDisponible;
        private bool _gmf;
        private DateTime _fechaCreacion;
        private DateTime _fechaModificacion;
        private List<TransaccionEntity> _transacciones;

        public CuentaEntityBuilder WithId(string id)
        {
            _id = id;
            return this;
        }

        public CuentaEntityBuilder WithNumeroCuenta(string numeroCuenta)
        {
            _numeroCuenta = numeroCuenta;
            return this;
        }

        public CuentaEntityBuilder WithIdCliente(string idCliente)
        {
            _idCliente = idCliente;
            return this;
        }

        public CuentaEntityBuilder WithTipoCuenta(TipoCuenta tipoCuenta)
        {
            _tipoCuenta = tipoCuenta;
            return this;
        }

        public CuentaEntityBuilder WithEstadoCuenta(EstadoCuenta estadoCuenta)
        {
            _estadoCuenta = estadoCuenta;
            return this;
        }

        public CuentaEntityBuilder WithSaldo(float saldo)
        {
            _saldo = saldo;
            return this;
        }

        public CuentaEntityBuilder WithSaldoDisponible(float saldoDisponible)
        {
            _saldoDisponible = saldoDisponible;
            return this;
        }

        public CuentaEntityBuilder WithGMF(bool gmf)
        {
            _gmf = gmf;
            return this;
        }

        public CuentaEntityBuilder WithFechaCreacion(DateTime fechaCreacion)
        {
            _fechaCreacion = fechaCreacion;
            return this;
        }

        public CuentaEntityBuilder WithFechaModificacion(DateTime fechaModificacion)
        {
            _fechaModificacion = fechaModificacion;
            return this;
        }

        public CuentaEntityBuilder WithTransacciones(List<TransaccionEntity> transacciones)
        {
            _transacciones = transacciones;
            return this;
        }

        public CuentaEntity Build()
        {
            return new CuentaEntity
            {
                Id = _id,
                NumeroCuenta = _numeroCuenta,
                IdCliente = _idCliente,
                TipoCuenta = (Mongo.Entities.Enums.TipoCuenta)_tipoCuenta,
                EstadoCuenta = (Mongo.Entities.Enums.EstadoCuenta)_estadoCuenta,
                Saldo = _saldo,
                SaldoDisponible = _saldoDisponible,
                GMF = _gmf,
                FechaCreacion = _fechaCreacion,
                FechaModificacion = _fechaModificacion,
                Transacciones = _transacciones
            };
        }
    }
}