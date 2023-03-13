using AutoMapper;
using credinet.exception.middleware.models;
using Domain.Model.Entidades;
using Domain.UseCase.Auth;
using EntryPoints.Grpc.Dtos.Protos;
using EntryPoints.Grpc.Validaciones;
using FluentValidation;
using FluentValidation.Results;
using Grpc.Core;
using Helpers.Commons.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace EntryPoints.Grpc.Controller
{
    public class AuthController : Auth.AuthBase
    {
        private readonly IAuthUseCase _authUseCase;
        private readonly IMapper _mapper;
        private readonly IValidator<UsuarioProto> _validator;

        public AuthController(IAuthUseCase authUseCase, IMapper mapper, IValidator<UsuarioProto> validator)
        {
            _authUseCase = authUseCase;
            _mapper = mapper;
            _validator = validator;
        }

        [Authorize(Roles = "ADMIN")]
        public override async Task<RespuestaAuth> CrearUsuario(UsuarioProto request, ServerCallContext context)

           => await HandlerRequestAsync(async () =>
         {
             await _validator.ValidateAsync(request).ModeloValido();
             var usuario = _mapper.Map<Usuario>(request);
             var accessToken = await _authUseCase.CrearUsuario(usuario);
             return accessToken;
         }, "Acceso Autorizado");

        public override async Task<RespuestaAuth> IniciarSesion(UsuarioProto request, ServerCallContext context)
        => await HandlerRequestAsync(async () =>
            {
                var usuario = _mapper.Map<Usuario>(request);
                return await _authUseCase.IniciarSesion(usuario);
            }, "Acceso Autorizado");

        private async Task<RespuestaAuth> HandlerRequestAsync<TResult>(Func<Task<TResult>> request, string message)
        {
            try
            {
                var resultado = await request() as AccesToken;

                return new()
                {
                    Mensaje = message,
                    Error = false,
                    Token = resultado!.AccessToken
                };
            }
            catch (BusinessException e)
            {
                return new()
                {
                    Mensaje = e.Message,
                    Error = true,
                    Token = ""
                };
                throw;
            }
            catch (Exception e)
            {
                return new()
                {
                    Mensaje = e.Message,
                    Error = true,
                    Token = ""
                };
            }
        }
    }
}