using AutoMapper;
using credinet.exception.middleware.models;
using Domain.Model.Entidades;
using Domain.Model.Entidades.Enums;
using Domain.UseCase.Clientes;
using Domain.UseCase.Cuentas;
using EntryPoints.Grpc.Dtos.Protos.Cuentas;
using EntryPoints.Grpc.Dtos.Protos.Cliente;
using EntryPoints.Grpc.Validaciones;
using FluentValidation;
using Grpc.Core;
using Helpers.Commons.Exceptions;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Helpers.ObjectsUtils.Extensions;

namespace EntryPoints.Grpc.Controller
{
    [Authorize]
    public class ClienteController : ClienteService.ClienteServiceBase
    {
        private readonly IClienteUseCase _clienteUseCase;
        private readonly ICuentasUseCase _cuentaUseCase;
        private readonly IMapper _mapper;
        private readonly IValidator<ClienteRequest> _validator;

        public ClienteController(IClienteUseCase clienteUseCase, IMapper mapper, IValidator<ClienteRequest> validator, ICuentasUseCase cuentaUseCase = null)
        {
            _clienteUseCase = clienteUseCase;
            _mapper = mapper;
            _validator = validator;
            _cuentaUseCase = cuentaUseCase;
        }

        [Authorize(Roles = "ADMIN")]
        public override async Task<ResponseCliente> CrearCliente(ClienteRequest request, ServerCallContext context)
        => await HandlerRequestAsync(async () =>
            {
                await _validator.ValidateAsync(request)!.ModeloValido();
                var cliente = _mapper.Map<Cliente>(request);
                cliente.UsuarioModificacion = ObtenerUsuarioRequest(context);
                var cl = await _clienteUseCase.CrearCliente(cliente);
                return cl;
            }, "Cliente creado");

        public override async Task<ResponseCliente> ObtenerCliente(IdCliente request, ServerCallContext context)
        {
            return await HandlerRequestAsync(async () =>
            {
                return await _clienteUseCase.ObtenerClientePorId(request.Id);
            }, "Cliente");
        }

        public override async Task<ListaClientes> ObtenerClientes(Empty request, ServerCallContext context)
        {
            var respuesta = new ListaClientes();
            var result = await _clienteUseCase.ObtenerClientes();
            var clientesrespuesta = _mapper.Map<List<ClienteResponse>>(result);
            respuesta.Clientes.Add(clientesrespuesta);
            return respuesta;
        }

        [Authorize(Roles = "ADMIN")]
        public override async Task<ResponseCliente> EliminarCliente(IdCliente request, ServerCallContext context)
        {
            return await HandlerRequestAsync(async () =>
            {
                return await _clienteUseCase.EliminarCliente(request.Id);
            }, "Cliente eliminado");
        }

        [Authorize(Roles = "ADMIN")]
        public override async Task<ResponseCliente> ActualizarCliente(ActualizarClienteProto request, ServerCallContext context)
        {
            return await HandlerRequestAsync(async () =>
            {
                var cliente = _mapper.Map<Cliente>(request);
                cliente.UsuarioModificacion = ObtenerUsuarioRequest(context);
                return await _clienteUseCase.ActualizarCliente(request.ClienteId, cliente);
            }, "Cliente actualizado");
        }

        public override async Task<ListaCuentasCliente> ObtenerCuentasCliente(IdCliente request, ServerCallContext context)
        {
            var cliente = await _clienteUseCase.ObtenerClientePorId(request.Id);
            var respuesta = new ListaCuentasCliente();
            var cuentasActivas = cliente.Cuentas.FindAll(cuenta => cuenta.EstadoCuenta.Equals(Domain.Model.Entidades.Enums.EstadoCuenta.ACTIVA)).OrderByDescending(x => x.Saldo);
            var cuentasInactivas = cliente.Cuentas.FindAll(cuenta => cuenta.EstadoCuenta.Equals(Domain.Model.Entidades.Enums.EstadoCuenta.INACTIVA)).OrderByDescending(x => x.Saldo);
            var cuentasCanceladas = cliente.Cuentas.FindAll(cuenta => cuenta.EstadoCuenta.Equals(Domain.Model.Entidades.Enums.EstadoCuenta.CANCELADA));

            respuesta.Cuentas.Add(_mapper.Map<List<CuentasClienteResponse>>(cuentasActivas.Concat(cuentasInactivas).Concat(cuentasCanceladas)));

            return respuesta;
        }

        public override async Task<ResponseCliente> MarcarCuentaGMF(DatosMarcarCuentaGMF request, ServerCallContext context)
        => await HandlerRequestAsync(async () =>
            {
                var cliente = await _clienteUseCase.ObtenerClientePorId(request.IdCliente);
                var cuenta = cliente.Cuentas.FirstOrDefault(c => c.Id == request.IdCuenta);
                if (cuenta == null)
                    throw new BusinessException(TipoExcepcionNegocio.ExceptionCuentaNoExiste.GetDescription()
                       , (int)TipoExcepcionNegocio.ExceptionCuentaNoExiste);
                cliente.ValidarGMF();
                cliente.UsuarioModificacion = ObtenerUsuarioRequest(context);
                var indexCuenta = cliente.Cuentas.FindIndex(c => c.Id == cuenta.Id);
                cliente.Cuentas[indexCuenta].GMF = true;
                await _cuentaUseCase.MarcarCuentaGMFAsync(cuenta.Id);
                return await _clienteUseCase.ActualizarCliente(cliente.Id, cliente);
            }, "");

        private string ObtenerUsuarioRequest(ServerCallContext context)
        {
            var st = context.GetHttpContext().User.Claims.Where(u => u.Type == "id").FirstOrDefault().Value;
            return st;
        }

        private async Task<ResponseCliente> HandlerRequestAsync<TResult>(Func<Task<TResult>> request, string message)
        {
            try
            {
                var result = await request() as Cliente;
                var resultadoConvertido = _mapper.Map<ClienteResponse>(result);
                return new()
                {
                    Mensaje = message,
                    Error = false,
                    Data = resultadoConvertido
                };
            }
            catch (BusinessException e)
            {
                return new()
                {
                    Mensaje = e.Message,
                    Error = true,
                    Data = null
                };
            }
            catch (Exception e)
            {
                return new()
                {
                    Mensaje = e.Message,
                    Error = true,
                    Data = null
                };
            }
        }
    }
}