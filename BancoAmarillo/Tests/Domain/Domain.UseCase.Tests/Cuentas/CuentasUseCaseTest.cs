using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using credinet.comun.models.Credits;
using credinet.exception.middleware.models;
using Domain.Model.Entidades;
using Domain.Model.Entidades.Enums;
using Domain.Model.Gateway;
using Domain.UseCase.Clientes;
using Domain.UseCase.Cuentas;
using Domain.UseCase.Tests.Builders;
using Moq;
using Xunit;

namespace Domain.UseCase.Tests.Cuentas
{
    public class CuentasUseCaseTest
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<ICuentaRepository> _mockCuentaRepository;

        private readonly Mock<IClienteRepository> _mockClienteRepository;

        private readonly CuentasUseCase _cuentaUseCase;

        public CuentasUseCaseTest()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _mockCuentaRepository = new();
            _mockClienteRepository = new();
            _cuentaUseCase = new(_mockCuentaRepository.Object,
                _mockClienteRepository.Object);
        }

        [Theory]
        [InlineData(true, "640204ce5a09158e24ac91dd", 0, TipoCuenta.AHORRO)]
        public async Task CrearCuentaErrorCliente(bool gmf, string idCliente, float saldo, TipoCuenta tipocuenta)
        {
            Cuenta nuevaCuenta = new CuentaBuilder()
            .WithIdCliente(idCliente)
            .WithGMF(gmf)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo)
            .Build();

            Cuenta nuevaCuentaAgregada = new CuentaBuilder()
            .WithIdCliente(idCliente)
            .WithGMF(gmf)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo)
            .WithId("640215246944b39b314ec67d")
            .WithNumeroCuenta("2300000000")
            .WithFechaModificacion(DateTime.Now)
            .WithFechaCreacion(DateTime.Now)
            .Build();


            Cliente clienteNullo = null;

            _mockCuentaRepository
                .Setup(repository => repository.CrearCuentaAsync(It.IsAny<Cuenta>()))
                .ReturnsAsync(nuevaCuentaAgregada);

            _mockCuentaRepository
                .Setup(repository => repository.ObtenerCountCuentasTipoAsync(tipocuenta))
                .ReturnsAsync(0);

            _mockClienteRepository
                .Setup(repository => repository.ObtenerClientePorId(It.IsAny<string>()))
            .ReturnsAsync(clienteNullo);


            var error = Assert.ThrowsAsync<BusinessException>(() => _cuentaUseCase.CrearCuentaAsync(nuevaCuenta));
        }

        [Theory]
        [InlineData(false, "640204ce5a09158e24ac91dd", 0, TipoCuenta.CORRIENTE)]
        [InlineData(false, "640204ce5a09158e24ac91dd", 0, TipoCuenta.AHORRO)]
        [InlineData(true, "640204ce5a09158e24ac91dd", 0, TipoCuenta.AHORRO)]
        public async Task CrearCuentaExitoso(bool gmf, string idCliente, float saldo, TipoCuenta tipocuenta)
        {
            Cuenta nuevaCuenta = new CuentaBuilder()
            .WithIdCliente(idCliente)
            .WithGMF(gmf)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo)
            .Build();

            Cuenta nuevaCuentaAgregada = new CuentaBuilder()
            .WithIdCliente(idCliente)
            .WithGMF(gmf)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo)
            .WithId("640215246944b39b314ec67d")
            .WithNumeroCuenta("2300000000")
            .WithFechaModificacion(DateTime.Now)
            .WithFechaCreacion(DateTime.Now)
            .Build();

            _mockCuentaRepository
                .Setup(repository => repository.CrearCuentaAsync(It.IsAny<Cuenta>()))
                .ReturnsAsync(nuevaCuentaAgregada);

            _mockCuentaRepository
                .Setup(repository => repository.ObtenerCountCuentasTipoAsync(tipocuenta))
                .ReturnsAsync(0);

            _mockClienteRepository
                .Setup(repository => repository.ObtenerClientePorId(It.IsAny<string>()))
            .ReturnsAsync(
                new ClienteBuilder()
                .WithEstado(EstadoCliente.ACTIVO)

                .WithId(idCliente).Build());


            Cuenta cuentaCreada = await _cuentaUseCase.CrearCuentaAsync(nuevaCuenta);



            _mockRepository.VerifyAll();

            Assert.NotNull(cuentaCreada);
            Assert.NotNull(cuentaCreada.Id);
        }

        [Theory]
        [InlineData(true, "640204ce5a09158e24ac91dd", 0, TipoCuenta.AHORRO)]
        public async Task CrearCuentaErrorGmf(bool gmf, string idCliente, float saldo, TipoCuenta tipocuenta)
        {
            Cuenta nuevaCuenta = new CuentaBuilder()
            .WithIdCliente(idCliente)
            .WithGMF(gmf)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo)
            .Build();

            Cuenta nuevaCuentaAgregada = new CuentaBuilder()
            .WithIdCliente(idCliente)
            .WithGMF(gmf)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo)
            .WithId("640215246944b39b314ec67d")
            .WithNumeroCuenta("2300000000")
            .WithFechaModificacion(DateTime.Now)
            .WithFechaCreacion(DateTime.Now)
            .Build();

            _mockCuentaRepository
                .Setup(repository => repository.CrearCuentaAsync(It.IsAny<Cuenta>()))
                .ReturnsAsync(nuevaCuentaAgregada);

            _mockCuentaRepository
                .Setup(repository => repository.ObtenerCountCuentasTipoAsync(tipocuenta))
                .ReturnsAsync(0);

            _mockClienteRepository
                .Setup(repository => repository.ObtenerClientePorId(It.IsAny<string>()))
            .ReturnsAsync(
                new ClienteBuilder()
                .WithEstado(EstadoCliente.ACTIVO)
                .WithId(idCliente).Build());


            _mockCuentaRepository
                .Setup(repository => repository.ObtenerCuentasPorIdClienteAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<Cuenta>
                {
                    nuevaCuentaAgregada
                });
            
            var error = Assert.ThrowsAsync<BusinessException>(() => _cuentaUseCase.CrearCuentaAsync(nuevaCuenta));

        }

        [Theory]
        [InlineData(false, "640204ce5a09158e24ac91dd", 0, TipoCuenta.CORRIENTE)]
        [InlineData(false, "640204ce5a09158e24ac91dd", 0, TipoCuenta.AHORRO)]
        [InlineData(true, "640204ce5a09158e24ac91dd", 0, TipoCuenta.AHORRO)]
        public async Task ActualizarCuentaExitoso(bool gmf, string idCliente, float saldo, TipoCuenta tipocuenta)
        {


            Cuenta nuevaCuentaAgregada = new CuentaBuilder()
            .WithIdCliente(idCliente)
            .WithGMF(gmf)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo)
            .WithId("640215246944b39b314ec67d")
            .WithNumeroCuenta("2300000000")
            .WithFechaModificacion(DateTime.Now)
            .WithFechaCreacion(DateTime.Now)
            .Build();

            Cuenta nuevaCuentaActualizada = new CuentaBuilder()
            .WithIdCliente(idCliente)
            .WithGMF(gmf)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo + 1000)
            .WithId("640215246944b39b314ec67d")
            .WithNumeroCuenta("2300000000")
            .WithFechaModificacion(DateTime.Now)
            .WithFechaCreacion(DateTime.Now)

            .Build();


            var returnList = new List<Cuenta>();

            returnList.Add(nuevaCuentaAgregada);

            _mockCuentaRepository
                .Setup(repository => repository.ActualizarCuentaAsync(It.IsAny<Cuenta>()))
                .ReturnsAsync(nuevaCuentaActualizada);

            _mockCuentaRepository
                .Setup(repository => repository.ObtenerCuentaPorIdAsync(It.IsAny<string>()))
                .ReturnsAsync(nuevaCuentaAgregada);
         
            _mockClienteRepository
                .Setup(repository => repository.ObtenerClientePorId(It.IsAny<string>()))
            .ReturnsAsync(
                new ClienteBuilder()
                .WithEstado(EstadoCliente.ACTIVO)
                .WithCuentas(returnList)
                .WithId(idCliente).Build());

            Cuenta Actualizada = await _cuentaUseCase.ActualizarCuentaAsync(nuevaCuentaActualizada);

            _mockRepository.VerifyAll();

            Assert.NotNull(Actualizada);
            Assert.Equal(Actualizada.Saldo,(saldo + 1000));
        }

        [Theory]
        [InlineData(false, "640204ce5a09158e24ac91dd", 0, TipoCuenta.CORRIENTE)]
        [InlineData(false, "640204ce5a09158e24ac91dd", 0, TipoCuenta.AHORRO)]
        [InlineData(true, "640204ce5a09158e24ac91dd", 0, TipoCuenta.AHORRO)]
        public async Task ActualizarCuentaErrorCliente(bool gmf, string idCliente, float saldo, TipoCuenta tipocuenta)
        {


            Cuenta nuevaCuentaAgregada = new CuentaBuilder()
            .WithIdCliente(idCliente)
            .WithGMF(gmf)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo)
            .WithId("640215246944b39b314ec67d")
            .WithNumeroCuenta("2300000000")
            .WithFechaModificacion(DateTime.Now)
            .WithFechaCreacion(DateTime.Now)
            .Build();

            Cuenta nuevaCuentaActualizada = new CuentaBuilder()
            .WithIdCliente(idCliente)
            .WithGMF(gmf)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo + 1000)
            .WithId("640215246944b39b314ec67d")
            .WithNumeroCuenta("2300000000")
            .WithFechaModificacion(DateTime.Now)
            .WithFechaCreacion(DateTime.Now)

            .Build();

            Cliente cliente = null;

            _mockCuentaRepository
                .Setup(repository => repository.ActualizarCuentaAsync(It.IsAny<Cuenta>()))
                .ReturnsAsync(nuevaCuentaActualizada);

            _mockCuentaRepository
                .Setup(repository => repository.ObtenerCuentaPorIdAsync(It.IsAny<string>()))
                .ReturnsAsync(nuevaCuentaAgregada);

            _mockClienteRepository
                .Setup(repository => repository.ObtenerClientePorId(It.IsAny<string>()))
            .ReturnsAsync(cliente);

            var error = Assert.ThrowsAsync<BusinessException>(() => _cuentaUseCase.ActualizarCuentaAsync(nuevaCuentaActualizada));
        }
        [Theory]
        [InlineData(false, "640204ce5a09158e24ac91dd", 0, TipoCuenta.CORRIENTE)]
        [InlineData(false, "640204ce5a09158e24ac91dd", 0, TipoCuenta.AHORRO)]
        [InlineData(true, "640204ce5a09158e24ac91dd", 0, TipoCuenta.AHORRO)]
        public async Task ActualizarCuentaErrorCuenta(bool gmf, string idCliente, float saldo, TipoCuenta tipocuenta)
        {


            Cuenta nuevaCuentaAgregada = null;


            Cuenta nuevaCuentaActualizada = new CuentaBuilder()
            .WithIdCliente(idCliente)
            .WithGMF(gmf)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo + 1000)
            .WithId("640215246944b39b314ec67d")
            .WithNumeroCuenta("2300000000")
            .WithFechaModificacion(DateTime.Now)
            .WithFechaCreacion(DateTime.Now)

            .Build();


            var returnList = new List<Cuenta>();

            returnList.Add(nuevaCuentaAgregada);

            Cliente cliente = null;

            _mockCuentaRepository
                .Setup(repository => repository.ActualizarCuentaAsync(It.IsAny<Cuenta>()))
                .ReturnsAsync(nuevaCuentaActualizada);

            _mockCuentaRepository
                .Setup(repository => repository.ObtenerCuentaPorIdAsync(It.IsAny<string>()))
                .ReturnsAsync(nuevaCuentaAgregada);

            _mockClienteRepository
                .Setup(repository => repository.ObtenerClientePorId(It.IsAny<string>()))
            .ReturnsAsync(cliente);

            var error = Assert.ThrowsAsync<BusinessException>(() => _cuentaUseCase.ActualizarCuentaAsync(nuevaCuentaActualizada));
        }

        [Theory]
        [InlineData(true, "640215246944b39b314ec67d", 100000, TipoCuenta.CORRIENTE, "640215246944b39b314ec67d", "640227e4cc5367411693ac3e", TipoTransaccion.TRANSFERENCIA, 1000, TipoMovimiento.CREDITO, 101000)]
        [InlineData(true, "640215246944b39b314ec67d", 100000, TipoCuenta.CORRIENTE, "640215246944b39b314ec67d", "640227e4cc5367411693ac3e", TipoTransaccion.TRANSFERENCIA, 1000, TipoMovimiento.DEBITO, 99000)]

        public async Task AgregarTransaccionExitoso(bool gmf, string idCliente, float saldo, TipoCuenta tipocuenta, string cuentaReceptora, string cuentaEmisora, TipoTransaccion tipoTransaccion, float montoTransaccion, TipoMovimiento tipoMovimiento, float esperado)
        {

            Cuenta cuentaEncontrada = new CuentaBuilder()
            .WithIdCliente("640215246944b39b314ec87d")
            .WithGMF(gmf)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo)
            .WithId("640215246944b39b314ec67d")
            .WithNumeroCuenta("2300000000")
            .WithFechaModificacion(DateTime.Now)
            .WithFechaCreacion(DateTime.Now)
                        .WithEstadoCuenta(EstadoCuenta.ACTIVA)
            .Build();

            Transaccion nuevaTransaccion = new TransaccionBuilder()
                .WithIdCuentaEmisora(cuentaEmisora)
                .WithIdCuentaReceptora(cuentaReceptora)
                .WithTipoTransaccion(tipoTransaccion)
                .WithValor(montoTransaccion)
                .WithTipoMovimiento(tipoMovimiento)
                .Build();

            var returnList = new List<Cuenta>();

            returnList.Add(cuentaEncontrada);

            _mockCuentaRepository
                .Setup(repository => repository.ActualizarCuentaAsync(It.IsAny<Cuenta>()))
                .ReturnsAsync(cuentaEncontrada);

            _mockCuentaRepository
                .Setup(repository => repository.ObtenerCuentaPorIdAsync(It.IsAny<string>()))
                .ReturnsAsync(cuentaEncontrada);

            _mockClienteRepository
                .Setup(repository => repository.ObtenerClientePorId(It.IsAny<string>()))
            .ReturnsAsync(
                new ClienteBuilder()
                .WithEstado(EstadoCliente.ACTIVO)
                .WithCuentas(returnList)
                .WithId(idCliente)
                .Build());

            Cuenta Actualizada = await _cuentaUseCase.AgregarTransaccionCuentaAsync(nuevaTransaccion, cuentaEncontrada.Id);

            _mockRepository.VerifyAll();

            Assert.NotNull(Actualizada);
            Assert.Equal(Actualizada.Saldo, esperado);
        }
        [Theory]
        [InlineData(true, "640215246944b39b314ec67d", 100000, TipoCuenta.CORRIENTE, "640215246944b39b314ec67d", "640227e4cc5367411693ac3e", TipoTransaccion.TRANSFERENCIA, 1000, TipoMovimiento.CREDITO, 101000)]
        [InlineData(true, "640215246944b39b314ec67d", 100000, TipoCuenta.CORRIENTE, "640215246944b39b314ec67d", "640227e4cc5367411693ac3e", TipoTransaccion.TRANSFERENCIA, 1000, TipoMovimiento.DEBITO, 99000)]

        public async Task AgregarTransaccionErrorCliente(bool gmf, string idCliente, float saldo, TipoCuenta tipocuenta, string cuentaReceptora, string cuentaEmisora, TipoTransaccion tipoTransaccion, float montoTransaccion, TipoMovimiento tipoMovimiento, float esperado)
        {

            Cuenta cuentaEncontrada = new CuentaBuilder()
            .WithIdCliente("640215246944b39b314ec87d")
            .WithGMF(gmf)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo)
            .WithId("640215246944b39b314ec67d")
            .WithNumeroCuenta("2300000000")
            .WithFechaModificacion(DateTime.Now)
            .WithFechaCreacion(DateTime.Now)
                        .WithEstadoCuenta(EstadoCuenta.ACTIVA)
            .Build();

            Transaccion nuevaTransaccion = new TransaccionBuilder()
                .WithIdCuentaEmisora(cuentaEmisora)
                .WithIdCuentaReceptora(cuentaReceptora)
                .WithTipoTransaccion(tipoTransaccion)
                .WithValor(montoTransaccion)
                .WithTipoMovimiento(tipoMovimiento)
                .Build();

            Cliente cliente = null;

            _mockCuentaRepository
                .Setup(repository => repository.ActualizarCuentaAsync(It.IsAny<Cuenta>()))
                .ReturnsAsync(cuentaEncontrada);

            _mockCuentaRepository
                .Setup(repository => repository.ObtenerCuentaPorIdAsync(It.IsAny<string>()))
                .ReturnsAsync(cuentaEncontrada);

            _mockClienteRepository
                .Setup(repository => repository.ObtenerClientePorId(It.IsAny<string>()))
            .ReturnsAsync(cliente);

            var error = Assert.ThrowsAsync<BusinessException>(() => _cuentaUseCase.AgregarTransaccionCuentaAsync(nuevaTransaccion, cuentaEncontrada.Id));

        }

        [Theory]
        [InlineData(true, "640215246944b39b314ec67d", 100000, TipoCuenta.CORRIENTE, "640215246944b39b314ec67d", "640227e4cc5367411693ac3e", TipoTransaccion.TRANSFERENCIA, 1000, TipoMovimiento.CREDITO, 101000)]
        [InlineData(true, "640215246944b39b314ec67d", 100000, TipoCuenta.CORRIENTE, "640215246944b39b314ec67d", "640227e4cc5367411693ac3e", TipoTransaccion.TRANSFERENCIA, 1000, TipoMovimiento.DEBITO, 99000)]

        public async Task AgregarTransaccionErrorCuenta(bool gmf, string idCliente, float saldo, TipoCuenta tipocuenta, string cuentaReceptora, string cuentaEmisora, TipoTransaccion tipoTransaccion, float montoTransaccion, TipoMovimiento tipoMovimiento, float esperado)
        {

            Cuenta cuentaEncontrada = null;

            Transaccion nuevaTransaccion = new TransaccionBuilder()
                .WithIdCuentaEmisora(cuentaEmisora)
                .WithIdCuentaReceptora(cuentaReceptora)
                .WithTipoTransaccion(tipoTransaccion)
                .WithValor(montoTransaccion)
                .WithTipoMovimiento(tipoMovimiento)
                .Build();

            var returnList = new List<Cuenta>();

            returnList.Add(cuentaEncontrada);

            _mockCuentaRepository
                .Setup(repository => repository.ActualizarCuentaAsync(It.IsAny<Cuenta>()))
                .ReturnsAsync(cuentaEncontrada);

            _mockCuentaRepository
                .Setup(repository => repository.ObtenerCuentaPorIdAsync(It.IsAny<string>()))
                .ReturnsAsync(cuentaEncontrada);

            _mockClienteRepository
                .Setup(repository => repository.ObtenerClientePorId(It.IsAny<string>()))
            .ReturnsAsync(
                new ClienteBuilder()
                .WithEstado(EstadoCliente.ACTIVO)
                .WithCuentas(returnList)
                .WithId(idCliente)
                .Build());

            var error = Assert.ThrowsAsync<BusinessException>(() => _cuentaUseCase.AgregarTransaccionCuentaAsync(nuevaTransaccion, It.IsAny<string>()));

        }

        [Theory]
        [InlineData("640204ce5a09158e24ac91dd", false, "640204ce5a09158e24ac91dd", 0, TipoCuenta.CORRIENTE)]
        public async Task CancelarCuentaExitoso(string idCuetna, bool gmf, string idCliente, float saldo, TipoCuenta tipocuenta)
        {

            Cuenta nuevaCuentaAgregada = new CuentaBuilder()
            .WithId(idCuetna)
            .WithIdCliente(idCliente)
            .WithGMF(gmf)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo)
            .WithNumeroCuenta("2300000000")
            .WithFechaModificacion(DateTime.Now)
                        .WithEstadoCuenta(EstadoCuenta.ACTIVA)
            .WithFechaCreacion(DateTime.Now)

            .Build();
            Cuenta nuevaCuentaActualizada = new CuentaBuilder()
            .WithId(idCuetna)
            .WithIdCliente(idCliente)
            .WithGMF(gmf)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo)
            .WithNumeroCuenta("2300000000")
            .WithFechaModificacion(DateTime.Now)
            .WithFechaCreacion(DateTime.Now)
            .WithEstadoCuenta(EstadoCuenta.ACTIVA)
            .Build();

            var returnList = new List<Cuenta>();

            returnList.Add(nuevaCuentaAgregada);

            _mockCuentaRepository
                .Setup(repository => repository.ActualizarCuentaAsync(It.IsAny<Cuenta>()))
                .ReturnsAsync(nuevaCuentaActualizada);

            _mockCuentaRepository
                .Setup(repository => repository.ObtenerCuentaPorIdAsync(It.IsAny<string>()))
                .ReturnsAsync(nuevaCuentaAgregada);

            _mockClienteRepository
                .Setup(repository => repository.ObtenerClientePorId(It.IsAny<string>()))
            .ReturnsAsync(
                new ClienteBuilder()
                .WithEstado(EstadoCliente.ACTIVO)
                .WithCuentas(returnList)
                .WithId(idCliente).Build());

           var estado =  await _cuentaUseCase.CancelarCuentaAsync(idCuetna);

            _mockRepository.VerifyAll();

            Assert.Equal(estado, true);
        }

        [Theory]
        [InlineData("640204ce5a09158e24ac91dd", false, "640204ce5a09158e24ac91dd", 0, TipoCuenta.CORRIENTE)]
        public async Task CancelarCuentaErrorCliente(string idCuetna, bool gmf, string idCliente, float saldo, TipoCuenta tipocuenta)
        {

            Cuenta nuevaCuentaAgregada = new CuentaBuilder()
            .WithId(idCuetna)
            .WithIdCliente(idCliente)
            .WithGMF(gmf)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo + 1000)
            .WithNumeroCuenta("2300000000")
            .WithFechaModificacion(DateTime.Now)
            .WithFechaCreacion(DateTime.Now)

            .Build();
            Cuenta nuevaCuentaActualizada = new CuentaBuilder()
            .WithId(idCuetna)
            .WithIdCliente(idCliente)
            .WithGMF(gmf)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo + 1000)
            .WithNumeroCuenta("2300000000")
            .WithFechaModificacion(DateTime.Now)
            .WithFechaCreacion(DateTime.Now)
            .WithEstadoCuenta(EstadoCuenta.ACTIVA)
            .Build();

            Cliente clienteNullo = null;

            _mockCuentaRepository
                .Setup(repository => repository.ActualizarCuentaAsync(It.IsAny<Cuenta>()))
                .ReturnsAsync(nuevaCuentaActualizada);

            _mockCuentaRepository
                .Setup(repository => repository.ObtenerCuentaPorIdAsync(It.IsAny<string>()))
                .ReturnsAsync(nuevaCuentaAgregada);

            _mockClienteRepository
                .Setup(repository => repository.ObtenerClientePorId(It.IsAny<string>()))
            .ReturnsAsync(clienteNullo);

            var error = Assert.ThrowsAsync<BusinessException>(() => _cuentaUseCase.CancelarCuentaAsync(idCuetna));
        }
        [Theory]
        [InlineData("640204ce5a09158e24ac91dd", false, "640204ce5a09158e24ac91dd", 0, TipoCuenta.CORRIENTE)]
        public async Task CancelarCuentaErrorCuenta(string idCuetna, bool gmf, string idCliente, float saldo, TipoCuenta tipocuenta)
        {

            Cuenta nuevaCuentaAgregada = null;

            Cuenta nuevaCuentaActualizada = null;

            Cliente clienteNullo = null;

            _mockCuentaRepository
                .Setup(repository => repository.ActualizarCuentaAsync(It.IsAny<Cuenta>()))
                .ReturnsAsync(nuevaCuentaActualizada);

            _mockCuentaRepository
                .Setup(repository => repository.ObtenerCuentaPorIdAsync(It.IsAny<string>()))
                .ReturnsAsync(nuevaCuentaAgregada);

            _mockClienteRepository
                .Setup(repository => repository.ObtenerClientePorId(It.IsAny<string>()))
            .ReturnsAsync(clienteNullo);

            var error = Assert.ThrowsAsync<BusinessException>(() => _cuentaUseCase.CancelarCuentaAsync(idCuetna));
        }

        [Theory]
        [InlineData("640204ce5a09158e24ac91dd", false, "640204ce5a09158e24ac91dd", 0, TipoCuenta.CORRIENTE)]
        public async Task InactivarCuentaExitoso(string idCuetna, bool gmf, string idCliente, float saldo, TipoCuenta tipocuenta)
        {

            Cuenta nuevaCuentaAgregada = new CuentaBuilder()
            .WithId(idCuetna)
            .WithIdCliente(idCliente)
            .WithGMF(gmf)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo + 1000)
            .WithNumeroCuenta("2300000000")
            .WithFechaModificacion(DateTime.Now)
            .WithFechaCreacion(DateTime.Now)

            .Build();
            Cuenta nuevaCuentaActualizada = new CuentaBuilder()
            .WithId(idCuetna)
            .WithIdCliente(idCliente)
            .WithGMF(gmf)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo + 1000)
            .WithNumeroCuenta("2300000000")
            .WithFechaModificacion(DateTime.Now)
            .WithFechaCreacion(DateTime.Now)
            .WithEstadoCuenta(EstadoCuenta.ACTIVA)
            .Build();

            var returnList = new List<Cuenta>();

            returnList.Add(nuevaCuentaAgregada);

            _mockCuentaRepository
                .Setup(repository => repository.ActualizarCuentaAsync(It.IsAny<Cuenta>()))
                .ReturnsAsync(nuevaCuentaActualizada);

            _mockCuentaRepository
                .Setup(repository => repository.ObtenerCuentaPorIdAsync(It.IsAny<string>()))
                .ReturnsAsync(nuevaCuentaAgregada);

            _mockClienteRepository
                .Setup(repository => repository.ObtenerClientePorId(It.IsAny<string>()))
            .ReturnsAsync(
                new ClienteBuilder()
                .WithEstado(EstadoCliente.ACTIVO)
                .WithCuentas(returnList)
                .WithId(idCliente).Build());

            var estado = await _cuentaUseCase.InactivarCuentaAsync(idCuetna);

            _mockRepository.VerifyAll();

            Assert.Equal(estado, true);
        }

        [Theory]
        [InlineData("640204ce5a09158e24ac91dd", false, "640204ce5a09158e24ac91dd", 0, TipoCuenta.CORRIENTE)]
        public async Task InactivarCuentaErrorYaInactiva(string idCuetna, bool gmf, string idCliente, float saldo, TipoCuenta tipocuenta)
        {

            Cuenta nuevaCuentaAgregada = new CuentaBuilder()
            .WithId(idCuetna)
            .WithIdCliente(idCliente)
            .WithGMF(gmf)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo + 1000)
            .WithNumeroCuenta("2300000000")
                        .WithEstadoCuenta(EstadoCuenta.INACTIVA)
            .WithFechaModificacion(DateTime.Now)
            .WithFechaCreacion(DateTime.Now)

            .Build();
            Cuenta nuevaCuentaActualizada = new CuentaBuilder()
            .WithId(idCuetna)
            .WithIdCliente(idCliente)
            .WithGMF(gmf)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo + 1000)
            .WithNumeroCuenta("2300000000")
            .WithFechaModificacion(DateTime.Now)
            .WithFechaCreacion(DateTime.Now)
            .WithEstadoCuenta(EstadoCuenta.INACTIVA)
            .Build();

            var returnList = new List<Cuenta>();

            returnList.Add(nuevaCuentaAgregada);

            _mockCuentaRepository
                .Setup(repository => repository.ActualizarCuentaAsync(It.IsAny<Cuenta>()))
                .ReturnsAsync(nuevaCuentaActualizada);

            _mockCuentaRepository
                .Setup(repository => repository.ObtenerCuentaPorIdAsync(It.IsAny<string>()))
                .ReturnsAsync(nuevaCuentaAgregada);

            _mockClienteRepository
                .Setup(repository => repository.ObtenerClientePorId(It.IsAny<string>()))
            .ReturnsAsync(
                new ClienteBuilder()
                .WithEstado(EstadoCliente.ACTIVO)
                .WithCuentas(returnList)
                .WithId(idCliente).Build());

            var error = Assert.ThrowsAsync<BusinessException>(() => _cuentaUseCase.InactivarCuentaAsync(idCuetna));
        }

        [Theory]
        [InlineData("640204ce5a09158e24ac91dd", false, "640204ce5a09158e24ac91dd", 0, TipoCuenta.CORRIENTE)]
        public async Task InactivarCuentaErrorCliente(string idCuetna, bool gmf, string idCliente, float saldo, TipoCuenta tipocuenta)
        {

            Cuenta nuevaCuentaAgregada = new CuentaBuilder()
            .WithId(idCuetna)
            .WithIdCliente(idCliente)
            .WithGMF(gmf)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo + 1000)
            .WithNumeroCuenta("2300000000")
            .WithFechaModificacion(DateTime.Now)
            .WithFechaCreacion(DateTime.Now)

            .Build();
            Cuenta nuevaCuentaActualizada = new CuentaBuilder()
            .WithId(idCuetna)
            .WithIdCliente(idCliente)
            .WithGMF(gmf)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo + 1000)
            .WithNumeroCuenta("2300000000")
            .WithFechaModificacion(DateTime.Now)
            .WithFechaCreacion(DateTime.Now)
            .WithEstadoCuenta(EstadoCuenta.ACTIVA)
            .Build();

            Cliente clienteNullo = null;

            _mockCuentaRepository
                .Setup(repository => repository.ActualizarCuentaAsync(It.IsAny<Cuenta>()))
                .ReturnsAsync(nuevaCuentaActualizada);

            _mockCuentaRepository
                .Setup(repository => repository.ObtenerCuentaPorIdAsync(It.IsAny<string>()))
                .ReturnsAsync(nuevaCuentaAgregada);

            _mockClienteRepository
                .Setup(repository => repository.ObtenerClientePorId(It.IsAny<string>()))
            .ReturnsAsync(clienteNullo);

            var error = Assert.ThrowsAsync<BusinessException>(() => _cuentaUseCase.InactivarCuentaAsync(idCuetna));
        }
        [Theory]
        [InlineData("640204ce5a09158e24ac91dd", false, "640204ce5a09158e24ac91dd", 0, TipoCuenta.CORRIENTE)]
        public async Task InactivarCuentaErrorCuenta(string idCuetna, bool gmf, string idCliente, float saldo, TipoCuenta tipocuenta)
        {

            Cuenta nuevaCuentaAgregada = null;

            Cuenta nuevaCuentaActualizada = null;

            Cliente clienteNullo = null;

            _mockCuentaRepository
                .Setup(repository => repository.ActualizarCuentaAsync(It.IsAny<Cuenta>()))
                .ReturnsAsync(nuevaCuentaActualizada);

            _mockCuentaRepository
                .Setup(repository => repository.ObtenerCuentaPorIdAsync(It.IsAny<string>()))
                .ReturnsAsync(nuevaCuentaAgregada);

            _mockClienteRepository
                .Setup(repository => repository.ObtenerClientePorId(It.IsAny<string>()))
            .ReturnsAsync(clienteNullo);

            var error = Assert.ThrowsAsync<BusinessException>(() => _cuentaUseCase.InactivarCuentaAsync(idCuetna));
        }

        [Theory]
        [InlineData("640204ce5a09158e24ac91dd", false, "640204ce5a09158e24ac91dd", 0, TipoCuenta.CORRIENTE)]
        public async Task ActivarCuentaExitoso(string idCuetna, bool gmf, string idCliente, float saldo, TipoCuenta tipocuenta)
        {

            Cuenta nuevaCuentaAgregada = new CuentaBuilder()
            .WithId(idCuetna)
            .WithIdCliente(idCliente)
            .WithGMF(gmf)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo + 1000)
            .WithNumeroCuenta("2300000000")
            .WithFechaModificacion(DateTime.Now)
            .WithFechaCreacion(DateTime.Now)

            .Build();
            Cuenta nuevaCuentaActualizada = new CuentaBuilder()
            .WithId(idCuetna)
            .WithIdCliente(idCliente)
            .WithGMF(gmf)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo + 1000)
            .WithNumeroCuenta("2300000000")
            .WithFechaModificacion(DateTime.Now)
            .WithFechaCreacion(DateTime.Now)
            .WithEstadoCuenta(EstadoCuenta.INACTIVA)
            .Build();

            var returnList = new List<Cuenta>();

            returnList.Add(nuevaCuentaAgregada);

            _mockCuentaRepository
                .Setup(repository => repository.ActualizarCuentaAsync(It.IsAny<Cuenta>()))
                .ReturnsAsync(nuevaCuentaActualizada);

            _mockCuentaRepository
                .Setup(repository => repository.ObtenerCuentaPorIdAsync(It.IsAny<string>()))
                .ReturnsAsync(nuevaCuentaAgregada);

            _mockClienteRepository
                .Setup(repository => repository.ObtenerClientePorId(It.IsAny<string>()))
            .ReturnsAsync(
                new ClienteBuilder()
                .WithEstado(EstadoCliente.ACTIVO)
                .WithCuentas(returnList)
                .WithId(idCliente).Build());

            var Actualizada = await _cuentaUseCase.ActivarCuentaAsync(idCuetna);

            _mockRepository.VerifyAll();

            Assert.NotNull(Actualizada);
            Assert.Equal(Actualizada.EstadoCuenta, EstadoCuenta.ACTIVA);
        }
        [Theory]
        [InlineData("640204ce5a09158e24ac91dd", false, "640204ce5a09158e24ac91dd", 0, TipoCuenta.CORRIENTE)]
        public async Task ActivarCuentaErrorYaActiva(string idCuetna, bool gmf, string idCliente, float saldo, TipoCuenta tipocuenta)
        {

            Cuenta nuevaCuentaAgregada = new CuentaBuilder()
            .WithId(idCuetna)
            .WithIdCliente(idCliente)
            .WithGMF(gmf)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo + 1000)
            .WithNumeroCuenta("2300000000")
                        .WithEstadoCuenta(EstadoCuenta.ACTIVA)
            .WithFechaModificacion(DateTime.Now)
            .WithFechaCreacion(DateTime.Now)

            .Build();
            Cuenta nuevaCuentaActualizada = new CuentaBuilder()
            .WithId(idCuetna)
            .WithIdCliente(idCliente)
            .WithGMF(gmf)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo + 1000)
            .WithNumeroCuenta("2300000000")
            .WithFechaModificacion(DateTime.Now)
            .WithFechaCreacion(DateTime.Now)
            .WithEstadoCuenta(EstadoCuenta.ACTIVA)
            .Build();

            var returnList = new List<Cuenta>();

            returnList.Add(nuevaCuentaAgregada);

            _mockCuentaRepository
                .Setup(repository => repository.ActualizarCuentaAsync(It.IsAny<Cuenta>()))
                .ReturnsAsync(nuevaCuentaActualizada);

            _mockCuentaRepository
                .Setup(repository => repository.ObtenerCuentaPorIdAsync(It.IsAny<string>()))
                .ReturnsAsync(nuevaCuentaAgregada);

            _mockClienteRepository
                .Setup(repository => repository.ObtenerClientePorId(It.IsAny<string>()))
            .ReturnsAsync(
                new ClienteBuilder()
                .WithEstado(EstadoCliente.ACTIVO)
                .WithCuentas(returnList)
                .WithId(idCliente).Build());

            var error = Assert.ThrowsAsync<BusinessException>(() => _cuentaUseCase.ActivarCuentaAsync(idCuetna));
        }

        [Theory]
        [InlineData("640204ce5a09158e24ac91dd", false, "640204ce5a09158e24ac91dd", 0, TipoCuenta.CORRIENTE)]
        public async Task ActivarCuentaErrorCliente(string idCuetna, bool gmf, string idCliente, float saldo, TipoCuenta tipocuenta)
        {

            Cuenta nuevaCuentaAgregada = new CuentaBuilder()
            .WithId(idCuetna)
            .WithIdCliente(idCliente)
            .WithGMF(gmf)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo + 1000)
            .WithNumeroCuenta("2300000000")
            .WithFechaModificacion(DateTime.Now)
            .WithFechaCreacion(DateTime.Now)

            .Build();
            Cuenta nuevaCuentaActualizada = new CuentaBuilder()
            .WithId(idCuetna)
            .WithIdCliente(idCliente)
            .WithGMF(gmf)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo + 1000)
            .WithNumeroCuenta("2300000000")
            .WithFechaModificacion(DateTime.Now)
            .WithFechaCreacion(DateTime.Now)
            .WithEstadoCuenta(EstadoCuenta.INACTIVA)
            .Build();


            Cliente clienteNullo = null;

            _mockCuentaRepository
                .Setup(repository => repository.ActualizarCuentaAsync(It.IsAny<Cuenta>()))
                .ReturnsAsync(nuevaCuentaActualizada);

            _mockCuentaRepository
                .Setup(repository => repository.ObtenerCuentaPorIdAsync(It.IsAny<string>()))
                .ReturnsAsync(nuevaCuentaAgregada);

            _mockClienteRepository
                .Setup(repository => repository.ObtenerClientePorId(It.IsAny<string>()))
            .ReturnsAsync(
               clienteNullo);

            var error = Assert.ThrowsAsync<BusinessException>(() => _cuentaUseCase.ActivarCuentaAsync(idCuetna));
        }

        [Theory]
        [InlineData("640204ce5a09158e24ac91dd", false, "640204ce5a09158e24ac91dd", 0, TipoCuenta.CORRIENTE)]
        public async Task ActivarCuentaErrorCuenta(string idCuetna, bool gmf, string idCliente, float saldo, TipoCuenta tipocuenta)
        {

            Cuenta nuevaCuentaAgregada = null;

            Cuenta nuevaCuentaActualizada = null;

            Cliente clienteNullo = null;

            _mockCuentaRepository
                .Setup(repository => repository.ActualizarCuentaAsync(It.IsAny<Cuenta>()))
                .ReturnsAsync(nuevaCuentaActualizada);

            _mockCuentaRepository
                .Setup(repository => repository.ObtenerCuentaPorIdAsync(It.IsAny<string>()))
                .ReturnsAsync(nuevaCuentaAgregada);

            _mockClienteRepository
                .Setup(repository => repository.ObtenerClientePorId(It.IsAny<string>()))
            .ReturnsAsync(
               clienteNullo);

            var error = Assert.ThrowsAsync<BusinessException>(() => _cuentaUseCase.ActivarCuentaAsync(idCuetna));
        }

        [Theory]
        [InlineData("640204ce5a09158e24ac91dd", false, "640204ce5a09158e24ac91dd", 0, TipoCuenta.CORRIENTE)]
        public async Task ObtenrCuentaPorIdExitoso(string idCuetna, bool gmf, string idCliente, float saldo, TipoCuenta tipocuenta)
        {

            Cuenta nuevaCuentaAgregada = new CuentaBuilder()
            .WithId(idCuetna)
            .WithIdCliente(idCliente)
            .WithGMF(gmf)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo)
            .WithNumeroCuenta("2300000000")
            .WithFechaModificacion(DateTime.Now)
            .WithFechaCreacion(DateTime.Now)
            .Build();



            _mockCuentaRepository
                .Setup(repository => repository.ObtenerCuentaPorIdAsync(It.IsAny<string>()))
                .ReturnsAsync(nuevaCuentaAgregada);



            var cuenta = await _cuentaUseCase.ObtenerCuentaPorIdAsync(idCuetna);

            _mockRepository.VerifyAll();

            Assert.NotNull(cuenta);
            Assert.Equal(cuenta.Id, idCuetna);
        }

        [Theory]
        [InlineData("640204ce5a09158e24ac91dd", false, "640204ce5a09158e24ac91dd", 0, TipoCuenta.CORRIENTE)]
        public async Task ObtenrCuentaPorIdErrorCancelada(string idCuetna, bool gmf, string idCliente, float saldo, TipoCuenta tipocuenta)
        {

            Cuenta nuevaCuentaAgregada = new CuentaBuilder()
            .WithId(idCuetna)
            .WithIdCliente(idCliente)
            .WithGMF(gmf)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo)
            .WithNumeroCuenta("2300000000")
            .WithFechaModificacion(DateTime.Now)
            .WithFechaCreacion(DateTime.Now)
            .WithEstadoCuenta(EstadoCuenta.CANCELADA)
            .Build();

            _mockCuentaRepository
                .Setup(repository => repository.ObtenerCuentaPorIdAsync(It.IsAny<string>()))
                .ReturnsAsync(nuevaCuentaAgregada);

            var error = Assert.ThrowsAsync<BusinessException>(() => _cuentaUseCase.ObtenerCuentaPorIdAsync(It.IsAny<string>()));

        }

        [Fact]
        public async Task ObtenrCuentaPorIdError()
        {
            Cuenta cuenta = null;

            _mockCuentaRepository
                .Setup(repository => repository.ObtenerCuentaPorIdAsync(It.IsAny<string>()))
                .ReturnsAsync(cuenta);

            _mockCuentaRepository
                .Setup(repository => repository.ActualizarCuentaAsync(It.IsAny<Cuenta>()))
                .ReturnsAsync(new Cuenta());

            var error = Assert.ThrowsAsync<BusinessException>(() => _cuentaUseCase.ObtenerCuentaPorIdAsync(It.IsAny<string>()));

        }

        [Theory]
        [InlineData("2300000000", false, "640204ce5a09158e24ac91dd", 0, TipoCuenta.CORRIENTE)]
        public async Task ObtenrCuentaPorNumeroExitoso(string numero, bool gmf, string idCliente, float saldo, TipoCuenta tipocuenta)
        {

            Cuenta nuevaCuentaAgregada = new CuentaBuilder()
            .WithId("640204ce5a09158e24ac91dd")
            .WithIdCliente(idCliente)
            .WithGMF(gmf)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo)
            .WithNumeroCuenta(numero)
            .WithFechaModificacion(DateTime.Now)
            .WithFechaCreacion(DateTime.Now)
            .Build();



            _mockCuentaRepository
                .Setup(repository => repository.ObtenerCuentaPorNumeroCuentaAsync(It.IsAny<string>()))
                .ReturnsAsync(nuevaCuentaAgregada);



            var cuenta = await _cuentaUseCase.ObtenerCuentaPorNumeroCuentaAsync(numero);

            _mockRepository.VerifyAll();

            Assert.NotNull(cuenta);
            Assert.Equal(cuenta.NumeroCuenta, numero);
        }


        [Theory]
        [InlineData("640204ce5a09158e24ac91dd", false, "640204ce5a09158e24ac91dd", 0, TipoCuenta.CORRIENTE)]
        public async Task ObtenrCuentaPorNumeroErrorCancelada(string idCuetna, bool gmf, string idCliente, float saldo, TipoCuenta tipocuenta)
        {

            Cuenta nuevaCuentaAgregada = new CuentaBuilder()
            .WithId(idCuetna)
            .WithIdCliente(idCliente)
            .WithGMF(gmf)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo)
            .WithNumeroCuenta("2300000000")
            .WithFechaModificacion(DateTime.Now)
            .WithFechaCreacion(DateTime.Now)
            .WithEstadoCuenta(EstadoCuenta.CANCELADA)
            .Build();

            _mockCuentaRepository
                .Setup(repository => repository.ObtenerCuentaPorNumeroCuentaAsync(It.IsAny<string>()))
                .ReturnsAsync(nuevaCuentaAgregada);

            var error = Assert.ThrowsAsync<BusinessException>(() => _cuentaUseCase.ObtenerCuentaPorNumeroCuentaAsync(It.IsAny<string>()));

        }


        [Fact]
        public async Task ObtenrCuentaPorNumeroError()
        {
            Cuenta cuenta = null;

            _mockCuentaRepository
                .Setup(repository => repository.ObtenerCuentaPorNumeroCuentaAsync(It.IsAny<string>()))
                .ReturnsAsync(cuenta);

            _mockCuentaRepository
                .Setup(repository => repository.ActualizarCuentaAsync(It.IsAny<Cuenta>()))
                .ReturnsAsync(new Cuenta());

            var error = Assert.ThrowsAsync<BusinessException>(() => _cuentaUseCase.ObtenerCuentaPorNumeroCuentaAsync(It.IsAny<string>()));

        }

            [Theory]
        [InlineData("640204ce5a09158e24ac91dd", false, "640204ce5a09158e24ac91dd", 0, TipoCuenta.CORRIENTE)]
        public async Task MarcarGMFPorIDCuetnaExitoso(string idCuetna, bool gmf, string idCliente, float saldo, TipoCuenta tipocuenta)
        {

            Cuenta nuevaCuentaAgregada = new CuentaBuilder()
            .WithId(idCuetna)
            .WithIdCliente(idCliente)
            .WithGMF(gmf)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo)
            .WithFechaModificacion(DateTime.Now)
            .WithFechaCreacion(DateTime.Now)
            .Build();


            Cuenta nuevaCuentaActualizada = new CuentaBuilder()
            .WithId(idCuetna)
            .WithIdCliente(idCliente)
            .WithGMF(true)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo)
            .WithFechaModificacion(DateTime.Now)
            .WithFechaCreacion(DateTime.Now)
            .Build();

            _mockCuentaRepository
                .Setup(repository => repository.ObtenerCuentaPorIdAsync(It.IsAny<string>()))
                .ReturnsAsync(nuevaCuentaAgregada);

            _mockCuentaRepository
                .Setup(repository => repository.ActualizarCuentaAsync(It.IsAny<Cuenta>()))
                .ReturnsAsync(nuevaCuentaActualizada);


            var cuenta = await _cuentaUseCase.MarcarCuentaGMFAsync(idCuetna);

            _mockRepository.VerifyAll();

            Assert.NotNull(cuenta);
            Assert.Equal(cuenta.GMF, true);
        }

        [Theory]
        [InlineData("640204ce5a09158e24ac91dd", false, "640204ce5a09158e24ac91dd", 0, TipoCuenta.CORRIENTE)]
        public async Task MarcarGMFPorIDCuetnaError(string idCuetna, bool gmf, string idCliente, float saldo, TipoCuenta tipocuenta)
        {

            Cuenta nuevaCuentaAgregada = new CuentaBuilder()
            .WithId(idCuetna)
            .WithIdCliente(idCliente)
            .WithGMF(gmf)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo)
            .WithFechaModificacion(DateTime.Now)
            .WithFechaCreacion(DateTime.Now)
            .WithEstadoCuenta(EstadoCuenta.CANCELADA)
            .Build();


            Cuenta nuevaCuentaActualizada = new CuentaBuilder()
            .WithId(idCuetna)
            .WithIdCliente(idCliente)
            .WithGMF(true)
            .WithTipoCuenta(tipocuenta)
            .WithSaldo(saldo)
            .WithFechaModificacion(DateTime.Now)
            .WithFechaCreacion(DateTime.Now)
            .WithEstadoCuenta(EstadoCuenta.CANCELADA)
            .Build();

            _mockCuentaRepository
                .Setup(repository => repository.ObtenerCuentaPorIdAsync(It.IsAny<string>()))
                .ReturnsAsync(nuevaCuentaAgregada);

            _mockCuentaRepository
                .Setup(repository => repository.ActualizarCuentaAsync(It.IsAny<Cuenta>()))
                .ReturnsAsync(nuevaCuentaActualizada);

            var error = Assert.ThrowsAsync<BusinessException>(() => _cuentaUseCase.MarcarCuentaGMFAsync(idCuetna));


        }

        [Fact]
        public async Task MarcarGMFPorIDCuetnaErrorNoExiste()
        {
            Cuenta cuenta = null;

            _mockCuentaRepository
                .Setup(repository => repository.ObtenerCuentaPorIdAsync(It.IsAny<string>()))
                .ReturnsAsync(cuenta);

            _mockCuentaRepository
                .Setup(repository => repository.ActualizarCuentaAsync(It.IsAny<Cuenta>()))
                .ReturnsAsync(new Cuenta());

            var error = Assert.ThrowsAsync<BusinessException>(() => _cuentaUseCase.MarcarCuentaGMFAsync(It.IsAny<string>()));


        }
    }

}
