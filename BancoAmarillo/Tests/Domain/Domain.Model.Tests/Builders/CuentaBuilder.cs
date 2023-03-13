using Domain.Model.Entidades;
using Domain.Model.Entidades.Enums;

namespace Domain.Model.Tests.Builders
{
    public class CuentaBuilder
    {
        private string _id;
        private string _idCliente;
        private string _numeroCuenta;
        private TipoCuenta _tipoCuenta;
        private EstadoCuenta _estadoCuenta;
        private float _saldo;
        private float _saldoDisponible;
        private bool _gmf;
        private DateTime _fechaCreacion;
        private DateTime _fechaModificacion;
        private List<Transaccion> _transacciones;

        public CuentaBuilder()
        {
            // Configurar los valores predeterminados para los campos opcionales.
            _gmf = false;
            _fechaCreacion = DateTime.Now;
            _fechaModificacion = DateTime.Now;
            _transacciones = new List<Transaccion>();
        }

        public CuentaBuilder WithId(string id)
        {
            _id = id;
            return this;
        }

        public CuentaBuilder WithIdCliente(string idCliente)
        {
            _idCliente = idCliente;
            return this;
        }

        public CuentaBuilder WithTipoCuenta(TipoCuenta tipoCuenta)
        {
            _tipoCuenta = tipoCuenta;
            return this;
        }

        public CuentaBuilder WithEstadoCuenta(EstadoCuenta estadoCuenta)
        {
            _estadoCuenta = estadoCuenta;
            return this;
        }

        public CuentaBuilder WithSaldo(float saldo)
        {
            _saldo = saldo;
            return this;
        }

        public CuentaBuilder WithSaldoDisponible(float saldoDisponible)
        {
            _saldoDisponible = saldoDisponible;
            return this;
        }

        public CuentaBuilder WithGMF(bool gmf)
        {
            _gmf = gmf;
            return this;
        }

        public CuentaBuilder WithFechaCreacion(DateTime fechaCreacion)
        {
            _fechaCreacion = fechaCreacion;
            return this;
        }

        public CuentaBuilder WithFechaModificacion(DateTime fechaModificacion)
        {
            _fechaModificacion = fechaModificacion;
            return this;
        }
        public CuentaBuilder WithTransacciones(List<Transaccion> transacciones)
        {
            _transacciones = transacciones;
            return this;
        }


            public Cuenta Build()

            => new Cuenta()
            {
                Id = _id,
                IdCliente = _idCliente,
                NumeroCuenta = _numeroCuenta,
                TipoCuenta = _tipoCuenta,
                EstadoCuenta = _estadoCuenta,
                Saldo = _saldo,
                SaldoDisponible = _saldoDisponible,
                GMF = _gmf,
                FechaCreacion = _fechaCreacion,
                FechaModificacion = _fechaModificacion,
                Transacciones = _transacciones
            };
    }
}