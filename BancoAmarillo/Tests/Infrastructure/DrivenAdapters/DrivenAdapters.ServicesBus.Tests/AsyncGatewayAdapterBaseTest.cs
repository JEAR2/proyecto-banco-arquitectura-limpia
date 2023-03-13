using Castle.Components.DictionaryAdapter.Xml;
using Domain.Model.Interfaces;
using DrivenAdapters.ServiceBus.Base;
using Helpers.ObjectsUtils;
using Helpers.ObjectsUtils.ApplicationSettings;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Extensions.Options;
using Moq;
using org.reactivecommons.api;
using org.reactivecommons.api.domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

namespace DrivenAdapters.ServiceBus.Tests
{
    public class AsyncGatewayAdapterBaseTest
    {
        private readonly Mock<IManageEventsUseCase> _mockManageEventsUseCase;
        private readonly Mock<IOptions<ConfiguradorAppSettings>> _mockConfiguradorAppSettings;
        private TData tdata;
        private readonly Mock<IDirectAsyncGateway<TData>> _mockDirectAsyncGateway;

        private readonly AsyncGatewayAdapterBase _adapter;

        public AsyncGatewayAdapterBaseTest()
        {
            _mockManageEventsUseCase = new();
            ConfiguradorAppSettings appSettings = ObtenerConfigParaTest();
            _mockConfiguradorAppSettings = new();
            _mockConfiguradorAppSettings.Setup(config => config.Value).Returns(appSettings);
            _mockDirectAsyncGateway = new();
            _adapter = new(_mockManageEventsUseCase.Object, _mockConfiguradorAppSettings.Object);
        }

        [Fact]
        public async Task HandleSendCommandAsyncTestExitoso()
        {
            var currentMethod = MethodBase.GetCurrentMethod();
            var command = new Command<TData>("1", "comando", new TData { Data = "aaweewswsws" });

            _mockManageEventsUseCase.Setup(useCase => useCase.ConsoleDebugLog(It.IsAny<string>()));

            _mockDirectAsyncGateway.Setup(directAsync => directAsync.SendCommand("cola", command)).Returns(Task.CompletedTask);

            _mockManageEventsUseCase.Setup(useCase => useCase.ConsoleDebugLog(It.IsAny<string>()));

            await _adapter.HandleSendCommandAsync<TData>(_mockDirectAsyncGateway.Object ,"1", new TData { Data="aaweewswsws"}, "cola", "comando", currentMethod!, "");

            _mockManageEventsUseCase.Verify(useCase => useCase.ConsoleDebugLog(It.IsAny<string>()), Times.Exactly(2));

            _mockDirectAsyncGateway.Verify(directAsync => directAsync.SendCommand(It.IsAny<string>(), It.IsAny<Command<TData>>()), Times.Once);

        }

        [Fact]
        public async Task HandleSendCommandAsyncTestException()
        {
            var currentMethod = MethodBase.GetCurrentMethod();

            _mockManageEventsUseCase.Setup(useCase => useCase.ConsoleDebugLog(It.IsAny<string>())).Throws(It.IsAny<SystemException>());

            _mockDirectAsyncGateway.Setup(directAsync => directAsync.SendCommand("", null)).ThrowsAsync(It.IsAny<SystemException>());

            _mockManageEventsUseCase.Setup(useCase => useCase.ConsoleErrorLog("Error al enviar el comando: [Id] : [1] - [Event] : [.AsyncGatewayAdapterBaseTest.]. Data: aaweewswsws", It.IsAny<SystemException>()));


            //await _adapter.HandleSendCommandAsync<TData>(_mockDirectAsyncGateway.Object, null, null, null, null, currentMethod!, null);
            await _adapter.HandleSendCommandAsync<TData>(_mockDirectAsyncGateway.Object, "1", new TData { Data = "aaweewswsws" }, "cola", "comando", currentMethod!, "");

            _mockManageEventsUseCase.Verify(useCase => useCase.ConsoleErrorLog(It.IsAny<string>(), It.IsAny<SystemException>()),Times.Once);
        }

        [Fact]
        public async Task HandleSendEventAsyncTestExitoso()
        {
            var currentMethod = MethodBase.GetCurrentMethod();
            var evento = new DomainEvent<TData>("1", "evento", new TData { Data = "aaweewswsws" });

            _mockManageEventsUseCase.Setup(useCase => useCase.ConsoleDebugLog(It.IsAny<string>()));

            _mockDirectAsyncGateway.Setup(directAsync => directAsync.SendEvent("evento", evento)).Returns(Task.CompletedTask);

            _mockManageEventsUseCase.Setup(useCase => useCase.ConsoleDebugLog(It.IsAny<string>()));

            await _adapter.HandleSendEventAsync<TData>(_mockDirectAsyncGateway.Object, "1", new TData { Data = "aaweewswsws" }, "topic", "evento", currentMethod!, "");

            _mockManageEventsUseCase.Verify(useCase => useCase.ConsoleDebugLog(It.IsAny<string>()), Times.Exactly(2));

            _mockDirectAsyncGateway.Verify(directAsync => directAsync.SendEvent(It.IsAny<string>(), It.IsAny<DomainEvent<TData>>()), Times.Once);

        }

        [Fact]
        public async Task HandleSendEventAsyncTestException()
        {
            var currentMethod = MethodBase.GetCurrentMethod();
            var data = new TData { Data = "aaweewswsws" };
            var evento = new DomainEvent<TData>("1", "evento", data);

            _mockManageEventsUseCase.Setup(useCase => useCase.ConsoleDebugLog(It.IsAny<string>())).Throws(It.IsAny<SystemException>());

            _mockDirectAsyncGateway.Setup(directAsync => directAsync.SendEvent(evento, "", null)).ThrowsAsync(It.IsAny<SystemException>());

            _mockManageEventsUseCase.Setup(useCase => useCase.ConsoleErrorLog("Error al enviar el evento: [Id] : [1] - [Event] : [.AsyncGatewayAdapterBaseTest.]. Data: aaweewswsws", It.IsAny<SystemException>()));


            //await _adapter.HandleSendCommandAsync<TData>(_mockDirectAsyncGateway.Object, null, null, null, null, currentMethod!, null);
            await _adapter.HandleSendEventAsync<TData>(_mockDirectAsyncGateway.Object, "1", data, "topic", "evento", currentMethod!, "");

            _mockManageEventsUseCase.Verify(useCase => useCase.ConsoleErrorLog(It.IsAny<string>(), It.IsAny<SystemException>()), Times.Once);
        }

        public ConfiguradorAppSettings ObtenerConfigParaTest()
        {
            var validationSettings = new ValidationSettings() { TipoContratoExpression = "", NombreExpression = "", DescripcionExpression = "", FechaExpression = "", RutaExpression = "" };
            var settingInstanciaRedis = new SettingInstanciaRedis() { Nombre = "", Instancia = "" };
            return new ConfiguradorAppSettings
            {
                AppId = "1",
                AppSecret = "",
                DomainName = "",
                MongoConnection = "",
                DefaultCountry = "",
                Database = "",
                StorageConnection = "",
                StorageContainerName = "",
                RedisCacheConnectionString = "",
                HealthChecksEndPoint = "",
                Validation = validationSettings,
                InstanciasRedis = settingInstanciaRedis,
                MongoDBConnection = "",
                RedisConnection = "",
                KeyJwt = "f6c5eb3a-3637-49ed-85fd-8ad11383d659",
                ServicesBusConnection = "",
                TopicTransacciones = ""
            };
        }

        public class TData{
            public string Data { get; set; }
        }

    }
}
