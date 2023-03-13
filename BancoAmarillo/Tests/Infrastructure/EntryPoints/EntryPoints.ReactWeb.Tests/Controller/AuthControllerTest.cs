using AutoMapper;
using BancoAmarillo.AppServices.Automapper;
using credinet.exception.middleware.models;
using Domain.Model.Entidades;
using Domain.Model.Entidades.Enums;
using Domain.Model.Gateway;
using Domain.UseCase.Auth;
using Domain.UseCase.Clientes;
using Domain.UseCase.Cuentas;
using EntryPoints.Grpc.Controller;
using EntryPoints.Grpc.Dtos.Protos;
using EntryPoints.Grpc.Dtos.Protos.Cliente;
using EntryPoints.ReactWeb.Tests.Clientes;
using FluentValidation;
using FluentValidation.Results;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EntryPoints.ReactWeb.Tests.Controller
{
    public class AuthControllerTest
    {
        private readonly Mock<IAuthUseCase> _authUseCase;
        private readonly IMapper _mapper;
        private readonly IConfigurationProvider _configProvider;
        private readonly Mock<IValidator<UsuarioProto>> _validator;
        private readonly AuthController _authController;

        public AuthControllerTest()
        {
            _authUseCase = new();
            _configProvider = new MapperConfiguration(opt => opt.AddProfile<ConfigurationProfile>());
            _mapper = _configProvider.CreateMapper();
            _validator = new();
            _authController = new(_authUseCase.Object, _mapper, _validator.Object);
        }

        [Fact]
        public async Task CrearUsuarioControllerTest()
        {
            var request = ObtenerRequestParaTest();
            var usuario = ObtenerUsuarioParaTest();

            var validResult = new ValidationResult();
            _validator.Setup(val => val.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(validResult);
            _authUseCase.Setup(useCase => useCase.CrearUsuario(It.IsAny<Usuario>())).ReturnsAsync(new AccesToken { AccessToken="123456"});

            var httpContext = new DefaultHttpContext();
            ClaimsPrincipal claims = new ClaimsPrincipal();
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            Claim claim = new Claim("id", "1");
            claimsIdentity.AddClaim(claim);
            httpContext.User = claims;
            httpContext.User.AddIdentity(claimsIdentity);
            var serverCallContext = TestServerCallContext.Create();
            serverCallContext.UserState["__HttpContext"] = httpContext;

            var respuesta = await _authController.CrearUsuario(request, serverCallContext);

            Assert.Equal("Acceso Autorizado", respuesta.Mensaje);
            Assert.False(respuesta.Error);
        }

        [Fact]
        public async Task IniciarSesionControllerTest()
        {
            var request = ObtenerRequestParaTest();

            _authUseCase.Setup(useCase => useCase.IniciarSesion(It.IsAny<Usuario>())).ReturnsAsync(new AccesToken { AccessToken = "123456" });

            var respuesta = await _authController.IniciarSesion(request, It.IsAny<ServerCallContext>());

            Assert.Equal("Acceso Autorizado", respuesta.Mensaje);
            Assert.False(respuesta.Error);
        }

        [Fact]
        public async Task IniciarSesionBussinessException()
        {
            var request = ObtenerRequestParaTest();

            _authUseCase.Setup(useCase => useCase.IniciarSesion(It.IsAny<Usuario>())).ThrowsAsync(new BusinessException(It.IsAny<string>(), It.IsAny<int>()));

            var respuesta = await _authController.IniciarSesion(request, It.IsAny<ServerCallContext>());

            Assert.NotNull(respuesta);
            Assert.True(respuesta.Error);
        }

        [Fact]
        public async Task IniciarSesionConExceptionDelServidor()
        {
            var request = ObtenerRequestParaTest();

            _authUseCase.Setup(useCase => useCase.IniciarSesion(It.IsAny<Usuario>())).ThrowsAsync(new Exception(It.IsAny<string>()));

            var respuesta = await _authController.IniciarSesion(request, It.IsAny<ServerCallContext>());

            Assert.NotNull(respuesta);
            Assert.True(respuesta.Error);
        }


        public UsuarioProto ObtenerRequestParaTest()
        {
            return new UsuarioProto
            {
                Correo = "swaed@gmail.com",
                Clave = "123456.pa"
            };
        }

        public Usuario ObtenerUsuarioParaTest()
        {
            return new Usuario
            {
                Id = "1",
                Correo = "swaed@gmail.com",
                Clave = "123456.pa",
                Rol = Rol.ADMIN
            };
        }
    }
}
