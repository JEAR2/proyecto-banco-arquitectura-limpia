using AutoMapper;
using Domain.Model.Entidades;
using Domain.Model.Gateway;
using Domain.Model.Interfaces;
using DrivenAdapters.ServiceBus.Base;
using DrivenAdapters.ServiceBus.Entities;
using Helpers.ObjectsUtils;
using Microsoft.Extensions.Options;
using org.reactivecommons.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DrivenAdapters.ServiceBus
{
    public class TransaccionesEventsRepository : AsyncGatewayAdapterBase, ITransaccionesEventsRepository
    {
        private readonly IDirectAsyncGateway<TransaccionEntityServiceBus> _directAsyncGatewayTransaccion;
        private readonly IOptions<ConfiguradorAppSettings> _appSettings;
        public TransaccionesEventsRepository(IDirectAsyncGateway<TransaccionEntityServiceBus> directAsyncGatewayTransaccion,
            IManageEventsUseCase manageEventsUseCase, IOptions<ConfiguradorAppSettings> appSettings) : base(manageEventsUseCase, appSettings)
        {
            _directAsyncGatewayTransaccion = directAsyncGatewayTransaccion;
            _appSettings = appSettings;
        }

        /// <summary>
        /// <see cref="ITransaccionesEventsRepository.NotificarTransaccionRealizada(Cliente, Transaccion)"/>
        /// </summary>
        /// <param name="cliente"></param>
        /// <param name="transaccion"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task NotificarTransaccionRealizada(string correo, Transaccion transaccion)
        {
            string eventName = "Transaccion.Realizada";

            var transaccionServiceBus = MappingTransaccion(correo, transaccion);
            
            await HandleSendEventAsync(_directAsyncGatewayTransaccion, transaccionServiceBus.Id, transaccionServiceBus
                , _appSettings.Value.TopicTransacciones, eventName, MethodBase.GetCurrentMethod()!);

        }

        public static TransaccionEntityServiceBus MappingTransaccion(string correo, Transaccion transaccion)
        {
            return new TransaccionEntityServiceBus() {
                Id = transaccion.Id,
                IdCuentaEmisora = transaccion.IdCuentaEmisora,
                IdCuentaReceptora = transaccion.IdCuentaReceptora,
                TipoTransaccion = transaccion.TipoTransaccion,
                Valor = transaccion.Valor,
                FechaMovimiento = transaccion.FechaMovimiento,
                TipoMovimiento = transaccion.TipoMovimiento,
                Correo = correo,
            };
        }


    }
}
