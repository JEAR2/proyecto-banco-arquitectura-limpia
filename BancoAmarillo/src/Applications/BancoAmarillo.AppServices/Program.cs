using BancoAmarillo.AppServices.Extensions;
using credinet.comun.api;
using EntryPoints.Grpc.Controller;
using FluentValidation.AspNetCore;
using Helpers.ObjectsUtils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SC.Configuration.Provider.Mongo;
using Serilog;
using System;
using System.IO;
using System.Text;
using System.Diagnostics.CodeAnalysis;

[assembly: ExcludeFromCodeCoverage]

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

#region Host Configuration

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonProvider();

builder.Host.UseSerilog((ctx, lc) => lc
       .WriteTo.Console()
       .ReadFrom.Configuration(ctx.Configuration));

#endregion Host Configuration

builder.Services.Configure<ConfiguradorAppSettings>(builder.Configuration.GetRequiredSection(nameof(ConfiguradorAppSettings)));
ConfiguradorAppSettings appSettings = builder.Configuration.GetSection(nameof(ConfiguradorAppSettings)).Get<ConfiguradorAppSettings>();

string country = EnvironmentHelper.GetCountryOrDefault(appSettings.DefaultCountry);

builder.Configuration.AddMongoProvider(
    nameof(MongoConfigurationProvider), appSettings.MongoConnection, country);

#region Service Configuration

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

string policyName = "cors";
builder.Services
    .RegisterCors(policyName)
    .RegisterMongo(appSettings.MongoConnection, $"{appSettings.Database}_{country}")
    .RegisterAsyncGateways(appSettings.ServicesBusConnection)
    .RegisterAutoMapper()
    .RegisterServices()
    .AddVersionedApiExplorer()
    .HabilitarVesionamiento()
    .RegistrarValidacionesFluentValidator()
    ;
builder.Services.AddGrpc(opt => opt.EnableDetailedErrors = true);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    options => options.TokenValidationParameters = new()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.KeyJwt)),
        ClockSkew = TimeSpan.Zero
    }
  );

builder.Services.AddAuthorization();

builder.Services
    .AddHealthChecks()
    .AddMongoDb(appSettings.MongoConnection, name: "MongoDB");

#endregion Service Configuration

WebApplication app = builder.Build();

// Enable middleware to serve generated Swagger as a JSON endpoint.

app.UseCors(policyName);
app.MapGrpcService<AuthController>();
app.MapGrpcService<ClienteController>();
app.MapGrpcService<CuentaController>();
app.MapGrpcService<TransaccionController>();
app.UseAuthentication();
app.UseAuthorization();
app.Run();