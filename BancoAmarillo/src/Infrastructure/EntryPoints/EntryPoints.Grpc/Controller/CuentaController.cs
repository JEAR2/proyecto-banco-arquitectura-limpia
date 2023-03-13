using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using credinet.exception.middleware.models;
using Domain.Model.Entidades;
using Domain.UseCase.Auth;
using Domain.UseCase.Cuentas;
using EntryPoints.Grpc.Dtos.Protos.Cuentas;
using EntryPoints.Grpc.Dtos.Protos.Cliente;
using Google.Protobuf.Collections;
using Grpc.Core;

namespace EntryPoints.Grpc.Controller
{
    public class CuentaController : CuentaService.CuentaServiceBase
    {
        private readonly ICuentasUseCase _cuentaUseCase;
        private readonly IMapper _mapper;

        public CuentaController(ICuentasUseCase cuentaUseCase, IMapper mapper)
        {
            _cuentaUseCase = cuentaUseCase;
            _mapper = mapper;
        }

        /// <summary>
        /// Crear Cuenta
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<RespuestaCuenta> CrearCuenta(CuentaProto request, ServerCallContext context)
         => await HandleRequestAsync(async () =>
         {
             var cuenta = _mapper.Map<Cuenta>(request);
             var repuesta = await _cuentaUseCase.CrearCuentaAsync(cuenta);
             return repuesta;
         }, "Cuenta Creada");

        /// <summary>
        /// Obtener Cuenta por Id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<RespuestaCuenta> ObtenerCuentaId(CuentaRequest request, ServerCallContext context)
         => await HandleRequestAsync(async () =>
         {
             var repuesta = await _cuentaUseCase.ObtenerCuentaPorIdAsync(request.IdCuenta);
             return repuesta;
         }, "Cuenta Encontrada");

        /// <summary>
        /// Obtener Cuenta por Numero Cuenta
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<RespuestaCuenta> ObtenerCuentaNumero(NumeroCuentaRequest request, ServerCallContext context)
         => await HandleRequestAsync(async () =>
         {
             var repuesta = await _cuentaUseCase.ObtenerCuentaPorNumeroCuentaAsync(request.Numerocuenta);
             return repuesta;
         }, "Cuenta Encontrada");

        /// <summary>
        /// Actualizar Cuenta
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<RespuestaCuenta> ActualizarCuenta(CuentaCompleta request, ServerCallContext context)
         => await HandleRequestAsync(async () =>
         {
             var cuenta = _mapper.Map<Cuenta>(request);
             var repuesta = await _cuentaUseCase.ActualizarCuentaAsync(cuenta);
             return repuesta;
         }, "Cuenta Actualizada");

        /// <summary>
        /// Cancelar Cuenta
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<RespuestaCuenta> CancelarCuenta(CuentaRequest request, ServerCallContext context)
         => await HandleRequestAsync(async () =>
         {
             await _cuentaUseCase.CancelarCuentaAsync(request.IdCuenta);
             return new CuentaCompleta();
         }, "Cuenta Cancelada");

        /// <summary>
        /// Activar Cuenta
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<RespuestaCuenta> ActivarCuentaAsync(CuentaRequest request, ServerCallContext context)
        => await HandleRequestAsync(async () =>
        {
            var repuesta = await _cuentaUseCase.ActivarCuentaAsync(request.IdCuenta);
            return repuesta;
        }, "Cuenta Activada");

        /// <summary>
        /// Inactivar Cuenta
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<RespuestaCuenta> InactivarCuentaAsync(CuentaRequest request, ServerCallContext context)
        => await HandleRequestAsync(async () =>
        {
            await _cuentaUseCase.InactivarCuentaAsync(request.IdCuenta);
            return new CuentaCompleta();
        }, "Cuenta ha sido desactivada");

        /// <summary>
        /// Gestor de respuesta
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="request"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task<RespuestaCuenta> HandleRequestAsync<TResult>(Func<Task<TResult>> request, string message)
        {
            try
            {
                var resultado = await request() as Cuenta;
                var resultadoconvertido = _mapper.Map<CuentaCompleta>(resultado);
                return new()
                {
                    Mensaje = message,
                    Error = false,
                    Data = resultadoconvertido
                };
            }
            catch (BusinessException e)
            {
                return new()
                {
                    Mensaje = e.Message,
                    Error = true,
                    Data = { }
                };
                throw;
            }
            catch (Exception e)
            {
                return new()
                {
                    Mensaje = e.Message,
                    Error = true,
                    Data = { }
                };
            }
        }
 
    }
}