using AutoMapper;
using BancoAmarillo.AppServices.Automapper;
using credinet.exception.middleware.models;
using Domain.Model.Entidades;
using Domain.Model.Entidades.Enums;
using Domain.UseCase.Cuentas;
using Domain.UseCase.Transacciones;
using EntryPoints.Grpc.Controller;
using EntryPoints.Grpc.Dtos.Protos;
using EntryPoints.Grpc.Dtos.Protos.Cuentas;
using Grpc.Core;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EntryPoints.ReactWeb.Tests.Controller
{
    public class CuentaControllerTest
    {
        private readonly Mock<ICuentasUseCase> _mockCuentaUseCase;
        private readonly IMapper _mapper;
        private readonly IConfigurationProvider _configProvider;
        private readonly CuentaController _controller;

        public CuentaControllerTest()
        {
            _mockCuentaUseCase = new();
            _configProvider = new MapperConfiguration(opt => opt.AddProfile<ConfigurationProfile>());
            _mapper = _configProvider.CreateMapper();
            _controller = new(_mockCuentaUseCase.Object, _mapper);
        }

        [Fact]
        public async Task CrearCuentaExitosa()
        {
            _mockCuentaUseCase.Setup(useCase => useCase.CrearCuentaAsync(It.IsAny<Cuenta>()))
                .ReturnsAsync(It.IsAny<Cuenta>());

            var response = await _controller.CrearCuenta(It.IsAny<CuentaProto>(), It.IsAny<ServerCallContext>());

            Assert.NotNull(response);
            Assert.IsType<RespuestaCuenta>(response);
        }

        [Fact]
        public async Task CrearCuentaBusinessException()
        {
            _mockCuentaUseCase.Setup(useCase => useCase.CrearCuentaAsync(It.IsAny<Cuenta>())).
                ThrowsAsync(new BusinessException(It.IsAny<string>(), It.IsAny<int>()));

            var response = await _controller.CrearCuenta(It.IsAny<CuentaProto>(), It.IsAny<ServerCallContext>());

            Assert.NotNull(response);
            Assert.IsType<RespuestaCuenta>(response);
            Assert.True(response.Error);
        }

        [Fact]
        public async Task CrearCuentaInternalException()
        {
            _mockCuentaUseCase.Setup(useCase => useCase.CrearCuentaAsync(It.IsAny<Cuenta>())).
                ThrowsAsync(It.IsAny<Exception>());

            var response = await _controller.CrearCuenta(It.IsAny<CuentaProto>(), It.IsAny<ServerCallContext>());

            Assert.NotNull(response);
            Assert.IsType<RespuestaCuenta>(response);
            Assert.True(response.Error);
        }

        [Fact]
        public async Task ObtenerCuentaIdExitosa()
        {
            _mockCuentaUseCase.Setup(useCase => useCase.ObtenerCuentaPorIdAsync(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<Cuenta>());

            var response = await _controller.ObtenerCuentaId(It.IsAny<CuentaRequest>(), It.IsAny<ServerCallContext>());

            Assert.NotNull(response);
            Assert.IsType<RespuestaCuenta>(response);
        }

        [Fact]
        public async Task ObtenerCuentaIdBusinessException()
        {
            _mockCuentaUseCase.Setup(useCase => useCase.ObtenerCuentaPorIdAsync(It.IsAny<string>())).
                ThrowsAsync(new BusinessException(It.IsAny<string>(), It.IsAny<int>()));

            var response = await _controller.ObtenerCuentaId(It.IsAny<CuentaRequest>(), It.IsAny<ServerCallContext>());

            Assert.NotNull(response);
            Assert.IsType<RespuestaCuenta>(response);
            Assert.True(response.Error);
        }

        [Fact]
        public async Task ObtenerCuentaIdInternalException()
        {
            _mockCuentaUseCase.Setup(useCase => useCase.ObtenerCuentaPorIdAsync(It.IsAny<string>())).
                ThrowsAsync(It.IsAny<Exception>());

            var response = await _controller.ObtenerCuentaId(It.IsAny<CuentaRequest>(), It.IsAny<ServerCallContext>());

            Assert.NotNull(response);
            Assert.IsType<RespuestaCuenta>(response);
            Assert.True(response.Error);
        }

        [Fact]
        public async Task ObtenerCuentaNumeroExitosa()
        {
            _mockCuentaUseCase.Setup(useCase => useCase.ObtenerCuentaPorNumeroCuentaAsync(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<Cuenta>());

            var response = await _controller.ObtenerCuentaNumero(It.IsAny<NumeroCuentaRequest>(), It.IsAny<ServerCallContext>());

            Assert.NotNull(response);
            Assert.IsType<RespuestaCuenta>(response);
        }

        [Fact]
        public async Task ObtenerCuentaNumeroBusinessException()
        {
            _mockCuentaUseCase.Setup(useCase => useCase.ObtenerCuentaPorNumeroCuentaAsync(It.IsAny<string>())).
                ThrowsAsync(new BusinessException(It.IsAny<string>(), It.IsAny<int>()));

            var response = await _controller.ObtenerCuentaNumero(It.IsAny<NumeroCuentaRequest>(), It.IsAny<ServerCallContext>());

            Assert.NotNull(response);
            Assert.IsType<RespuestaCuenta>(response);
            Assert.True(response.Error);
        }

        [Fact]
        public async Task ObtenerCuentaNumeroInternalException()
        {
            _mockCuentaUseCase.Setup(useCase => useCase.ObtenerCuentaPorNumeroCuentaAsync(It.IsAny<string>())).
                ThrowsAsync(It.IsAny<Exception>());

            var response = await _controller.ObtenerCuentaNumero(It.IsAny<NumeroCuentaRequest>(), It.IsAny<ServerCallContext>());

            Assert.NotNull(response);
            Assert.IsType<RespuestaCuenta>(response);
            Assert.True(response.Error);
        }


        [Fact]
        public async Task ActualizarCuentaExitosa()
        {
            _mockCuentaUseCase.Setup(useCase => useCase.ActualizarCuentaAsync(It.IsAny<Cuenta>()))
                .ReturnsAsync(It.IsAny<Cuenta>());

            var response = await _controller.ActualizarCuenta(It.IsAny<CuentaCompleta>(), It.IsAny<ServerCallContext>());

            Assert.NotNull(response);
            Assert.IsType<RespuestaCuenta>(response);
        }

        [Fact]
        public async Task ActualizarCuentaBusinessException()
        {
            _mockCuentaUseCase.Setup(useCase => useCase.ActualizarCuentaAsync(It.IsAny<Cuenta>())).
                ThrowsAsync(new BusinessException(It.IsAny<string>(), It.IsAny<int>()));

            var response = await _controller.ActualizarCuenta(It.IsAny<CuentaCompleta>(), It.IsAny<ServerCallContext>());

            Assert.NotNull(response);
            Assert.IsType<RespuestaCuenta>(response);
            Assert.True(response.Error);
        }

        [Fact]
        public async Task ActualizarCuentaInternalException()
        {
            _mockCuentaUseCase.Setup(useCase => useCase.ActualizarCuentaAsync(It.IsAny<Cuenta>())).
                ThrowsAsync(It.IsAny<Exception>());

            var response = await _controller.ActualizarCuenta(It.IsAny<CuentaCompleta>(), It.IsAny<ServerCallContext>());

            Assert.NotNull(response);
            Assert.IsType<RespuestaCuenta>(response);
            Assert.True(response.Error);
        }

        [Fact]
        public async Task CancelarCuentaExitosa()
        {
            _mockCuentaUseCase.Setup(useCase => useCase.CancelarCuentaAsync(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<bool>());

            var response = await _controller.CancelarCuenta(It.IsAny<CuentaRequest>(), It.IsAny<ServerCallContext>());

            Assert.NotNull(response);
            Assert.IsType<RespuestaCuenta>(response);
        }

        [Fact]
        public async Task CancelarCuentaBusinessException()
        {
            _mockCuentaUseCase.Setup(useCase => useCase.CancelarCuentaAsync(It.IsAny<string>())).
                ThrowsAsync(new BusinessException(It.IsAny<string>(), It.IsAny<int>()));

            var response = await _controller.CancelarCuenta(It.IsAny<CuentaRequest>(), It.IsAny<ServerCallContext>());

            Assert.NotNull(response);
            Assert.IsType<RespuestaCuenta>(response);
            Assert.True(response.Error);
        }

        [Fact]
        public async Task CancelarCuentaInternalException()
        {
            _mockCuentaUseCase.Setup(useCase => useCase.CancelarCuentaAsync(It.IsAny<string>())).
                ThrowsAsync(It.IsAny<Exception>());

            var response = await _controller.CancelarCuenta(It.IsAny<CuentaRequest>(), It.IsAny<ServerCallContext>());

            Assert.NotNull(response);
            Assert.IsType<RespuestaCuenta>(response);
            Assert.True(response.Error);
        }

        [Fact]
        public async Task ActivarCuentaAsyncExitosa()
        {
            _mockCuentaUseCase.Setup(useCase => useCase.ActivarCuentaAsync(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<Cuenta>());

            var response = await _controller.ActivarCuentaAsync(It.IsAny<CuentaRequest>(), It.IsAny<ServerCallContext>());

            Assert.NotNull(response);
            Assert.IsType<RespuestaCuenta>(response);
        }

        [Fact]
        public async Task ActivarCuentaAsyncBusinessException()
        {
            _mockCuentaUseCase.Setup(useCase => useCase.ActivarCuentaAsync(It.IsAny<string>())).
                ThrowsAsync(new BusinessException(It.IsAny<string>(), It.IsAny<int>()));

            var response = await _controller.ActivarCuentaAsync(It.IsAny<CuentaRequest>(), It.IsAny<ServerCallContext>());

            Assert.NotNull(response);
            Assert.IsType<RespuestaCuenta>(response);
            Assert.True(response.Error);
        }

        [Fact]
        public async Task ActivarCuentaAsyncInternalException()
        {
            _mockCuentaUseCase.Setup(useCase => useCase.ActivarCuentaAsync(It.IsAny<string>())).
                ThrowsAsync(It.IsAny<Exception>());

            var response = await _controller.ActivarCuentaAsync(It.IsAny<CuentaRequest>(), It.IsAny<ServerCallContext>());

            Assert.NotNull(response);
            Assert.IsType<RespuestaCuenta>(response);
            Assert.True(response.Error);
        }

        [Fact]
        public async Task InactivarCuentaAsyncExitosa()
        {
            _mockCuentaUseCase.Setup(useCase => useCase.InactivarCuentaAsync(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<bool>());

            var response = await _controller.InactivarCuentaAsync(It.IsAny<CuentaRequest>(), It.IsAny<ServerCallContext>());

            Assert.NotNull(response);
            Assert.IsType<RespuestaCuenta>(response);
        }

        [Fact]
        public async Task InactivarCuentaAsyncBusinessException()
        {
            _mockCuentaUseCase.Setup(useCase => useCase.InactivarCuentaAsync(It.IsAny<string>())).
                ThrowsAsync(new BusinessException(It.IsAny<string>(), It.IsAny<int>()));

            var response = await _controller.InactivarCuentaAsync(It.IsAny<CuentaRequest>(), It.IsAny<ServerCallContext>());

            Assert.NotNull(response);
            Assert.IsType<RespuestaCuenta>(response);
            Assert.True(response.Error);
        }

        [Fact]
        public async Task InactivarCuentaAsyncInternalException()
        {
            _mockCuentaUseCase.Setup(useCase => useCase.InactivarCuentaAsync(It.IsAny<string>())).
                ThrowsAsync(It.IsAny<Exception>());

            var response = await _controller.InactivarCuentaAsync(It.IsAny<CuentaRequest>(), It.IsAny<ServerCallContext>());

            Assert.NotNull(response);
            Assert.IsType<RespuestaCuenta>(response);
            Assert.True(response.Error);
        }

    }
}
