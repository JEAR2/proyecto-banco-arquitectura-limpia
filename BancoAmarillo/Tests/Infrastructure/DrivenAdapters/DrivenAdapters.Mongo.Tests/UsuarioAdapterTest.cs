using AutoMapper;
using BancoAmarillo.AppServices.Automapper;
using BCrypt.Net;
using credinet.exception.middleware.models;
using Domain.Model.Entidades;
using DrivenAdapters.Mongo.Adaptadores;
using DrivenAdapters.Mongo.Entities;
using DrivenAdapters.Mongo.Entities.Enums;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DrivenAdapters.Mongo.Tests
{
    public class UsuarioAdapterTest
    {
        private readonly Mock<IContext> _mockContext;
        private readonly Mock<IMongoCollection<UsuarioEntity>> _mockCollectionUsuario;
        private readonly Mock<IAsyncCursor<UsuarioEntity>> _usuarioCursor;
        private readonly IMapper _mapper;
        private readonly IConfigurationProvider _configurationProvider;
        private readonly UsuarioAdapter _adapter;

        public UsuarioAdapterTest()
        {
            _mockContext = new();
            _mockCollectionUsuario = new();
            _usuarioCursor = new();
            _configurationProvider = new MapperConfiguration(options => options.AddProfile<ConfigurationProfile>());
            _mapper = _configurationProvider.CreateMapper();

            _mockCollectionUsuario.Object.InsertMany(ObtenerUsuariosEntityParaTest());

            _usuarioCursor.SetupSequence(item => item.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true).Returns(false);

            _usuarioCursor.SetupSequence(item => item.MoveNextAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(true)).Returns(Task.FromResult(false));

            _adapter = new(_mockContext.Object, _mapper);
        }


        [Fact]
        public async Task IncioSesionExitoso()
        {
            var usuario = ObtenerUsuarioParaTest();
            _usuarioCursor.Setup(item => item.Current).Returns(ObtenerUsuariosEntityParaTest());

            _mockCollectionUsuario.Setup(collection => collection.FindAsync(It.IsAny<FilterDefinition<UsuarioEntity>>(),
                It.IsAny<FindOptions<UsuarioEntity, UsuarioEntity>>(), It.IsAny<CancellationToken>())).ReturnsAsync(_usuarioCursor.Object);

            _mockContext.Setup(context => context.Usuarios).Returns(_mockCollectionUsuario.Object);

            var response = await _adapter.IniciarSesionAsync(usuario);

            Assert.NotNull(response);
            Assert.IsType<Usuario>(response);

        }

        [Fact]
        public async Task IncioSesionUsuarioNull()
        {
            var usuario = ObtenerUsuarioParaTest();

            _mockCollectionUsuario.Setup(collection => collection.FindAsync(It.IsAny<FilterDefinition<UsuarioEntity>>(),
                It.IsAny<FindOptions<UsuarioEntity, UsuarioEntity>>(), It.IsAny<CancellationToken>())).ReturnsAsync(_usuarioCursor.Object);

            _mockContext.Setup(context => context.Usuarios).Returns(_mockCollectionUsuario.Object);


            BusinessException businessException = await Assert.ThrowsAsync<BusinessException>(async ()
                => await _adapter.IniciarSesionAsync(usuario));

        }

        [Fact]
        public async Task IncioSesionUsuarioContrasenaIncorrecto()
        {
            var usuario = new Usuario() { Id = "3", Correo = "dewde@gmail.com", Clave = "122344566.sD", Rol = Domain.Model.Entidades.Enums.Rol.USER };

            _usuarioCursor.Setup(item => item.Current).Returns(ObtenerUsuariosEntityParaTest());

            _mockCollectionUsuario.Setup(collection => collection.FindAsync(It.IsAny<FilterDefinition<UsuarioEntity>>(),
                It.IsAny<FindOptions<UsuarioEntity, UsuarioEntity>>(), It.IsAny<CancellationToken>())).ReturnsAsync(_usuarioCursor.Object);

            _mockContext.Setup(context => context.Usuarios).Returns(_mockCollectionUsuario.Object);


            BusinessException businessException = await Assert.ThrowsAsync<BusinessException>(async ()
                => await _adapter.IniciarSesionAsync(usuario));

        }

        [Fact]
        public async Task RegistrarUsuarioExitoso()
        {
            var usuario = new Usuario() { Id = "3", Correo = "dewde@gmail.com", Clave = "123456.JP", Rol = Domain.Model.Entidades.Enums.Rol.USER };

            _mockCollectionUsuario.Setup(collection => collection.FindAsync(It.IsAny<FilterDefinition<UsuarioEntity>>(),
               It.IsAny<FindOptions<UsuarioEntity, UsuarioEntity>>(), It.IsAny<CancellationToken>())).ReturnsAsync(_usuarioCursor.Object);

            _mockCollectionUsuario.Setup(collection => collection.InsertOneAsync(It.IsAny<UsuarioEntity>(),
               It.IsAny<InsertOneOptions>(),
               It.IsAny<CancellationToken>()));

            _mockContext.Setup(context => context.Usuarios).Returns(_mockCollectionUsuario.Object);

            var response = await _adapter.RegistrarUsuario(usuario);

            Assert.NotNull(response);
            Assert.IsType<Usuario>(response);

        }

        [Fact]
        public async Task RegistrarUsuarioExcepcionYaRegistrado()
        {
            var usuario = new Usuario() { Id = "3", Correo = "dewde@gmail.com", Clave = "123456.JP", Rol = Domain.Model.Entidades.Enums.Rol.USER };

            _usuarioCursor.Setup(item => item.Current).Returns(ObtenerUsuariosEntityParaTest());

            _mockCollectionUsuario.Setup(collection => collection.FindAsync(It.IsAny<FilterDefinition<UsuarioEntity>>(),
               It.IsAny<FindOptions<UsuarioEntity, UsuarioEntity>>(), It.IsAny<CancellationToken>())).ReturnsAsync(_usuarioCursor.Object);

            _mockCollectionUsuario.Setup(collection => collection.InsertOneAsync(It.IsAny<UsuarioEntity>(),
               It.IsAny<InsertOneOptions>(),
               It.IsAny<CancellationToken>()));

            _mockContext.Setup(context => context.Usuarios).Returns(_mockCollectionUsuario.Object);

            BusinessException businessException = await Assert.ThrowsAsync<BusinessException>(async ()
                => await _adapter.RegistrarUsuario(usuario));

        }

        public IEnumerable<UsuarioEntity> ObtenerUsuariosEntityParaTest()
        {
            var hashpswd = BCrypt.Net.BCrypt.HashPassword("samba2.Pa");
            return new List<UsuarioEntity>()
            {
                new UsuarioEntity
                {
                    Id="1",
                    Correo="jdpv@gmail.com",
                    Clave=hashpswd,
                    Rol=Rol.ADMIN
                },
             
            };
        }
        public Usuario ObtenerUsuarioParaTest()
        {
            return new Usuario
            {
                Id = "1",
                Correo = "jdpv@gmail.com",
                Clave = "samba2.Pa",
                Rol = Domain.Model.Entidades.Enums.Rol.ADMIN
            };
        }
    }
}
