using AutoMapper;
using BancoAmarillo.AppServices.Automapper;
using Domain.Model.Entidades;
using Domain.Model.Entidades.Enums;
using DrivenAdapters.Mongo.Adaptadores;
using DrivenAdapters.Mongo.Entities;
using DrivenAdapters.Mongo.Tests.Entities;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace DrivenAdapters.Mongo.Tests
{
    public class CuentaAdapterTest
    {
        private readonly Mock<IContext> _mockContext;
        private readonly IMapper _mockMapper;
        private readonly Mock<IMongoCollection<CuentaEntity>> _mockColeccionCuentas;
        private readonly Mock<IAsyncCursor<CuentaEntity>> _mockCuentaCursor;
        private readonly IConfigurationProvider _configurationProvider;

        public CuentaAdapterTest()
        {
            _mockContext = new();
            _mockColeccionCuentas = new();
            _mockCuentaCursor = new();
            _configurationProvider = new MapperConfiguration(options => options.AddProfile<ConfigurationProfile>());
            _mockMapper = _configurationProvider.CreateMapper();
            _mockColeccionCuentas.Object.InsertMany(ObtenerCuentasTest());
            _mockCuentaCursor.SetupSequence(item => item.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true).Returns(false);
            _mockCuentaCursor.SetupSequence(item => item.MoveNextAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(true)).Returns(Task.FromResult(false));
        }

        [Fact]
        public async Task Cuenta_Adapter_Crear_Cuenta_Retorna_Cuenta_Creada()
        {
            _mockColeccionCuentas.Setup(op => op.InsertOneAsync(It.IsAny<CuentaEntity>(), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()));

            _mockContext.Setup(context => context.Cuentas).Returns(_mockColeccionCuentas.Object);
            var cuentaAdapter = new CuentaAdapter(_mockContext.Object, _mockMapper);

            var result = await cuentaAdapter.CrearCuentaAsync(ObtenerCuentaTest());

            Assert.NotNull(result);
            Assert.Equal(Domain.Model.Entidades.Enums.EstadoCuenta.ACTIVA, result.EstadoCuenta);
            Assert.IsType<Cuenta>(result);
        }

        [Fact]
        public async Task Cuenta_Adapter_Actualizar_Cuenta_Retorna_Cuenta_Actualizada()
        {
            string idCuenta = "1";
            _mockColeccionCuentas.Setup(op => op.FindAsync(It.IsAny<FilterDefinition<CuentaEntity>>(),
                It.IsAny<FindOptions<CuentaEntity, CuentaEntity>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_mockCuentaCursor.Object);
            _mockColeccionCuentas.Setup(op => op.ReplaceOneAsync(It.IsAny<FilterDefinition<CuentaEntity>>(),
              It.IsAny<CuentaEntity>(), It.IsAny<ReplaceOptions>(), It.IsAny<CancellationToken>()));

            _mockContext.Setup(context => context.Cuentas).Returns(_mockColeccionCuentas.Object);

            var cuentaRepository = new CuentaAdapter(_mockContext.Object, _mockMapper);
            var result = await cuentaRepository.ActualizarCuentaAsync(ObtenerCuentaTest());

            Assert.NotNull(result);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        public async Task Cuenta_Adapter_Obtener_Cuenta_Por_Numero_Cuenta_Retorna_Cuenta_Encontrada(string numeroCuenta)
        {
            List<CuentaEntity> listaCuentas = ObtenerCuentasTest();
            _mockCuentaCursor.Setup(item => item.Current).Returns(listaCuentas);

            _mockColeccionCuentas.Setup(op => op.FindAsync(It.IsAny<FilterDefinition<CuentaEntity>>(),
                It.IsAny<FindOptions<CuentaEntity, CuentaEntity>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_mockCuentaCursor.Object);

            _mockContext.Setup(context => context.Cuentas).Returns(_mockColeccionCuentas.Object);

            var cuentaRepository = new CuentaAdapter(_mockContext.Object, _mockMapper);

            var result = await cuentaRepository.ObtenerCuentaPorNumeroCuentaAsync(numeroCuenta);

            Assert.NotNull(result);
            Assert.IsType<Cuenta>(result);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        public async Task Cuenta_Adapter_Obtener_Cuenta_Por_Id_Cliente_Retorna_Cuenta_Encontrada(string idCliente)
        {
            List<CuentaEntity> listaCuentas = ObtenerCuentasTest();
            _mockCuentaCursor.Setup(item => item.Current).Returns(listaCuentas);

            _mockColeccionCuentas.Setup(op => op.FindAsync(It.IsAny<FilterDefinition<CuentaEntity>>(),
                It.IsAny<FindOptions<CuentaEntity, CuentaEntity>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_mockCuentaCursor.Object);

            _mockContext.Setup(context => context.Cuentas).Returns(_mockColeccionCuentas.Object);

            var cuentaRepository = new CuentaAdapter(_mockContext.Object, _mockMapper);

            var result = await cuentaRepository.ObtenerCuentasPorIdClienteAsync(idCliente);

            Assert.NotNull(result);
            Assert.IsType<List<Cuenta>>(result);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        public async Task Cuenta_Adapter_Obtener_Cuenta_Por_Id_Retorna_Cuenta_Encontrada(string idCuenta)
        {
            List<CuentaEntity> listaCuentas = ObtenerCuentasTest();
            _mockCuentaCursor.Setup(item => item.Current).Returns(listaCuentas);

            _mockColeccionCuentas.Setup(op => op.FindAsync(It.IsAny<FilterDefinition<CuentaEntity>>(),
                It.IsAny<FindOptions<CuentaEntity, CuentaEntity>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_mockCuentaCursor.Object);

            _mockContext.Setup(context => context.Cuentas).Returns(_mockColeccionCuentas.Object);

            var cuentaRepository = new CuentaAdapter(_mockContext.Object, _mockMapper);

            var result = await cuentaRepository.ObtenerCuentaPorIdAsync(idCuenta);

            Assert.NotNull(result);
            Assert.IsType<Cuenta>(result);
        }

        [Fact]
        public async Task Cuenta_Adapter_Obtener_Cantidad_Cuentas_Por_Tipo_Cuenta_Retorna_Cantidad_Cuentas_Encontradas()
        {
            var tipoCuenta = TipoCuenta.AHORRO;

            _mockColeccionCuentas.Setup(op => op.CountDocumentsAsync(It.IsAny<FilterDefinition<CuentaEntity>>(),
                It.IsAny<CountOptions>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);

            _mockContext.Setup(context => context.Cuentas).Returns(_mockColeccionCuentas.Object);

            var cuentaRepository = new CuentaAdapter(_mockContext.Object, _mockMapper);

            var result = await cuentaRepository.ObtenerCountCuentasTipoAsync(tipoCuenta);

            Assert.NotNull(result);
            Assert.Equal(1, result);
        }

        private List<CuentaEntity> ObtenerCuentasTest() => new()
        {
            new CuentaEntityBuilder()
            .WithId("1")
            .WithNumeroCuenta("23423423")
            .WithIdCliente("21312")
            .WithTipoCuenta(Domain.Model.Entidades.Enums.TipoCuenta.AHORRO)
            .WithEstadoCuenta(Domain.Model.Entidades.Enums.EstadoCuenta.ACTIVA)
            .WithSaldo(1266523)
            .WithSaldoDisponible(1023654)
            .WithGMF(false)
            .WithTransacciones(new List<TransaccionEntity>())
            .Build(),
        };

        private Cuenta ObtenerCuentaTest() =>
            new Cuenta()
            {
                Id = "1",
                NumeroCuenta = "213123",
                IdCliente = "1",
                TipoCuenta = Domain.Model.Entidades.Enums.TipoCuenta.AHORRO,
                EstadoCuenta = Domain.Model.Entidades.Enums.EstadoCuenta.ACTIVA,
                Saldo = 100000,
                SaldoDisponible = 9630212,
                GMF = false,
                Transacciones = new List<Transaccion>()
            };
    }
}