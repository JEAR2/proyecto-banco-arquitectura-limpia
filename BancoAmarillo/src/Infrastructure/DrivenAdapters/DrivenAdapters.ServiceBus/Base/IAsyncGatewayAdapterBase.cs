using org.reactivecommons.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DrivenAdapters.ServiceBus.Base
{
    public interface IAsyncGatewayAdapterBase
    {
        Task HandleSendCommandAsync<TData>(IDirectAsyncGateway<TData> directAsyncGateway, string id, TData data, string queue, string commandName,
            MethodBase methodBase, [CallerMemberName] string? callerMemberName = null);

        Task HandleSendEventAsync<TData>(IDirectAsyncGateway<TData> directAsyncGateway, string id, TData data, string topic, string eventName,
            MethodBase methodBase, [CallerMemberName] string callerMemberName = null);
    }
}
