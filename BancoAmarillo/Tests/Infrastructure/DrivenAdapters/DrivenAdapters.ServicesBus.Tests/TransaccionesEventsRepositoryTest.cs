using Domain.Model.Entidades;
using Domain.Model.Interfaces;
using DrivenAdapters.ServiceBus.Base;
using DrivenAdapters.ServiceBus.Entities;
using Helpers.ObjectsUtils;
using Helpers.ObjectsUtils.ApplicationSettings;
using Microsoft.Extensions.Options;
using Moq;
using org.reactivecommons.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DrivenAdapters.ServiceBus.Tests
{
    public class TransaccionesEventsRepositoryTest
    {
        private readonly Mock<IDirectAsyncGateway<TransaccionEntityServiceBus>> _mockDirectAsyncGatewayTransaccion;
        private readonly Mock<IOptions<ConfiguradorAppSettings>> _mockConfiguradorAppSettings;
        private readonly Mock<IManageEventsUseCase> _mockManageEventsUseCase;
        private readonly Mock<IAsyncGatewayAdapterBase> _mockAsyncGatewayAdapaterBase;

        private readonly TransaccionesEventsRepository _transaccionesEventsRepository;

        public TransaccionesEventsRepositoryTest()
        {
            _mockDirectAsyncGatewayTransaccion = new();
            ConfiguradorAppSettings appSettings = ObtenerConfigParaTest();
            _mockConfiguradorAppSettings = new();
            _mockConfiguradorAppSettings.Setup(config => config.Value).Returns(appSettings);
            _mockAsyncGatewayAdapaterBase = new();
            _mockManageEventsUseCase = new();
            _transaccionesEventsRepository = new(_mockDirectAsyncGatewayTransaccion.Object, _mockManageEventsUseCase.Object, _mockConfiguradorAppSettings.Object);
        }

        [Fact]
        public async Task NotificarTransaccionRealizadaTest()
        {
            var transaccion = ObtenerTransaccionyParaServiceBus();
            var transaccionEntity = ObtenerTransaccionEntityParaServiceBus();
            _mockAsyncGatewayAdapaterBase.Setup(adapter => adapter.HandleSendEventAsync(It.IsAny<IDirectAsyncGateway<TransaccionEntityServiceBus>>(), "1", transaccionEntity, "topic", "evento", System.Reflection.MethodBase.GetCurrentMethod()!,"")).Returns(Task.CompletedTask);

            Assert.Equal(Task.CompletedTask, _transaccionesEventsRepository.NotificarTransaccionRealizada("jdpv@gmail.com", transaccion));
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
        public TransaccionEntityServiceBus ObtenerTransaccionEntityParaServiceBus()
        {
            return new TransaccionEntityServiceBus()
            {
                Id = "1",
                IdCuentaEmisora = "1",
                IdCuentaReceptora = "2",
                TipoTransaccion = Domain.Model.Entidades.Enums.TipoTransaccion.TRANSFERENCIA,
                Valor = 10,
                FechaMovimiento = DateTime.UtcNow.ToLocalTime(),
                TipoMovimiento = Domain.Model.Entidades.Enums.TipoMovimiento.CREDITO,
                Correo = "jdpv@gmail.com"
            };
        }

        public Transaccion ObtenerTransaccionyParaServiceBus()
        {
            return new Transaccion()
            {
                Id = "1",
                IdCuentaEmisora = "1",
                IdCuentaReceptora = "2",
                TipoTransaccion = Domain.Model.Entidades.Enums.TipoTransaccion.TRANSFERENCIA,
                Valor = 10,
                FechaMovimiento = DateTime.UtcNow.ToLocalTime(),
                TipoMovimiento = Domain.Model.Entidades.Enums.TipoMovimiento.CREDITO
            };
        }
    }
}
