// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: protos/Cliente.proto
// </auto-generated>
#pragma warning disable 0414, 1591, 8981
#region Designer generated code

using grpc = global::Grpc.Core;

namespace EntryPoints.Grpc.Dtos.Protos.Cliente {
  public static partial class ClienteService
  {
    static readonly string __ServiceName = "ClienteService";

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static void __Helper_SerializeMessage(global::Google.Protobuf.IMessage message, grpc::SerializationContext context)
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (message is global::Google.Protobuf.IBufferMessage)
      {
        context.SetPayloadLength(message.CalculateSize());
        global::Google.Protobuf.MessageExtensions.WriteTo(message, context.GetBufferWriter());
        context.Complete();
        return;
      }
      #endif
      context.Complete(global::Google.Protobuf.MessageExtensions.ToByteArray(message));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static class __Helper_MessageCache<T>
    {
      public static readonly bool IsBufferMessage = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::Google.Protobuf.IBufferMessage)).IsAssignableFrom(typeof(T));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static T __Helper_DeserializeMessage<T>(grpc::DeserializationContext context, global::Google.Protobuf.MessageParser<T> parser) where T : global::Google.Protobuf.IMessage<T>
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (__Helper_MessageCache<T>.IsBufferMessage)
      {
        return parser.ParseFrom(context.PayloadAsReadOnlySequence());
      }
      #endif
      return parser.ParseFrom(context.PayloadAsNewBuffer());
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::EntryPoints.Grpc.Dtos.Protos.Cliente.ClienteRequest> __Marshaller_ClienteRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::EntryPoints.Grpc.Dtos.Protos.Cliente.ClienteRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente> __Marshaller_ResponseCliente = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::EntryPoints.Grpc.Dtos.Protos.Cliente.IdCliente> __Marshaller_IdCliente = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::EntryPoints.Grpc.Dtos.Protos.Cliente.IdCliente.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::EntryPoints.Grpc.Dtos.Protos.Cliente.Empty> __Marshaller_Empty = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::EntryPoints.Grpc.Dtos.Protos.Cliente.Empty.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::EntryPoints.Grpc.Dtos.Protos.Cliente.ListaClientes> __Marshaller_ListaClientes = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::EntryPoints.Grpc.Dtos.Protos.Cliente.ListaClientes.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::EntryPoints.Grpc.Dtos.Protos.Cliente.ActualizarClienteProto> __Marshaller_ActualizarClienteProto = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::EntryPoints.Grpc.Dtos.Protos.Cliente.ActualizarClienteProto.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::EntryPoints.Grpc.Dtos.Protos.Cliente.ListaCuentasCliente> __Marshaller_ListaCuentasCliente = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::EntryPoints.Grpc.Dtos.Protos.Cliente.ListaCuentasCliente.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::EntryPoints.Grpc.Dtos.Protos.Cliente.DatosMarcarCuentaGMF> __Marshaller_DatosMarcarCuentaGMF = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::EntryPoints.Grpc.Dtos.Protos.Cliente.DatosMarcarCuentaGMF.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::EntryPoints.Grpc.Dtos.Protos.Cliente.ClienteRequest, global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente> __Method_CrearCliente = new grpc::Method<global::EntryPoints.Grpc.Dtos.Protos.Cliente.ClienteRequest, global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente>(
        grpc::MethodType.Unary,
        __ServiceName,
        "CrearCliente",
        __Marshaller_ClienteRequest,
        __Marshaller_ResponseCliente);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::EntryPoints.Grpc.Dtos.Protos.Cliente.IdCliente, global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente> __Method_ObtenerCliente = new grpc::Method<global::EntryPoints.Grpc.Dtos.Protos.Cliente.IdCliente, global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente>(
        grpc::MethodType.Unary,
        __ServiceName,
        "ObtenerCliente",
        __Marshaller_IdCliente,
        __Marshaller_ResponseCliente);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::EntryPoints.Grpc.Dtos.Protos.Cliente.Empty, global::EntryPoints.Grpc.Dtos.Protos.Cliente.ListaClientes> __Method_ObtenerClientes = new grpc::Method<global::EntryPoints.Grpc.Dtos.Protos.Cliente.Empty, global::EntryPoints.Grpc.Dtos.Protos.Cliente.ListaClientes>(
        grpc::MethodType.Unary,
        __ServiceName,
        "ObtenerClientes",
        __Marshaller_Empty,
        __Marshaller_ListaClientes);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::EntryPoints.Grpc.Dtos.Protos.Cliente.IdCliente, global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente> __Method_EliminarCliente = new grpc::Method<global::EntryPoints.Grpc.Dtos.Protos.Cliente.IdCliente, global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente>(
        grpc::MethodType.Unary,
        __ServiceName,
        "EliminarCliente",
        __Marshaller_IdCliente,
        __Marshaller_ResponseCliente);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::EntryPoints.Grpc.Dtos.Protos.Cliente.ActualizarClienteProto, global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente> __Method_ActualizarCliente = new grpc::Method<global::EntryPoints.Grpc.Dtos.Protos.Cliente.ActualizarClienteProto, global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente>(
        grpc::MethodType.Unary,
        __ServiceName,
        "ActualizarCliente",
        __Marshaller_ActualizarClienteProto,
        __Marshaller_ResponseCliente);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::EntryPoints.Grpc.Dtos.Protos.Cliente.IdCliente, global::EntryPoints.Grpc.Dtos.Protos.Cliente.ListaCuentasCliente> __Method_ObtenerCuentasCliente = new grpc::Method<global::EntryPoints.Grpc.Dtos.Protos.Cliente.IdCliente, global::EntryPoints.Grpc.Dtos.Protos.Cliente.ListaCuentasCliente>(
        grpc::MethodType.Unary,
        __ServiceName,
        "ObtenerCuentasCliente",
        __Marshaller_IdCliente,
        __Marshaller_ListaCuentasCliente);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::EntryPoints.Grpc.Dtos.Protos.Cliente.DatosMarcarCuentaGMF, global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente> __Method_MarcarCuentaGMF = new grpc::Method<global::EntryPoints.Grpc.Dtos.Protos.Cliente.DatosMarcarCuentaGMF, global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente>(
        grpc::MethodType.Unary,
        __ServiceName,
        "MarcarCuentaGMF",
        __Marshaller_DatosMarcarCuentaGMF,
        __Marshaller_ResponseCliente);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::EntryPoints.Grpc.Dtos.Protos.Cliente.ClienteReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of ClienteService</summary>
    [grpc::BindServiceMethod(typeof(ClienteService), "BindService")]
    public abstract partial class ClienteServiceBase
    {
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente> CrearCliente(global::EntryPoints.Grpc.Dtos.Protos.Cliente.ClienteRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente> ObtenerCliente(global::EntryPoints.Grpc.Dtos.Protos.Cliente.IdCliente request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::EntryPoints.Grpc.Dtos.Protos.Cliente.ListaClientes> ObtenerClientes(global::EntryPoints.Grpc.Dtos.Protos.Cliente.Empty request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente> EliminarCliente(global::EntryPoints.Grpc.Dtos.Protos.Cliente.IdCliente request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente> ActualizarCliente(global::EntryPoints.Grpc.Dtos.Protos.Cliente.ActualizarClienteProto request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::EntryPoints.Grpc.Dtos.Protos.Cliente.ListaCuentasCliente> ObtenerCuentasCliente(global::EntryPoints.Grpc.Dtos.Protos.Cliente.IdCliente request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente> MarcarCuentaGMF(global::EntryPoints.Grpc.Dtos.Protos.Cliente.DatosMarcarCuentaGMF request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for ClienteService</summary>
    public partial class ClienteServiceClient : grpc::ClientBase<ClienteServiceClient>
    {
      /// <summary>Creates a new client for ClienteService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public ClienteServiceClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for ClienteService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public ClienteServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected ClienteServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected ClienteServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente CrearCliente(global::EntryPoints.Grpc.Dtos.Protos.Cliente.ClienteRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return CrearCliente(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente CrearCliente(global::EntryPoints.Grpc.Dtos.Protos.Cliente.ClienteRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_CrearCliente, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente> CrearClienteAsync(global::EntryPoints.Grpc.Dtos.Protos.Cliente.ClienteRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return CrearClienteAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente> CrearClienteAsync(global::EntryPoints.Grpc.Dtos.Protos.Cliente.ClienteRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_CrearCliente, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente ObtenerCliente(global::EntryPoints.Grpc.Dtos.Protos.Cliente.IdCliente request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ObtenerCliente(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente ObtenerCliente(global::EntryPoints.Grpc.Dtos.Protos.Cliente.IdCliente request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_ObtenerCliente, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente> ObtenerClienteAsync(global::EntryPoints.Grpc.Dtos.Protos.Cliente.IdCliente request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ObtenerClienteAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente> ObtenerClienteAsync(global::EntryPoints.Grpc.Dtos.Protos.Cliente.IdCliente request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_ObtenerCliente, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::EntryPoints.Grpc.Dtos.Protos.Cliente.ListaClientes ObtenerClientes(global::EntryPoints.Grpc.Dtos.Protos.Cliente.Empty request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ObtenerClientes(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::EntryPoints.Grpc.Dtos.Protos.Cliente.ListaClientes ObtenerClientes(global::EntryPoints.Grpc.Dtos.Protos.Cliente.Empty request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_ObtenerClientes, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::EntryPoints.Grpc.Dtos.Protos.Cliente.ListaClientes> ObtenerClientesAsync(global::EntryPoints.Grpc.Dtos.Protos.Cliente.Empty request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ObtenerClientesAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::EntryPoints.Grpc.Dtos.Protos.Cliente.ListaClientes> ObtenerClientesAsync(global::EntryPoints.Grpc.Dtos.Protos.Cliente.Empty request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_ObtenerClientes, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente EliminarCliente(global::EntryPoints.Grpc.Dtos.Protos.Cliente.IdCliente request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return EliminarCliente(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente EliminarCliente(global::EntryPoints.Grpc.Dtos.Protos.Cliente.IdCliente request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_EliminarCliente, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente> EliminarClienteAsync(global::EntryPoints.Grpc.Dtos.Protos.Cliente.IdCliente request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return EliminarClienteAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente> EliminarClienteAsync(global::EntryPoints.Grpc.Dtos.Protos.Cliente.IdCliente request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_EliminarCliente, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente ActualizarCliente(global::EntryPoints.Grpc.Dtos.Protos.Cliente.ActualizarClienteProto request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ActualizarCliente(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente ActualizarCliente(global::EntryPoints.Grpc.Dtos.Protos.Cliente.ActualizarClienteProto request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_ActualizarCliente, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente> ActualizarClienteAsync(global::EntryPoints.Grpc.Dtos.Protos.Cliente.ActualizarClienteProto request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ActualizarClienteAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente> ActualizarClienteAsync(global::EntryPoints.Grpc.Dtos.Protos.Cliente.ActualizarClienteProto request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_ActualizarCliente, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::EntryPoints.Grpc.Dtos.Protos.Cliente.ListaCuentasCliente ObtenerCuentasCliente(global::EntryPoints.Grpc.Dtos.Protos.Cliente.IdCliente request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ObtenerCuentasCliente(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::EntryPoints.Grpc.Dtos.Protos.Cliente.ListaCuentasCliente ObtenerCuentasCliente(global::EntryPoints.Grpc.Dtos.Protos.Cliente.IdCliente request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_ObtenerCuentasCliente, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::EntryPoints.Grpc.Dtos.Protos.Cliente.ListaCuentasCliente> ObtenerCuentasClienteAsync(global::EntryPoints.Grpc.Dtos.Protos.Cliente.IdCliente request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ObtenerCuentasClienteAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::EntryPoints.Grpc.Dtos.Protos.Cliente.ListaCuentasCliente> ObtenerCuentasClienteAsync(global::EntryPoints.Grpc.Dtos.Protos.Cliente.IdCliente request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_ObtenerCuentasCliente, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente MarcarCuentaGMF(global::EntryPoints.Grpc.Dtos.Protos.Cliente.DatosMarcarCuentaGMF request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return MarcarCuentaGMF(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente MarcarCuentaGMF(global::EntryPoints.Grpc.Dtos.Protos.Cliente.DatosMarcarCuentaGMF request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_MarcarCuentaGMF, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente> MarcarCuentaGMFAsync(global::EntryPoints.Grpc.Dtos.Protos.Cliente.DatosMarcarCuentaGMF request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return MarcarCuentaGMFAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente> MarcarCuentaGMFAsync(global::EntryPoints.Grpc.Dtos.Protos.Cliente.DatosMarcarCuentaGMF request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_MarcarCuentaGMF, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected override ClienteServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new ClienteServiceClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static grpc::ServerServiceDefinition BindService(ClienteServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_CrearCliente, serviceImpl.CrearCliente)
          .AddMethod(__Method_ObtenerCliente, serviceImpl.ObtenerCliente)
          .AddMethod(__Method_ObtenerClientes, serviceImpl.ObtenerClientes)
          .AddMethod(__Method_EliminarCliente, serviceImpl.EliminarCliente)
          .AddMethod(__Method_ActualizarCliente, serviceImpl.ActualizarCliente)
          .AddMethod(__Method_ObtenerCuentasCliente, serviceImpl.ObtenerCuentasCliente)
          .AddMethod(__Method_MarcarCuentaGMF, serviceImpl.MarcarCuentaGMF).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static void BindService(grpc::ServiceBinderBase serviceBinder, ClienteServiceBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_CrearCliente, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::EntryPoints.Grpc.Dtos.Protos.Cliente.ClienteRequest, global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente>(serviceImpl.CrearCliente));
      serviceBinder.AddMethod(__Method_ObtenerCliente, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::EntryPoints.Grpc.Dtos.Protos.Cliente.IdCliente, global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente>(serviceImpl.ObtenerCliente));
      serviceBinder.AddMethod(__Method_ObtenerClientes, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::EntryPoints.Grpc.Dtos.Protos.Cliente.Empty, global::EntryPoints.Grpc.Dtos.Protos.Cliente.ListaClientes>(serviceImpl.ObtenerClientes));
      serviceBinder.AddMethod(__Method_EliminarCliente, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::EntryPoints.Grpc.Dtos.Protos.Cliente.IdCliente, global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente>(serviceImpl.EliminarCliente));
      serviceBinder.AddMethod(__Method_ActualizarCliente, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::EntryPoints.Grpc.Dtos.Protos.Cliente.ActualizarClienteProto, global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente>(serviceImpl.ActualizarCliente));
      serviceBinder.AddMethod(__Method_ObtenerCuentasCliente, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::EntryPoints.Grpc.Dtos.Protos.Cliente.IdCliente, global::EntryPoints.Grpc.Dtos.Protos.Cliente.ListaCuentasCliente>(serviceImpl.ObtenerCuentasCliente));
      serviceBinder.AddMethod(__Method_MarcarCuentaGMF, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::EntryPoints.Grpc.Dtos.Protos.Cliente.DatosMarcarCuentaGMF, global::EntryPoints.Grpc.Dtos.Protos.Cliente.ResponseCliente>(serviceImpl.MarcarCuentaGMF));
    }

  }
}
#endregion