using AutoMapper;
using BancoAmarillo.AppServices.Automapper;
using credinet.exception.middleware.models;
using Domain.Model.Entidades;
using Domain.Model.Entidades.Enums;
using Domain.UseCase.Transacciones;
using EntryPoints.Grpc.Controller;
using EntryPoints.Grpc.Dtos.Protos;
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
    public class TransaccionesControllerTest
    {
        private readonly Mock<ITransaccionesUseCase> _mockTransaccionesUseCase;
        private readonly IMapper _mapper;
        private readonly IConfigurationProvider _configProvider;
        private readonly TransaccionController _controller;

        public TransaccionesControllerTest()
        {
            _mockTransaccionesUseCase = new();
            _configProvider = new MapperConfiguration(opt => opt.AddProfile<ConfigurationProfile>());
            _mapper = _configProvider.CreateMapper();
            _controller = new(_mockTransaccionesUseCase.Object, _mapper);
        }

        [Fact]
        public async Task RealizarTransaccionExitosa()
        {
            _mockTransaccionesUseCase.Setup(useCase => useCase.RealizarTransaccion(It.IsAny<Transaccion>())).ReturnsAsync(It.IsAny<Transaccion>());

            var response = await _controller.RealizarTransaccion(It.IsAny<TransaccionRequest>(), It.IsAny<ServerCallContext>()); 

            Assert.NotNull(response);
            Assert.IsType<TransaccionRespuesta>(response);
        }

        [Fact]
        public async Task RealizarTransaccionBusinessException()
        {
            var transaccionRequest = ObtenerTransaccionRequestParaTest();
            var transaccion = ObtenerTransaccionParaTest();
            _mockTransaccionesUseCase.Setup(useCase => useCase.RealizarTransaccion(It.IsAny<Transaccion>())).
                ThrowsAsync(new BusinessException(It.IsAny<string>(), It.IsAny<int>()));

            var response = await _controller.RealizarTransaccion(It.IsAny<TransaccionRequest>(), It.IsAny<ServerCallContext>());

            Assert.NotNull(response);
            Assert.IsType<TransaccionRespuesta>(response);
            Assert.True(response.Error);
        }

        [Fact]
        public async Task RealizarTransaccionInternalException()
        {
            _mockTransaccionesUseCase.Setup(useCase => useCase.RealizarTransaccion(It.IsAny<Transaccion>())).
                ThrowsAsync(It.IsAny<Exception>());

            var response = await _controller.RealizarTransaccion(It.IsAny<TransaccionRequest>(), It.IsAny<ServerCallContext>());

            Assert.NotNull(response);
            Assert.IsType<TransaccionRespuesta>(response);
            Assert.True(response.Error);
        }

        public TransaccionRequest ObtenerTransaccionRequestParaTest()
        {
            return new TransaccionRequest()
            {
                IdCuentaEmisora = "1",
                IdCuentaReceptora = "2",
                TipoTransaccion = TipoTransaccionEnum.Transferencia,
                Valor = -10,
            };
        }

        public Transaccion ObtenerTransaccionParaTest()
        {
            return new Transaccion()
            {
                Id = "1",
                IdCuentaEmisora = "1",
                IdCuentaReceptora = "2",
                TipoTransaccion = TipoTransaccion.TRANSFERENCIA,
                Valor = -10,
                FechaMovimiento = DateTime.UtcNow.ToLocalTime(),
                TipoMovimiento = TipoMovimiento.CREDITO
            };
        }
    }
}
