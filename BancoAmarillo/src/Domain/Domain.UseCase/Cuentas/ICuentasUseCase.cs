using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using credinet.exception.middleware.models;
using Domain.Model.Entidades;
using Domain.Model.Gateway;
using Helpers.Commons.Exceptions;

namespace Domain.UseCase.Cuentas
{
    public interface ICuentasUseCase
    {
        Task<Cuenta> CrearCuentaAsync(Cuenta cuenta);

        Task<Cuenta> ActualizarCuentaAsync(Cuenta cuenta);

        Task<Cuenta> ObtenerCuentaPorIdAsync(string cuentaId);

        Task<bool> CancelarCuentaAsync(string cuentaId);

        Task<Cuenta> ActivarCuentaAsync(string cuentaId);

        Task<bool> InactivarCuentaAsync(string cuentaId);


        Task<Cuenta> ObtenerCuentaPorNumeroCuentaAsync(string numeroCuenta);
        Task<Cuenta> AgregarTransaccionCuentaAsync(Transaccion transaccion, string IdcuentaObjetivo);

        Task<Cuenta> MarcarCuentaGMFAsync(string idCuenta);
    }
}