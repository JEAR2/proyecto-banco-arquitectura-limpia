using AutoMapper.Data;
using BancoAmarillo.AppServices.Automapper;
using credinet.comun.api;
using Domain.Model.Gateway;
using Domain.Model.Interfaces;
using Domain.UseCase.Auth;
using Domain.UseCase.Clientes;
using Domain.UseCase.Common;
using Domain.UseCase.Cuentas;
using Domain.UseCase.Transacciones;
using DrivenAdapters.Mongo;
using DrivenAdapters.Mongo.Adaptadores;
using EntryPoints.Grpc.Validaciones;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using org.reactivecommons.api.impl;
using org.reactivecommons.api;
using StackExchange.Redis;
using System;
using DrivenAdapters.ServiceBus.Entities;
using DrivenAdapters.ServiceBus;
using System.Diagnostics.CodeAnalysis;

namespace BancoAmarillo.AppServices.Extensions
{
    /// <summary>
    /// Service Extensions
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ServiceExtensions
    {
        /// <summary>
        /// Registers the cors.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="policyName">Name of the policy.</param>
        /// <returns></returns>
        public static IServiceCollection RegisterCors(this IServiceCollection services, string policyName) =>
            services.AddCors(o => o.AddPolicy(policyName, builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

        /// <summary>
        /// Método para registrar AutoMapper
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IServiceCollection RegisterAutoMapper(this IServiceCollection services) =>
            services.AddAutoMapper(cfg =>
            {
                cfg.AddDataReaderMapping();
            }, typeof(ConfigurationProfile));

        /// <summary>
        /// Método para registrar Mongo
        /// </summary>
        /// <param name="services">services.</param>
        /// <param name="connectionString">connection string.</param>
        /// <param name="db">database.</param>
        /// <returns></returns>
        public static IServiceCollection RegisterMongo(this IServiceCollection services, string connectionString, string db) =>
                                    services.AddSingleton<IContext>(provider => new Context(connectionString, db));

        /// <summary>
        ///   Método para registrar Redis Cache
        /// </summary>
        /// <param name="services">services.</param>
        /// <param name="connectionString">connection string.</param>
        /// <param name="dbNumber">database number.</param>
        /// <returns></returns>
        public static IServiceCollection RegisterRedis(this IServiceCollection services, string connectionString, int dbNumber)
        {
            services.AddSingleton(s => LazyConnection(connectionString).Value.GetDatabase(dbNumber));

            ConnectionMultiplexer multiplexer = ConnectionMultiplexer.Connect(connectionString,
                opt => opt.DefaultDatabase = dbNumber);
            services.AddSingleton<IConnectionMultiplexer>(multiplexer);

            return services;
        }

        /// <summary>
        /// Método para registrar los servicios
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            #region Helpers

            services.AddSingleton<IMensajesHelper, MensajesApiHelper>();

            #endregion Helpers

            #region UseCases

            services.AddScoped<IManageEventsUseCase, ManageEventsUseCase>();
            services.AddScoped<IAuthUseCase, AuthUseCase>();
            services.AddScoped<IClienteUseCase, ClienteUseCase>();
            services.AddScoped<ICuentasUseCase, CuentasUseCase>();
            services.AddScoped<ITransaccionesUseCase, TransaccionesUseCase>();

            #endregion UseCases

            #region Adapters

            services.AddScoped<IAuthRepository, UsuarioAdapter>();
            services.AddScoped<IClienteRepository, ClienteAdapter>();
            services.AddScoped<ICuentaRepository, CuentaAdapter>();
            services.AddScoped<ITransaccionesRepository, TransaccionAdapter>();
            services.AddScoped<ITransaccionesEventsRepository, TransaccionesEventsRepository>();

            #endregion Adapters

            return services;
        }

        /// <summary>
        /// Registra la validaciones de modelo
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection RegistrarValidacionesFluentValidator(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<ValidacionUsuario>();
            services.AddValidatorsFromAssemblyContaining<ValidacionCliente>();
            return services;
        }

        /// <summary>
        ///   Lazies the connection.
        /// </summary>
        /// <param name="connectionString">connection string.</param>
        /// <returns></returns>
        private static Lazy<ConnectionMultiplexer> LazyConnection(string connectionString) =>
            new(() =>
            {
                return ConnectionMultiplexer.Connect(connectionString);
            });

        /// <summary>
        /// RegisterAsyncGateways
        /// </summary>
        /// <param name="services"></param>
        /// <param name="serviceBusConn"></param>
        public static IServiceCollection RegisterAsyncGateways(this IServiceCollection services,
                string serviceBusConn)
        {
            services.RegisterAsyncGateway<TransaccionEntityServiceBus>(serviceBusConn);
            return services;
        }

        private static void RegisterAsyncGateway<TEntity>(this IServiceCollection services, string serviceBusConn) =>
                services.AddSingleton<IDirectAsyncGateway<TEntity>>(new DirectAsyncGatewayServiceBus<TEntity>(serviceBusConn));
    }
}