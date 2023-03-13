using AutoMapper;
using credinet.comun.models.Credits;
using credinet.exception.middleware.models;
using Domain.Model.Entidades;
using Domain.Model.Entidades.Enums;
using Domain.Model.Gateway;
using Domain.Model.Tests.Builders;
using Domain.UseCase.Clientes;
using DrivenAdapters.Mongo.Tests.Entities;
using Helpers.Commons.Exceptions;
using Moq;
using Xunit;

namespace Domain.UseCase.Tests
{
    public class ClienteUseCaseTest
    {
        private readonly Mock<IClienteRepository> _mockClienteRepository;
        private readonly Mock<IAuthRepository> _mockAuthRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ClienteUseCase _clienteUseCase;

        public ClienteUseCaseTest()
        {
            _mockClienteRepository = new();
            _mockAuthRepository = new();
            _mockMapper = new();
            _clienteUseCase = new(_mockClienteRepository.Object, _mockAuthRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Cliente_Use_Case_Crear_Cliente_Retorna_Cliente_Creado()
        {
            // Arrange
            var cliente = new Cliente
            {
                Nombre = "Juan",
                Apellido = "Pérez",
                Correo = "juan.perez@example.com",
                FechaNacimiento = new DateTime(1990, 1, 1),
                TipoIdentificacion = TipoIdentificacion.CEDULACIUDADANIA,
                NumeroIdentificacion = "1234567890",
                Estado = EstadoCliente.ACTIVO
            };

            _mockAuthRepository.Setup(repo => repo.RegistrarUsuario(It.IsAny<Usuario>()))
                .ReturnsAsync(new Usuario { Id = "123456", Correo = cliente.Correo });

            _mockClienteRepository.Setup(repo => repo.CrearCliente(It.IsAny<Cliente>()))
                .ReturnsAsync(cliente);

            _mockMapper.Setup(m => m.Map<Cliente>(It.IsAny<Cliente>())).Returns((Cliente c) => c);

            // Act
            var resultado = await _clienteUseCase.CrearCliente(cliente);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(cliente.Nombre, resultado.Nombre);
            Assert.Equal(Model.Entidades.Enums.EstadoCliente.ACTIVO, resultado.Estado);
            Assert.Empty(resultado.Cuentas);
            Assert.Equal(cliente.Correo, resultado.Correo);
        }

        [Fact]
        public async Task Cliente_Use_Case_Obtener_Cliente_Por_Id_Retorna_Cliente_Encontrado()
        {
            string nombre = "compra celular";
            string correo = "correo";
            List<Cuenta> cuentas = new List<Cuenta>();

            _mockClienteRepository.Setup(repository => repository.ObtenerClientePorId(It.IsAny<string>()))
                .ReturnsAsync(ObtenerClienteTest(nombre, correo, cuentas));

            Cliente clienteEncontrado = await _clienteUseCase.ObtenerClientePorId(It.IsAny<string>());

            _mockClienteRepository.Verify(repository => repository.ObtenerClientePorId(It.IsAny<string>()), Times.Once);

            Assert.NotNull(clienteEncontrado);
            Assert.Equal(correo, clienteEncontrado.Correo);
        }

        [Fact]
        public async Task Cliente_Use_Case_Obtener_Clientes_Retorna_lista_De_Clientes()
        {
            _mockClienteRepository.Setup(repository => repository.ObtenerClientes())
                .ReturnsAsync(ObtenerClientesTest);

            List<Cliente> clientes = await _clienteUseCase.ObtenerClientes();

            _mockClienteRepository.Verify(repository => repository.ObtenerClientes(), Times.Once);

            Assert.NotNull(clientes);
        }

        [Fact]
        public async Task Cliente_Use_Case_Actualizar_Cliente_Retorna_Cliente_Actualizado()
        {
            string idCliente = "1";
            Cliente cliente = new ClienteBuilder()
                .WithId(idCliente)
                .WithNombre("John")
                .WithCorreo("John@correo.com")
                .WithCuentas(new List<Cuenta>())
                .Build();
            _mockClienteRepository.Setup(repository => repository.ObtenerClientePorId(It.IsAny<string>()))
                .ReturnsAsync(cliente);
            _mockClienteRepository.Setup(repository => repository.ActualizarCliente(It.IsAny<string>(), It.IsAny<Cliente>()))
                .ReturnsAsync(cliente);

            Cliente clienteActualizado = await _clienteUseCase.ActualizarCliente(idCliente, cliente);

            _mockClienteRepository.Verify(repository => repository.ActualizarCliente(It.IsAny<string>(), It.IsAny<Cliente>()), Times.Once);
            Assert.NotNull(clienteActualizado);
            Assert.Equal(idCliente, clienteActualizado.Id);
        }

        [Fact]
        public async Task Cliente_Use_Case_Actualizar_Cliente_Retorna_Error_Cliente_No_Encontrado()
        {
            BusinessException excepcion = await Assert.ThrowsAsync<BusinessException>(async () => await _clienteUseCase.ActualizarCliente(null, null));

            _mockClienteRepository.Verify(repository => repository.ActualizarCliente(It.IsAny<string>(), It.IsAny<Cliente>()), Times.Never());

            Assert.Equal((int)TipoExcepcionNegocio.ExceptionClienteNoEncontrado, excepcion.code);
        }

        [Fact]
        public async Task Cliente_Use_Case_Eliminar_Cliente_Retorna_Cliente_Eliminado()
        {
            string idCliente = "1";
            Cliente cliente = new ClienteBuilder()
                .WithId(idCliente)
                .WithNombre("John")
                .WithCorreo("John@correo.com")
                .WithCuentas(new List<Cuenta>())
                .Build();
            _mockClienteRepository.Setup(repository => repository.ObtenerClientePorId(It.IsAny<string>()))
                .ReturnsAsync(cliente);

            _mockClienteRepository.Setup(repository => repository.EliminarCliente(It.IsAny<string>()))
                .ReturnsAsync(true);

            Cliente clienteEliminado = await _clienteUseCase.EliminarCliente(idCliente);

            _mockClienteRepository.Verify(repository => repository.EliminarCliente(It.IsAny<string>()), Times.Once);
            Assert.NotNull(clienteEliminado);
            Assert.Equal(idCliente, clienteEliminado.Id);
        }

        [Fact]
        public async Task Cliente_Use_Case_Eliminar_Cliente_Retorna_Error_Cliente_No_Encontrado()
        {
            BusinessException excepcion = await Assert.ThrowsAsync<BusinessException>(async () => await _clienteUseCase.EliminarCliente(null));

            _mockClienteRepository.Verify(repository => repository.EliminarCliente(It.IsAny<string>()), Times.Never());

            Assert.Equal((int)TipoExcepcionNegocio.ExceptionClienteNoEncontrado, excepcion.code);
        }

        private List<Cliente> ObtenerClientesTest() => new()
        {
            new ClienteBuilder()
            .WithId("1")
            .WithNombre("John")
            .WithCorreo("John@correo.com")
            .WithCuentas(new List<Cuenta>())
            .Build(),
            new ClienteBuilder()
            .WithId("2")
            .WithNombre("Edward")
            .WithCorreo("Edward@correo.com")
            .WithCuentas(new List<Cuenta>())
            .Build()
        };

        private Cliente ObtenerClienteTest(string nombre, string correo, List<Cuenta> cuentas)
        {
            return new ClienteBuilder()
                .WithId("123")
                .WithNombre(nombre)
                .WithCorreo(correo)
                .WithCuentas(cuentas)
                .Build();
        }
    }
}