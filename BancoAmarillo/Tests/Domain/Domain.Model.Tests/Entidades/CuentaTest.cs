using credinet.exception.middleware.models;
using Domain.Model.Entidades;
using Domain.Model.Entidades.Enums;
using Domain.Model.Tests.Builders;
using Helpers.Commons.Exceptions;
using System.Runtime.InteropServices;
using System.Threading;
using Xunit;

namespace Domain.Model.Tests.Entidades
{
    public class CuentaTest
    {
        [Theory]
        [InlineData(10)]
        [InlineData(30)]
        public void CrearCuentaExistosa(int monto)
        {
            var nuevaCuenta = new CuentaBuilder()
                .WithSaldoDisponible(monto)
                .WithTransacciones(new List<Transaccion>())
                .Build();

            nuevaCuenta.ValidarMinimoAhorros();

            Assert.Equal(monto, nuevaCuenta.SaldoDisponible);
        }

        [Fact]
        public void CraarCuentaerrorMontoMinimo()
        {
            var nuevaCuenta = new CuentaBuilder()
               .WithSaldoDisponible(-1)
               .WithSaldo(-1)
               .WithTipoCuenta(Model.Entidades.Enums.TipoCuenta.AHORRO)
               .Build();

            var error = Assert.Throws<BusinessException>(() => nuevaCuenta.ValidarMinimoAhorros());

            Assert.Equal((int)TipoExcepcionNegocio.ExceptionReglaaNegocio, error.code);
        }

        [Theory]
        [InlineData(997, TipoCuenta.AHORRO, false)]
        [InlineData(1000, TipoCuenta.AHORRO, false)]
        [InlineData(1001, TipoCuenta.AHORRO, true)]
        [InlineData(2988997, TipoCuenta.CORRIENTE, false)]
        [InlineData(3001001, TipoCuenta.CORRIENTE, true)]
        public void ValidarRetiroAhorrosRetornaError(float valorRetiro, TipoCuenta tipoCuenta, bool gMF)
        {
            var nuevaCuenta = new CuentaBuilder()
               .WithSaldo(1000)
               .WithTipoCuenta(tipoCuenta)
               .WithGMF(gMF)
               
               .Build();
            nuevaCuenta.ActualizarSaldoDisponible();

            Assert.Throws<BusinessException>(() => nuevaCuenta.ValidarRetiro(valorRetiro)); 

        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void RealizarConsignacionError(int monto)
        {
            var nuevaCuenta = new CuentaBuilder()
                .WithSaldo(0)
                .Build();

            Assert.Throws<BusinessException>(() => nuevaCuenta.RealizarConsignacion(monto)); 
 
        }
        [Theory]
        [InlineData(1)]
        [InlineData(3000)]
        public void RealizarConsignacion(int monto)
        {
            var nuevaCuenta = new CuentaBuilder()
                .WithSaldo(0)
                .Build();
            nuevaCuenta.RealizarConsignacion(monto);

            Assert.Equal(monto, nuevaCuenta.Saldo);

        }

        [Theory]
        [InlineData(997, TipoCuenta.AHORRO, false)]
        [InlineData(1001, TipoCuenta.AHORRO, true)]
        [InlineData(-1, TipoCuenta.AHORRO, false)]
        [InlineData(-1001, TipoCuenta.AHORRO, true)]
        public void RealizarRetiroError(float valorRetiro, TipoCuenta tipoCuenta, bool gMF)
        {
            var nuevaCuenta = new CuentaBuilder()
               .WithSaldo(1000)
               .WithTipoCuenta(tipoCuenta)
               .WithGMF(gMF)

               .Build();
            nuevaCuenta.ActualizarSaldoDisponible();

            Assert.Throws<BusinessException>(() => nuevaCuenta.RealizarRetiro(valorRetiro));

        }


        [Theory]
        [InlineData(996, TipoCuenta.AHORRO, false)]
        [InlineData(1000, TipoCuenta.AHORRO, true)]
        public void RealizarRetiro(float valorRetiro, TipoCuenta tipoCuenta, bool gMF)
        {
            var nuevaCuenta = new CuentaBuilder()
               .WithSaldo(1000)
               .WithTipoCuenta(tipoCuenta)
               .WithGMF(gMF)

               .Build();
            nuevaCuenta.RealizarRetiro(valorRetiro);

            Assert.Equal((int)0.0, (int)nuevaCuenta.Saldo);

        }


        [Theory]
        [InlineData(0.1, EstadoCuenta.ACTIVA)]
        [InlineData(0, EstadoCuenta.ACTIVA)]
        public void CancelarCuenta(float saldo, EstadoCuenta estadoCuenta)
        {
            var nuevaCuenta = new CuentaBuilder()
               .WithSaldo(saldo)
               .WithEstadoCuenta(estadoCuenta)

               .Build();
            nuevaCuenta.CancelarCuenta();

            Assert.Equal(nuevaCuenta.EstadoCuenta, EstadoCuenta.CANCELADA);

        }


        [Theory]
        [InlineData(-1000, EstadoCuenta.ACTIVA)]
        [InlineData(1000, EstadoCuenta.ACTIVA)]
        [InlineData(0, EstadoCuenta.INACTIVA)]
        public void CancelarCuentaError(float saldo, EstadoCuenta estadoCuenta)
        {
            var nuevaCuenta = new CuentaBuilder()
               .WithSaldo(saldo)
               .WithEstadoCuenta(estadoCuenta)

               .Build();

            Assert.Throws<BusinessException>(() => nuevaCuenta.CancelarCuenta());

        }


        [Theory]
        [InlineData( EstadoCuenta.CANCELADA)]
        public void ValidarEstadoCanceladaError( EstadoCuenta estadoCuenta)
        {
            var nuevaCuenta = new CuentaBuilder()
               .WithEstadoCuenta(estadoCuenta)

               .Build();

            Assert.Throws<BusinessException>(() => nuevaCuenta.ValidarEstadoCancelada());

        }



    }
}