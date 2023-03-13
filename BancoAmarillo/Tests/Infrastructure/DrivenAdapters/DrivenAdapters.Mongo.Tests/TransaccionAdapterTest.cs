using AutoMapper;
using BancoAmarillo.AppServices.Automapper;
using Domain.Model.Entidades;
using Domain.Model.Entidades.Enums;
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
using System.Transactions;
using Xunit;

namespace DrivenAdapters.Mongo.Tests
{
    public class TransaccionAdapterTest
    {
        private readonly Mock<IContext> _mockContext;
        private readonly Mock<IMongoCollection<TransaccionEntity>> _mockCollectionTransaccion;
        private readonly Mock<IAsyncCursor<TransaccionEntity>> _transaccionCursor;
        private readonly IMapper _mapper;
        private readonly IConfigurationProvider _configurationProvider;
        public TransaccionAdapterTest()
        {
            _mockContext = new();
            _mockCollectionTransaccion = new();
            _transaccionCursor = new();
            _configurationProvider = new MapperConfiguration(options => options.AddProfile<ConfigurationProfile>());
            _mapper = _configurationProvider.CreateMapper();

            _mockCollectionTransaccion.Object.InsertMany(ObtenerTransaccionesParaTest());

            _transaccionCursor.SetupSequence(item => item.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true).Returns(false);

            _transaccionCursor.SetupSequence(item => item.MoveNextAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(true)).Returns(Task.FromResult(false));
        }

        

        [Fact]
        public async Task CrearTransaccionTest()
        {
            _mockCollectionTransaccion.Setup(collection => collection.InsertOneAsync(It.IsAny<TransaccionEntity>(),
               It.IsAny<InsertOneOptions>(),
               It.IsAny<CancellationToken>()));

            var transaccion = new Transaccion()
            {
                Id = "1",
                IdCuentaEmisora = "1",
                IdCuentaReceptora = "2",
                TipoTransaccion = TipoTransaccion.TRANSFERENCIA,
                Valor = 10,
                FechaMovimiento = DateTime.UtcNow.ToLocalTime(),
                TipoMovimiento = TipoMovimiento.CREDITO
            };

            _mockContext.Setup(context => context.Transacciones).Returns(_mockCollectionTransaccion.Object);

            

            var transaccionAdapter = new TransaccionAdapter(_mockContext.Object, _mapper);
            var response = await transaccionAdapter.CrearTransaccionAsync(transaccion);

            _mockCollectionTransaccion.Verify(
            x => x.InsertOneAsync(It.IsAny<TransaccionEntity>(), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.IsType<Transaccion>(response);
        }

        [Theory]
        [InlineData("1")]
        public async Task ObtenerTransaccionPorId(string id)
        {
            _transaccionCursor.Setup(item => item.Current).Returns(ObtenerTransaccionesParaTest());

            _mockCollectionTransaccion.Setup(collection => collection.FindAsync(It.IsAny<FilterDefinition<TransaccionEntity>>(),
                It.IsAny<FindOptions<TransaccionEntity, TransaccionEntity>>(), It.IsAny<CancellationToken>())).ReturnsAsync(_transaccionCursor.Object);

            _mockContext.Setup(context => context.Transacciones).Returns(_mockCollectionTransaccion.Object);


            var transaccionAdapter = new TransaccionAdapter(_mockContext.Object, _mapper);
            var response = await transaccionAdapter.ObtenerTransaccionPorIdAsync(id);

            _mockCollectionTransaccion.Verify(
            x => x.FindAsync(It.IsAny<FilterDefinition<TransaccionEntity>>(),
                It.IsAny<FindOptions<TransaccionEntity, TransaccionEntity>>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.IsType<Transaccion>(response);

        }




        private List<TransaccionEntity> ObtenerTransaccionesParaTest()
        {
            List<TransaccionEntity> list = new List<TransaccionEntity>();
            var transaccion = new TransaccionEntity();
            transaccion.Id = "1";
            transaccion.IdCuentaEmisora = "1";
            transaccion.IdCuentaReceptora = "2";
            transaccion.TipoTransaccion = TipoTransaccionEntity.TRANSFERENCIA;
            transaccion.Valor = 10;
            transaccion.FechaMovimiento = DateTime.UtcNow.ToLocalTime();
            transaccion.TipoMovimiento = TipoMovimientoEntity.CREDITO;

            var transaccion2 = new TransaccionEntity();
            transaccion2.Id = "2";
            transaccion2.IdCuentaEmisora = "1";
            transaccion2.IdCuentaReceptora = "2";
            transaccion2.TipoTransaccion = TipoTransaccionEntity.TRANSFERENCIA;
            transaccion2.Valor = -10;
            transaccion2.FechaMovimiento = DateTime.UtcNow.ToLocalTime();
            transaccion2.TipoMovimiento = TipoMovimientoEntity.DEBITO;
            list.Add(transaccion);
            list.Add(transaccion2);
            return list;
        }

       
    }



}
