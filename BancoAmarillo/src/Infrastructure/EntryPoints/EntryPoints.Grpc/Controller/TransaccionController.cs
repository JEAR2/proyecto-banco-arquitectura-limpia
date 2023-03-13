using AutoMapper;
using credinet.exception.middleware.models;
using Domain.Model.Entidades;
using Domain.UseCase.Cuentas;
using Domain.UseCase.Transacciones;
using EntryPoints.Grpc.Dtos.Protos;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntryPoints.Grpc.Controller
{
    public class TransaccionController : TransaccionService.TransaccionServiceBase
    {
        private readonly ITransaccionesUseCase _transaccionesUseCase;
        private readonly IMapper _mapper;

        public TransaccionController(ITransaccionesUseCase transaccionesUseCase, IMapper mapper)
        {
            _transaccionesUseCase = transaccionesUseCase;
            _mapper = mapper;
        }

        public override async Task<TransaccionRespuesta> RealizarTransaccion(TransaccionRequest request, ServerCallContext context)
        => await HandleRequestAsync(async () =>
        {
            var transaccion = _mapper.Map<Transaccion>(request);
            var respuesta = await _transaccionesUseCase.RealizarTransaccion(transaccion);
            return respuesta;
        }, "Realizar transacción");

        private async Task<TransaccionRespuesta> HandleRequestAsync<TResult>(Func<Task<TResult>> request, string message)
        {
            try
            {
                var resultado = await request() as Transaccion;
                var resultadoconvertido = _mapper.Map<TransaccionInfo>(resultado);
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
