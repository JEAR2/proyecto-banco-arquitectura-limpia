using credinet.exception.middleware.models;
using Domain.Model.Entidades;
using Domain.Model.Entidades.Enums;
using Domain.Model.Gateway;
using Domain.UseCase.Clientes;
using Domain.UseCase.Cuentas;
using Domain.UseCase.Transacciones;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xunit;

namespace Domain.UseCase.Tests.Transacciones
{
    public class TransaccionesUseCaseTest
    {
        private readonly Mock<ITransaccionesRepository> _mockTransaccionesRepository;
        private readonly Mock<ICuentasUseCase> _mockCuentaUseCase;
        private readonly Mock<IClienteUseCase> _mockClienteUseCase;
        private readonly Mock<ITransaccionesEventsRepository> _mockTransaccionesEventsRepository;
        private readonly TransaccionesUseCase _transaccionesUseCase;
        public TransaccionesUseCaseTest()
        {
            _mockTransaccionesRepository = new();
            _mockCuentaUseCase = new();
            _mockClienteUseCase = new();
            _mockTransaccionesEventsRepository = new();
            _transaccionesUseCase = new(_mockTransaccionesRepository.Object, _mockCuentaUseCase.Object,
                _mockClienteUseCase.Object, _mockTransaccionesEventsRepository.Object);
        }

        [Theory]
        [InlineData("1")]
        public async Task ObtenerTransaccionPorIdExiste(string id)
        {
            _mockTransaccionesRepository.Setup(repo => repo.ObtenerTransaccionPorIdAsync(id)).ReturnsAsync(ObtenerTransaccionParaTest());

            var respuesta = await _transaccionesUseCase.ObtenerTransaccion(id);

            _mockTransaccionesRepository.Verify(repo => repo.ObtenerTransaccionPorIdAsync(id));
            Assert.NotNull(respuesta);
            Assert.True(respuesta.Id == id);
        }

        [Theory]
        [InlineData("1")]
        public async Task ObtenerTransaccionPorIdNoExiste(string id)
        {
            _mockTransaccionesRepository.Setup(repo => repo.ObtenerTransaccionPorIdAsync(id)).ReturnsAsync((Transaccion)null!);

            var transaccionUseCase = new TransaccionesUseCase(_mockTransaccionesRepository.Object, _mockCuentaUseCase.Object,
                _mockClienteUseCase.Object, _mockTransaccionesEventsRepository.Object);

            BusinessException businessException = await Assert.ThrowsAsync<BusinessException>(async ()
                => await _transaccionesUseCase.ObtenerTransaccion(id));
        }

        [Fact]
        public async Task RealizarTransaccionTipoTransferenciaTestExistosa()
        {
            var transaccion = ObtenerTransaccionParaTest();

            _mockCuentaUseCase.Setup(useCase => useCase.ObtenerCuentaPorIdAsync(transaccion.IdCuentaEmisora)).
                ReturnsAsync(ObtenerCuentaEmisoraParaTest());

            _mockCuentaUseCase.Setup(useCase => useCase.ObtenerCuentaPorIdAsync(transaccion.IdCuentaReceptora)).
                ReturnsAsync(ObtenerCuentaReceptoraParaTest());

            _mockClienteUseCase.Setup(useCase => useCase.ObtenerClientePorId(ObtenerCuentaEmisoraParaTest().IdCliente)).
                ReturnsAsync(ObtenerClienteEmisorParaTest());

            _mockClienteUseCase.Setup(useCase => useCase.ObtenerClientePorId(ObtenerCuentaReceptoraParaTest().IdCliente)).
               ReturnsAsync(ObtenerClienteReceptorParaTest());

            _mockTransaccionesRepository.Setup(repo => repo.CrearTransaccionAsync(transaccion)).ReturnsAsync(transaccion);

            _mockCuentaUseCase.Setup(useCase => useCase.AgregarTransaccionCuentaAsync(It.IsAny<Transaccion>(), It.IsAny<string>())).ReturnsAsync(It.IsAny<Cuenta>());

            _mockTransaccionesEventsRepository.Setup(repo => repo.NotificarTransaccionRealizada(It.IsAny<string>(), It.IsAny<Transaccion>())).Returns(Task.CompletedTask);

            var taskCompleted = _transaccionesUseCase.RealizarTransaccion(transaccion);

            Assert.True(taskCompleted.IsCompleted);


            _mockCuentaUseCase.Verify(useCase => useCase.ObtenerCuentaPorIdAsync(transaccion.IdCuentaEmisora), Times.Once);

            _mockCuentaUseCase.Verify(useCase => useCase.ObtenerCuentaPorIdAsync(transaccion.IdCuentaReceptora), Times.Once);

            _mockClienteUseCase.Verify(useCase => useCase.ObtenerClientePorId(ObtenerCuentaEmisoraParaTest().IdCliente), Times.Once);

            _mockClienteUseCase.Verify(useCase => useCase.ObtenerClientePorId(ObtenerCuentaReceptoraParaTest().IdCliente), Times.Once);

            _mockTransaccionesRepository.Verify(repo => repo.CrearTransaccionAsync(It.IsAny<Transaccion>()), Times.Exactly(2));

            _mockCuentaUseCase.Verify(useCase => useCase.AgregarTransaccionCuentaAsync(It.IsAny<Transaccion>(), It.IsAny<string>()), Times.Exactly(2));

            _mockTransaccionesEventsRepository.Verify(repo => repo.NotificarTransaccionRealizada(It.IsAny<string>(), It.IsAny<Transaccion>()), Times.Exactly(2));

        }

        [Fact]
        public async Task RealizarTransaccionTipoTransferenciaTestCuentaEmisoraNoExiste()
        {
            var transaccion = ObtenerTransaccionParaTest();

            _mockCuentaUseCase.Setup(useCase => useCase.ObtenerCuentaPorIdAsync(transaccion.IdCuentaEmisora)).
                ReturnsAsync((Cuenta)null!);

            BusinessException businessException = await Assert.ThrowsAsync<BusinessException>(async ()
               => await _transaccionesUseCase.RealizarTransaccion(transaccion));
        }

        [Fact]
        public async Task RealizarTransaccionTipoTransferenciaTestCuentaReceptoraNoExiste()
        {
            var transaccion = ObtenerTransaccionParaTest();

            _mockCuentaUseCase.Setup(useCase => useCase.ObtenerCuentaPorIdAsync(transaccion.IdCuentaEmisora)).
                ReturnsAsync(ObtenerCuentaEmisoraParaTest());

            _mockCuentaUseCase.Setup(useCase => useCase.ObtenerCuentaPorIdAsync(transaccion.IdCuentaReceptora)).
                ReturnsAsync((Cuenta)null!);

            BusinessException businessException = await Assert.ThrowsAsync<BusinessException>(async ()
               => await _transaccionesUseCase.RealizarTransaccion(transaccion));


        }

        [Fact]
        public async Task RealizarTransaccionTipoTransferenciaTestCuentaEmisoraInactiva()
        {
            var transaccion = ObtenerTransaccionParaTest();
            var cuentaInactiva = ObtenerCuentaEmisoraParaTest();
            cuentaInactiva.EstadoCuenta = EstadoCuenta.INACTIVA;
            _mockCuentaUseCase.Setup(useCase => useCase.ObtenerCuentaPorIdAsync(transaccion.IdCuentaEmisora)).
                ReturnsAsync(cuentaInactiva);

            _mockCuentaUseCase.Setup(useCase => useCase.ObtenerCuentaPorIdAsync(transaccion.IdCuentaReceptora)).
                ReturnsAsync(ObtenerCuentaReceptoraParaTest());

            BusinessException businessException = await Assert.ThrowsAsync<BusinessException>(async ()
               => await _transaccionesUseCase.RealizarTransaccion(transaccion));


        }

        [Fact]
        public async Task RealizarConsignacionCuentaReceptoraNoExiste()
        {
            var transaccion = ObtenerTransaccionParaTest();
            transaccion.TipoTransaccion = TipoTransaccion.CONSIGNACION;

            _mockCuentaUseCase.Setup(useCase => useCase.ObtenerCuentaPorIdAsync(transaccion.IdCuentaReceptora)).
                ReturnsAsync((Cuenta)null!);

            BusinessException businessException = await Assert.ThrowsAsync<BusinessException>(async ()
               => await _transaccionesUseCase.RealizarTransaccion(transaccion));
        }

        [Fact]
        public async Task RealzarConsignacionExitosa()
        {
            var transaccion = ObtenerTransaccionParaTest();
            transaccion.TipoTransaccion = TipoTransaccion.CONSIGNACION;

            _mockCuentaUseCase.Setup(useCase => useCase.ObtenerCuentaPorIdAsync(transaccion.IdCuentaReceptora)).
                ReturnsAsync(ObtenerCuentaReceptoraParaTest());

            _mockTransaccionesRepository.Setup(repo => repo.CrearTransaccionAsync(transaccion)).ReturnsAsync(transaccion);

            _mockCuentaUseCase.Setup(useCase => useCase.AgregarTransaccionCuentaAsync(It.IsAny<Transaccion>(), It.IsAny<string>())).ReturnsAsync(It.IsAny<Cuenta>());

            _mockClienteUseCase.Setup(useCase => useCase.ObtenerClientePorId(ObtenerCuentaReceptoraParaTest().IdCliente)).ReturnsAsync(ObtenerClienteReceptorParaTest());

            _mockTransaccionesEventsRepository.Setup(repo => repo.NotificarTransaccionRealizada(It.IsAny<string>(), It.IsAny<Transaccion>())).Returns(Task.CompletedTask);

            var taskCompleted = _transaccionesUseCase.RealizarTransaccion(transaccion);

            Assert.True(taskCompleted.IsCompleted);


            _mockCuentaUseCase.Verify(useCase => useCase.ObtenerCuentaPorIdAsync(transaccion.IdCuentaReceptora), Times.Once);

            _mockTransaccionesRepository.Verify(repo => repo.CrearTransaccionAsync(It.IsAny<Transaccion>()), Times.Once);

            _mockCuentaUseCase.Verify(useCase => useCase.AgregarTransaccionCuentaAsync(It.IsAny<Transaccion>(), It.IsAny<string>()), Times.Once);

            _mockClienteUseCase.Verify(useCase => useCase.ObtenerClientePorId(ObtenerCuentaReceptoraParaTest().IdCliente), Times.Once);

            _mockTransaccionesEventsRepository.Verify(repo => repo.NotificarTransaccionRealizada(It.IsAny<string>(), It.IsAny<Transaccion>()), Times.Exactly(1));
        }

        [Fact]
        public async Task RealizarRetiroCuentaReceptoraNoExiste()
        {
            var transaccion = ObtenerTransaccionParaTest();
            transaccion.TipoTransaccion = TipoTransaccion.RETIRO;

            _mockCuentaUseCase.Setup(useCase => useCase.ObtenerCuentaPorIdAsync(transaccion.IdCuentaReceptora)).
                ReturnsAsync((Cuenta)null!);

            BusinessException businessException = await Assert.ThrowsAsync<BusinessException>(async ()
               => await _transaccionesUseCase.RealizarTransaccion(transaccion));
        }

        [Fact]
        public async Task RealzarRetiroExitoso()
        {
            var transaccion = ObtenerTransaccionParaTest();
            transaccion.TipoTransaccion = TipoTransaccion.RETIRO;

            _mockCuentaUseCase.Setup(useCase => useCase.ObtenerCuentaPorIdAsync(transaccion.IdCuentaReceptora)).
                ReturnsAsync(ObtenerCuentaReceptoraParaTest());

            _mockTransaccionesRepository.Setup(repo => repo.CrearTransaccionAsync(transaccion)).ReturnsAsync(transaccion);

            _mockCuentaUseCase.Setup(useCase => useCase.AgregarTransaccionCuentaAsync(It.IsAny<Transaccion>(), It.IsAny<string>())).ReturnsAsync(It.IsAny<Cuenta>());

            _mockClienteUseCase.Setup(useCase => useCase.ObtenerClientePorId(ObtenerCuentaReceptoraParaTest().IdCliente)).ReturnsAsync(ObtenerClienteReceptorParaTest());

            _mockTransaccionesEventsRepository.Setup(repo => repo.NotificarTransaccionRealizada(It.IsAny<string>(), It.IsAny<Transaccion>())).Returns(Task.CompletedTask);

            var taskCompleted = _transaccionesUseCase.RealizarTransaccion(transaccion);

            Assert.True(taskCompleted.IsCompleted);


            _mockCuentaUseCase.Verify(useCase => useCase.ObtenerCuentaPorIdAsync(transaccion.IdCuentaReceptora), Times.Once);

            _mockTransaccionesRepository.Verify(repo => repo.CrearTransaccionAsync(It.IsAny<Transaccion>()), Times.Once);

            _mockCuentaUseCase.Verify(useCase => useCase.AgregarTransaccionCuentaAsync(It.IsAny<Transaccion>(), It.IsAny<string>()), Times.Once);

            _mockClienteUseCase.Verify(useCase => useCase.ObtenerClientePorId(ObtenerCuentaReceptoraParaTest().IdCliente), Times.Once);

            _mockTransaccionesEventsRepository.Verify(repo => repo.NotificarTransaccionRealizada(It.IsAny<string>(), It.IsAny<Transaccion>()), Times.Exactly(1));
        }

        [Fact]
        public async Task TransaccionConValorNegativo()
        {
            var transaccion = ObtenerTransaccionParaTest();
            transaccion.Valor = -10;

            BusinessException businessException = await Assert.ThrowsAsync<BusinessException>(async ()
               => await _transaccionesUseCase.RealizarTransaccion(transaccion));
        }
        public Transaccion ObtenerTransaccionParaTest()
        {
            return new Transaccion()
            {
                Id = "1",
                IdCuentaEmisora = "1",
                IdCuentaReceptora = "2",
                TipoTransaccion = TipoTransaccion.TRANSFERENCIA,
                Valor = 10,
                FechaMovimiento = DateTime.UtcNow.ToLocalTime(),
                TipoMovimiento = TipoMovimiento.CREDITO
            };
        }

        public Cuenta ObtenerCuentaEmisoraParaTest()
        {
            return new Cuenta()
            {
                Id = "1",
                NumeroCuenta = "1",
                IdCliente = "1",
                TipoCuenta = TipoCuenta.AHORRO,
                EstadoCuenta = EstadoCuenta.ACTIVA,
                Saldo = 100000,
                SaldoDisponible = 100000,
                GMF = false,
                FechaCreacion = DateTime.UtcNow.ToLocalTime(),
                Transacciones = {}
            };
        }

        public Cuenta ObtenerCuentaReceptoraParaTest()
        {
            return new Cuenta()
            {
                Id = "2",
                NumeroCuenta = "2",
                IdCliente = "2",
                TipoCuenta = TipoCuenta.AHORRO,
                EstadoCuenta = EstadoCuenta.ACTIVA,
                Saldo = 100000,
                SaldoDisponible = 100000,
                GMF = false,
                FechaCreacion = DateTime.UtcNow.ToLocalTime(),
                Transacciones = { }
            };
        }

        public Cliente ObtenerClienteEmisorParaTest()
        {
            return new Cliente
            {
                Id = "1",
                Nombre = "Juan",
                Apellido = "Pérez",
                Correo = "juan.perez@example.com",
                FechaNacimiento = new DateTime(1990, 1, 1),
                TipoIdentificacion = TipoIdentificacion.CEDULACIUDADANIA,
                NumeroIdentificacion = "1234567890",
                Estado = EstadoCliente.ACTIVO
            };
        }

        public Cliente ObtenerClienteReceptorParaTest()
        {
            return new Cliente
            {
                Id = "1",
                Nombre = "Juan",
                Apellido = "Pérez",
                Correo = "juan.perez@example.com",
                FechaNacimiento = new DateTime(1990, 1, 1),
                TipoIdentificacion = TipoIdentificacion.CEDULACIUDADANIA,
                NumeroIdentificacion = "1234567890",
                Estado = EstadoCliente.ACTIVO
            };
        }
    }
}
