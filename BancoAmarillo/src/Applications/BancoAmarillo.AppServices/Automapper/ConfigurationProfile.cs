using AutoMapper;
using Domain.Model.Entidades;
using Domain.Model.Entidades.Enums;
using Domain.Model.Interfaces;
using DrivenAdapters.Mongo.Entities;
using EntryPoints.Grpc.Dtos.Protos;
using EntryPoints.Grpc.Dtos.Protos.Cuentas;
using EntryPoints.Grpc.Dtos.Protos.Cliente;
using System;
using System.Diagnostics.CodeAnalysis;

namespace BancoAmarillo.AppServices.Automapper
{
    /// <summary>
    /// EntityProfile
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ConfigurationProfile : Profile
    {
        /// <summary>
        /// ConfigurationProfile
        /// </summary>
        public ConfigurationProfile()
        {
            #region DTO entities

            CreateMap<Usuario, UsuarioEntity>().ReverseMap();
            CreateMap<Cuenta, CuentaEntity>().ReverseMap();
            CreateMap<Cliente, ClienteEntity>().ForAllMembers(opt => opt.Condition((src, dest, srcMember) =>
            {
                return srcMember != null && !srcMember.Equals("");
            }
            ));
            CreateMap<Cuenta, CuentaEntity>().ForAllMembers(opt => opt.Condition((src, dest, srcMember) =>
            {
                return srcMember != null && !srcMember.Equals("");
            }));

            CreateMap<CuentaEntity, Cuenta>();
            CreateMap<ClienteEntity, Cliente>();

            CreateMap<Transaccion, TransaccionEntity>().ReverseMap();

            #endregion DTO entities

            #region DTOProtos

            CreateMap<UsuarioProto, Usuario>().ReverseMap();
            CreateMap<AccesToken, RespuestaAuth>().ReverseMap();
            CreateMap<CuentaProto, Cuenta>().ReverseMap();
            CreateMap<CuentaCompleta, Cuenta>().ReverseMap();
            CreateMap<ClienteRequest, Cliente>().ReverseMap();
            CreateMap<ClienteResponse, Cliente>().ReverseMap();
            CreateMap<ActualizarClienteProto, Cliente>().ReverseMap();
            CreateMap<CuentasClienteResponse, Cuenta>().ReverseMap();
            CreateMap<TransaccionInfo, Transaccion>().ReverseMap();
            CreateMap<TransaccionRequest, Transaccion>().ReverseMap();
            CreateMap<TransaccionProto, Transaccion>().ReverseMap();

            #endregion DTOProtos
        }
    }
}