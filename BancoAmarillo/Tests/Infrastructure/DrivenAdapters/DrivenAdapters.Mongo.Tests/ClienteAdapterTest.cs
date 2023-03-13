using AutoMapper;
using BancoAmarillo.AppServices.Automapper;
using credinet.comun.models.Credits;
using credinet.exception.middleware.models;
using Domain.Model.Entidades;
using Domain.Model.Entidades.Enums;
using Domain.Model.Gateway;
using Domain.Model.Tests.Builders;
using DrivenAdapters.Mongo.Adaptadores;
using DrivenAdapters.Mongo.Entities;
using DrivenAdapters.Mongo.Tests.Entities;
using MongoDB.Driver;
using Moq;
using Xunit;
using Xunit.Sdk;

namespace DrivenAdapters.Mongo.Tests
{
    public class ClienteAdapterTest
    {
        private readonly Mock<IContext> _mockContext;
        private readonly IMapper _mockMapper;
        private readonly Mock<IMongoCollection<ClienteEntity>> _mockColeccionClientes;
        private readonly Mock<IAsyncCursor<ClienteEntity>> _mockClienteCursor;
        private readonly IConfigurationProvider _configurationProvider;

        public ClienteAdapterTest()
        {
            _mockContext = new();
            _mockColeccionClientes = new();
            _mockClienteCursor = new();
            _configurationProvider = new MapperConfiguration(options => options.AddProfile<ConfigurationProfile>());
            _mockMapper = _configurationProvider.CreateMapper();
            _mockColeccionClientes.Object.InsertMany(ObtenerClientesTest());
            _mockClienteCursor.SetupSequence(item => item.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true).Returns(false);
            _mockClienteCursor.SetupSequence(item => item.MoveNextAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(true)).Returns(Task.FromResult(false));
        }

        [Fact]
        public async Task Cliente_Adapter_Crear_Cliente_Retorna_Cliente_Creado()
        {
            _mockColeccionClientes.Setup(op => op.InsertOneAsync(It.IsAny<ClienteEntity>(), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()));

            _mockContext.Setup(context => context.Clientes).Returns(_mockColeccionClientes.Object);
            //_mockMapper.Setup(m => m.Map<ClienteEntity>(It.IsAny<ClienteEntity>())).Returns((ClienteEntity c) => c);
            //_mockMapper.Setup(m => m.Map<Cliente>(It.IsAny<Cliente>())).Returns((Cliente c) => c);
            var clienteAdapter = new ClienteAdapter(_mockContext.Object, _mockMapper);

            var result = await clienteAdapter.CrearCliente(ObtenerClienteTest());

            Assert.NotNull(result);
            Assert.Equal("john@correo.com", result.Correo);
            Assert.IsType<Cliente>(result);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        public async Task Cliente_Adapter_Obtener_Cliente_Por_Id_Retorna_Cliente_Encontrado(string idCliente)
        {
            List<ClienteEntity> listaClientes = new() { ObtenerClienteEntityTest() };
            _mockClienteCursor.Setup(item => item.Current).Returns(listaClientes);

            _mockColeccionClientes.Setup(op => op.FindAsync(It.IsAny<FilterDefinition<ClienteEntity>>(),
                It.IsAny<FindOptions<ClienteEntity, ClienteEntity>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_mockClienteCursor.Object);

            _mockContext.Setup(context => context.Clientes).Returns(_mockColeccionClientes.Object);

            var clienteRepository = new ClienteAdapter(_mockContext.Object, _mockMapper);

            var result = await clienteRepository.ObtenerClientePorId(idCliente);

            Assert.NotNull(result);
            Assert.IsType<Cliente>(result);
        }

        [Fact]
        public async Task Cliente_Adapter_Obtener_Clientes_Retorna_Lista_De_Clientes()
        {
            List<ClienteEntity> listaClientes = new() { ObtenerClienteEntityTest() };
            _mockClienteCursor.Setup(item => item.Current).Returns(listaClientes);

            _mockColeccionClientes.Setup(op => op.FindAsync(It.IsAny<FilterDefinition<ClienteEntity>>(),
                It.IsAny<FindOptions<ClienteEntity, ClienteEntity>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_mockClienteCursor.Object);

            _mockContext.Setup(context => context.Clientes).Returns(_mockColeccionClientes.Object);

            var clienteRepository = new ClienteAdapter(_mockContext.Object, _mockMapper);

            var result = await clienteRepository.ObtenerClientes();

            Assert.NotNull(result);
            Assert.IsType<List<Cliente>>(result);
        }

        [Fact]
        public async Task Cliente_Adapter_Actualizar_Cliente_Retorna_Cliente_Actualizado()
        {
            string idCliente = "1";
            _mockColeccionClientes.Setup(op => op.FindAsync(It.IsAny<FilterDefinition<ClienteEntity>>(),
                It.IsAny<FindOptions<ClienteEntity, ClienteEntity>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_mockClienteCursor.Object);
            _mockColeccionClientes.Setup(op => op.ReplaceOneAsync(It.IsAny<FilterDefinition<ClienteEntity>>(),
              It.IsAny<ClienteEntity>(), It.IsAny<ReplaceOptions>(), It.IsAny<CancellationToken>()));

            _mockContext.Setup(context => context.Clientes).Returns(_mockColeccionClientes.Object);

            var clienteRepository = new ClienteAdapter(_mockContext.Object, _mockMapper);
            var result = await clienteRepository.ActualizarCliente(idCliente, ObtenerClienteTest());

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Cliente_Adapter_Eliminar_Cliente_Retorna_Cliente_Actualizado()
        {
            string idCliente = "1";
            _mockColeccionClientes.Setup(op => op.UpdateOneAsync(It.IsAny<FilterDefinition<ClienteEntity>>(),
              It.IsAny<UpdateDefinition<ClienteEntity>>(), It.IsAny<UpdateOptions>(), It.IsAny<CancellationToken>()));

            _mockContext.Setup(context => context.Clientes).Returns(_mockColeccionClientes.Object);

            var clienteRepository = new ClienteAdapter(_mockContext.Object, _mockMapper);
            var result = await clienteRepository.EliminarCliente(idCliente);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Cliente_Adapter_Eliminar_Cliente_Retorna_Error()
        {
            _mockColeccionClientes.Setup(op => op.UpdateOneAsync(It.IsAny<FilterDefinition<ClienteEntity>>(),
              It.IsAny<UpdateDefinition<ClienteEntity>>(), It.IsAny<UpdateOptions>(), It.IsAny<CancellationToken>())).Throws(new System.Exception());

            _mockContext.Setup(context => context.Clientes).Returns(_mockColeccionClientes.Object);

            var clienteRepository = new ClienteAdapter(_mockContext.Object, _mockMapper);
            //var result = await clienteRepository.EliminarCliente(It.IsAny<string>());
            var error = Assert.ThrowsAsync<System.Exception>(() => clienteRepository.EliminarCliente(It.IsAny<string>()));

            Assert.IsType<System.Exception>(error.Result);
        }

        private List<ClienteEntity> ObtenerClientesTest() => new()
        {
            new ClienteEntityBuilder()
            .WithId("1")
            .WithNombre("John")
            .WithCorreo("John@correo.com")
            .WithCuentas(new List<CuentaEntity>())
            .Build(),
            new ClienteEntityBuilder()
            .WithId("2")
            .WithNombre("Edward")
            .WithCorreo("Edward@correo.com")
            .WithCuentas(new List<CuentaEntity>())
            .Build(),
        };

        private ClienteEntity ObtenerClienteEntityTest() =>
            new ClienteEntityBuilder()
            .WithId("1")
            .WithNombre("John")
            .WithCorreo("John@correo.com")
            .WithCuentas(new List<CuentaEntity>())
            .Build();

        private Cliente ObtenerClienteTest()
        {
            var cliente = new Cliente
            {
                Id = "1",
                Nombre = "John",
                Apellido = "Acevedo",
                Correo = "john@correo.com",
                FechaNacimiento = new DateTime(1990, 1, 1),
                TipoIdentificacion = TipoIdentificacion.CEDULACIUDADANIA,
                NumeroIdentificacion = "1234567890",
                Estado = EstadoCliente.ACTIVO
            };
            return cliente;
        }
    }
}