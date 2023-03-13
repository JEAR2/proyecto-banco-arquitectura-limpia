using Domain.Model.Entidades;
using Domain.Model.Entidades.Enums;
using Domain.Model.Gateway;
using Domain.UseCase.Auth;
using Helpers.ObjectsUtils;
using Helpers.ObjectsUtils.ApplicationSettings;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Domain.UseCase.Tests.Auth
{
    public class AuthUseCaseTest
    {
        private readonly Mock<IOptions<ConfiguradorAppSettings>> _mockOptions;
        private readonly Mock<IAuthRepository> _mockAuthAdapter;
        private readonly AuthUseCase _authUseCase;


        public AuthUseCaseTest()
        {
            ConfiguradorAppSettings appSettings = ObtenerConfigParaTest();
            _mockOptions = new();
            _mockOptions.Setup(config => config.Value).Returns(appSettings);
            _mockAuthAdapter = new();
            _authUseCase = new(_mockOptions.Object, _mockAuthAdapter.Object);
        }

        [Fact]
        public async Task CrearUsuarioUseCaseTest()
        {
            var usuario = ObtenerUsuarioParaTest();

            _mockAuthAdapter.Setup(adaptar => adaptar.RegistrarUsuario(usuario)).ReturnsAsync(usuario);

            var respones = await _authUseCase.CrearUsuario(usuario);

            Assert.NotNull(respones);
            Assert.IsType<AccesToken>(respones);
        }


        [Fact]
        public async Task IniciarSesionUseCaseTest()
        {
            var usuario = ObtenerUsuarioParaTest();

            _mockAuthAdapter.Setup(adaptar => adaptar.IniciarSesionAsync(usuario)).ReturnsAsync(usuario);

            var respones = await _authUseCase.IniciarSesion(usuario);

            Assert.NotNull(respones);
            Assert.IsType<AccesToken>(respones);
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

        public Usuario ObtenerUsuarioParaTest()
        {
            return new Usuario
            {
                Id = "12",
                Correo = "qwsqw@gmail.com",
                Clave = "sambaaa2.Pa",
                Rol = Rol.ADMIN
            };
        }

    }
}
