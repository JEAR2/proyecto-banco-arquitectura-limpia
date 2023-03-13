using credinet.exception.middleware.models;
using Domain.Model.Entidades;
using Domain.Model.Tests.Builders;
using Helpers.Commons.Exceptions;
using Xunit;

namespace Domain.Model.Tests.Entidades
{
    public class ClienteTest
    {
        [Fact]
        public void ValidarGMFExito()
        {
            List<Cuenta> cuentas = new List<Cuenta>();
            var cuenta = new CuentaBuilder().WithGMF(false).Build();
            var cuenta2 = new CuentaBuilder().WithGMF(false).Build();
            cuentas.Add(cuenta);
            cuentas.Add(cuenta2);
            var cliente = new ClienteBuilder().WithCuentas(cuentas).Build();
            cliente.ValidarGMF();
            Assert.False(cuenta.GMF);
            Assert.False(cuenta2.GMF);
        }

        [Fact]
        public void ValidarGMFError()
        {
            List<Cuenta> cuentas = new List<Cuenta>();
            var cuenta = new CuentaBuilder().WithGMF(false).Build();
            var cuenta2 = new CuentaBuilder().WithGMF(true).Build();
            cuentas.Add(cuenta);
            cuentas.Add(cuenta2);
            var cliente = new ClienteBuilder().WithCuentas(cuentas).Build();
            var error = Assert.Throws<BusinessException>(() => cliente.ValidarGMF());
            Assert.Equal((int)TipoExcepcionNegocio.ExceptionReglaaNegocio, error.code);
        }

        [Fact]
        public void ValidarCuentasCanceladasExitoso()
        {
            List<Cuenta> cuentas = new List<Cuenta>();
            var cuenta = new CuentaBuilder().WithEstadoCuenta(Model.Entidades.Enums.EstadoCuenta.CANCELADA).Build();
            var cuenta2 = new CuentaBuilder().WithEstadoCuenta(Model.Entidades.Enums.EstadoCuenta.CANCELADA).Build();
            cuentas.Add(cuenta);
            cuentas.Add(cuenta2);
            var cliente = new ClienteBuilder().WithCuentas(cuentas).Build();
            cliente.ValidarCuentasCanceladas();
            Assert.Equal(Model.Entidades.Enums.EstadoCuenta.CANCELADA, cuenta.EstadoCuenta);
            Assert.Equal(Model.Entidades.Enums.EstadoCuenta.CANCELADA, cuenta2.EstadoCuenta);
        }

        [Fact]
        public void ValidarCuentasCanceladasError()
        {
            List<Cuenta> cuentas = new List<Cuenta>();
            var cuenta = new CuentaBuilder().WithEstadoCuenta(Model.Entidades.Enums.EstadoCuenta.ACTIVA).Build();
            var cuenta2 = new CuentaBuilder().WithEstadoCuenta(Model.Entidades.Enums.EstadoCuenta.CANCELADA).Build();
            cuentas.Add(cuenta);
            cuentas.Add(cuenta2);
            var cliente = new ClienteBuilder().WithCuentas(cuentas).Build();
            var error = Assert.Throws<BusinessException>(() => cliente.ValidarCuentasCanceladas());
            Assert.Equal((int)TipoExcepcionNegocio.ExceptionReglaaNegocio, error.code);
        }
    }
}