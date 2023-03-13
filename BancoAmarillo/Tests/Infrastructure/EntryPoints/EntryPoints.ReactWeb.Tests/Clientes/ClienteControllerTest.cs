using AutoMapper;
using BancoAmarillo.AppServices.Automapper;
using credinet.exception.middleware.models;
using Domain.Model.Entidades;
using Domain.Model.Entidades.Enums;
using Domain.UseCase.Clientes;
using Domain.UseCase.Cuentas;
using EntryPoints.Grpc.Controller;
using EntryPoints.Grpc.Dtos.Protos.Cliente;
using EntryPoints.Grpc.Validaciones;
using FluentValidation;
using FluentValidation.Results;
using Grpc.Core;
using Helpers.Commons.Exceptions;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace EntryPoints.ReactWeb.Tests.Clientes
{
    public class ClienteControllerTest
    {
        private readonly Mock<IClienteUseCase> _clienteUseCase;
        private readonly Mock<ICuentasUseCase> _cuentaUseCase;
        private readonly IMapper _mapper;
        private readonly IConfigurationProvider _configProvider;
        private readonly Mock<IValidator<ClienteRequest>> _validator;
        private readonly ClienteController _clienteController;
        private readonly Mock<ServerCallContext> _mockContext;

        public ClienteControllerTest()
        {
            _clienteUseCase = new();
            _cuentaUseCase = new();
            _mockContext = new Mock<ServerCallContext>();
            _configProvider = new MapperConfiguration(opt => opt.AddProfile<ConfigurationProfile>());
            _mapper = _configProvider.CreateMapper();
            _validator = new();
            _clienteController = new(_clienteUseCase.Object, _mapper, _validator.Object, _cuentaUseCase.Object);
        }

        [Fact]
        public async Task Cliente_Controller_Crear_Cliente_Retorna_Cliente_Creado()
        {
            var request = ObtenerClienteRequestParaTest();
            var cliente = ObtenerClienteParaTest();

            var validResult = new ValidationResult();
            _validator.Setup(val => val.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(validResult);
            _clienteUseCase.Setup(useCase => useCase.CrearCliente(It.IsAny<Cliente>())).ReturnsAsync(cliente);

            var httpContext = new DefaultHttpContext();
            ClaimsPrincipal claims = new ClaimsPrincipal();
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            Claim claim = new Claim("id", "1");
            claimsIdentity.AddClaim(claim);
            httpContext.User = claims;
            httpContext.User.AddIdentity(claimsIdentity);
            var serverCallContext = TestServerCallContext.Create();
            serverCallContext.UserState["__HttpContext"] = httpContext;

            var respuesta = await _clienteController.CrearCliente(request, serverCallContext);

            Assert.IsType<ClienteResponse>(respuesta.Data);
        }

        [Fact]
        public async Task Cliente_Controller_Obtener_Cliente_Retorna_Cliente_Encontrado()
        {
            var cliente = ObtenerClienteParaTest();
            var idClienteRequest = new IdCliente()
            {
                Id = "2"
            };
            _clienteUseCase.Setup(useCase => useCase.ObtenerClientePorId(It.IsAny<string>())).ReturnsAsync(cliente);

            var respuesta = await _clienteController.ObtenerCliente(idClienteRequest, _mockContext.Object);

            Assert.IsType<ClienteResponse>(respuesta.Data);
        }

        [Fact]
        public async Task Cliente_Controller_Obtener_Cliente_Retorna_Error()
        {
            var cliente = ObtenerClienteParaTest();
            var idClienteRequest = new IdCliente()
            {
                Id = "2"
            };
            _clienteUseCase.Setup(useCase => useCase.ObtenerClientePorId(It.IsAny<string>())).Throws(It.IsAny<Exception>());

            var respuesta = await _clienteController.ObtenerCliente(idClienteRequest, _mockContext.Object);

            Assert.True(respuesta.Error);
        }

        [Fact]
        public async Task Cliente_Controller_Obtener_Clientes_Retorna_Lista_De_Clientes_Encontrados()
        {
            var clientes = ObtenerClientesParaTest();
            var empty = new Empty();
            _clienteUseCase.Setup(useCase => useCase.ObtenerClientes()).ReturnsAsync(clientes);

            var respuesta = await _clienteController.ObtenerClientes(empty, _mockContext.Object);

            Assert.IsType<ListaClientes>(respuesta);
        }

        [Fact]
        public async Task Cliente_Controller_Eliminar_Cliente_Retorna_Cliente_Actualizado()
        {
            var cliente = ObtenerClienteParaTest();
            var idClienteRequest = new IdCliente()
            {
                Id = "2"
            };
            _clienteUseCase.Setup(useCase => useCase.EliminarCliente(It.IsAny<string>())).ReturnsAsync(cliente);

            var respuesta = await _clienteController.EliminarCliente(idClienteRequest, _mockContext.Object);

            Assert.IsType<ClienteResponse>(respuesta.Data);
        }

        [Fact]
        public async Task Cliente_Controller_Actualizar_Cliente_Retorna_Cliente_Actualizado()
        {
            var request = ObtenerClienteRequestParaTest();
            var requestActualizar = new ActualizarClienteProto()
            {
                ClienteId = "1",
                Nombre = "Juan",
                Apellido = "Perez"
            };
            var cliente = ObtenerClienteParaTest();
            var validResult = new ValidationResult();
            _validator.Setup(val => val.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(validResult);
            _clienteUseCase.Setup(useCase => useCase.ActualizarCliente(It.IsAny<string>(), It.IsAny<Cliente>())).ReturnsAsync(cliente);

            var httpContext = new DefaultHttpContext();
            ClaimsPrincipal claims = new ClaimsPrincipal();
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            Claim claim = new Claim("id", "1");
            claimsIdentity.AddClaim(claim);
            httpContext.User = claims;
            httpContext.User.AddIdentity(claimsIdentity);
            var serverCallContext = TestServerCallContext.Create();
            serverCallContext.UserState["__HttpContext"] = httpContext;

            var respuesta = await _clienteController.ActualizarCliente(requestActualizar, serverCallContext);

            Assert.IsType<ClienteResponse>(respuesta.Data);
        }

        [Fact]
        public async Task Cliente_Controller_Obtener_Cuentas_Cliente_Retorna_Lista_De_Cuentas_Del_Cliente()
        {
            var cliente = ObtenerClienteCuentasParaTest();
            var idClienteRequest = new IdCliente()
            {
                Id = "2"
            };
            _clienteUseCase.Setup(useCase => useCase.ObtenerClientePorId(It.IsAny<string>())).ReturnsAsync(cliente);

            var respuesta = await _clienteController.ObtenerCuentasCliente(idClienteRequest, _mockContext.Object);

            Assert.IsType<ListaCuentasCliente>(respuesta);
        }

        [Fact]
        public async Task Cliente_Controller_Marcar_Cuenta_GMF_Retorna_Cliente_Actualizado()
        {
            var request = ObtenerClienteRequestParaTest();
            var clienteCuentas = ObtenerClienteCuentasParaTest();
            var datosRequest = new DatosMarcarCuentaGMF()
            {
                IdCliente = "1",
                IdCuenta = "2",
            };

            var cuenta = new Cuenta()
            {
                Id = "2",
                NumeroCuenta = "2324234234",
                IdCliente = "23454",
                TipoCuenta = TipoCuenta.AHORRO,
                EstadoCuenta = EstadoCuenta.ACTIVA,
                Saldo = 6010110,
                SaldoDisponible = 700000,
                GMF = false,
                FechaCreacion = DateTime.UtcNow.ToLocalTime(),
                FechaModificacion = DateTime.UtcNow.ToLocalTime()
            };
            var validResult = new ValidationResult();

            _clienteUseCase.Setup(useCase => useCase.ObtenerClientePorId(It.IsAny<string>())).ReturnsAsync(clienteCuentas);
            _validator.Setup(val => val.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(validResult);
            _cuentaUseCase.Setup(useCase => useCase.MarcarCuentaGMFAsync(It.IsAny<string>())).ReturnsAsync(cuenta);
            _clienteUseCase.Setup(useCase => useCase.ActualizarCliente(It.IsAny<string>(), It.IsAny<Cliente>())).ReturnsAsync(clienteCuentas);

            var httpContext = new DefaultHttpContext();
            ClaimsPrincipal claims = new ClaimsPrincipal();
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            Claim claim = new Claim("id", "1");
            claimsIdentity.AddClaim(claim);
            httpContext.User = claims;
            httpContext.User.AddIdentity(claimsIdentity);
            var serverCallContext = TestServerCallContext.Create();
            serverCallContext.UserState["__HttpContext"] = httpContext;

            var respuesta = await _clienteController.MarcarCuentaGMF(datosRequest, serverCallContext);

            Assert.IsType<ClienteResponse>(respuesta.Data);
        }

        [Fact]
        public async Task Cliente_Controller_Marcar_Cuenta_GMF_Retorna_Error_Cuenta_No_Encontrada()
        {
            var datosRequest = new DatosMarcarCuentaGMF()
            {
                IdCliente = "1",
                IdCuenta = "2202121",
            };
            _clienteUseCase.Setup(useCase => useCase.ObtenerClientePorId(It.IsAny<string>()))
                .ReturnsAsync(ObtenerClienteCuentasParaTest);

            var result = await _clienteController.MarcarCuentaGMF(datosRequest, _mockContext.Object);

            Assert.True(result.Error);
        }

        private ClienteRequest ObtenerClienteRequestParaTest()
        {
            return new ClienteRequest
            {
                TipoIdentificacion = TipoIdenticacion.Cedulaciudadnia,
                NumeroIdentificacion = "1234567890",
                Nombre = "Juan",
                Apellido = "Pérez",
                Correo = "juan.perez@example.com",
                FechaNacimiento = new DateTime(1990, 1, 1).ToString()
            };
        }

        private Cliente ObtenerClienteParaTest()
        {
            return new Cliente
            {
                Id = "1",
                TipoIdentificacion = TipoIdentificacion.CEDULACIUDADANIA,
                NumeroIdentificacion = "1234567890",
                Nombre = "Juan",
                Apellido = "Pérez",
                Correo = "juan.perez@example.com",
                FechaNacimiento = new DateTime(1990, 1, 1),
                FechaCreacion = DateTime.UtcNow.ToLocalTime(),
                FechaModificacion = DateTime.UtcNow.ToLocalTime(),
                UsuarioModificacion = "ADMIN",
                Estado = EstadoCliente.ACTIVO,
                Cuentas = { }
            };
        }

        private Cliente ObtenerClienteCuentasParaTest()
        {
            return new Cliente
            {
                Id = "1",
                TipoIdentificacion = TipoIdentificacion.CEDULACIUDADANIA,
                NumeroIdentificacion = "1234567890",
                Nombre = "Juan",
                Apellido = "Pérez",
                Correo = "juan.perez@example.com",
                FechaNacimiento = new DateTime(1990, 1, 1),
                FechaCreacion = DateTime.UtcNow.ToLocalTime(),
                FechaModificacion = DateTime.UtcNow.ToLocalTime(),
                UsuarioModificacion = "ADMIN",
                Estado = EstadoCliente.ACTIVO,
                Cuentas = new List<Cuenta> { new Cuenta()
                    {
                         Id = "1",
                         NumeroCuenta = "2324234234",
                         IdCliente = "23454",
                         TipoCuenta = TipoCuenta.AHORRO,
                         EstadoCuenta = EstadoCuenta.ACTIVA,
                         Saldo = 6010110,
                         SaldoDisponible = 700000,
                         GMF = false,
                         FechaCreacion = DateTime.UtcNow.ToLocalTime(),
                         FechaModificacion = DateTime.UtcNow.ToLocalTime()
                    },
                     new Cuenta()
                    {
                         Id = "2",
                         NumeroCuenta = "2342342342",
                         IdCliente = "23454",
                         TipoCuenta = TipoCuenta.AHORRO,
                         EstadoCuenta = EstadoCuenta.INACTIVA,
                         Saldo = 6010110,
                         SaldoDisponible = 700000,
                         GMF = false,
                         FechaCreacion = DateTime.UtcNow.ToLocalTime(),
                         FechaModificacion = DateTime.UtcNow.ToLocalTime()
                    }}
            };
        }

        private List<Cliente> ObtenerClientesParaTest() => new() {
         new Cliente()
            {
                Id = "1",
                TipoIdentificacion = TipoIdentificacion.CEDULACIUDADANIA,
                NumeroIdentificacion = "1234567890",
                Nombre = "Juan",
                Apellido = "Pérez",
                Correo = "juan.perez@example.com",
                FechaNacimiento = new DateTime(1990, 1, 1),
                FechaCreacion = DateTime.UtcNow.ToLocalTime(),
                FechaModificacion = DateTime.UtcNow.ToLocalTime(),
                UsuarioModificacion = "ADMIN",
                Estado = EstadoCliente.ACTIVO,
                Cuentas = {
                }
            },
         new Cliente()
            {
                Id = "2",
                TipoIdentificacion = TipoIdentificacion.CEDULACIUDADANIA,
                NumeroIdentificacion = "154652",
                Nombre = "Sandra",
                Apellido = "Lopez",
                Correo = "sandra.lopez@example.com",
                FechaNacimiento = new DateTime(1990, 1, 1),
                FechaCreacion = DateTime.UtcNow.ToLocalTime(),
                FechaModificacion = DateTime.UtcNow.ToLocalTime(),
                UsuarioModificacion = "ADMIN",
                Estado = EstadoCliente.ACTIVO,
                Cuentas = { }
            }
         };
    }
}