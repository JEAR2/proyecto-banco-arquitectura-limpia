using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.Entidades;
using Domain.Model.Entidades.Enums;

namespace Domain.Model.Gateway
{
    public interface ICuentaRepository
    {

        Task<Cuenta> CrearCuentaAsync(Cuenta cuenta);

        Task<Cuenta> ActualizarCuentaAsync(Cuenta cuenta);

        Task<Cuenta> ObtenerCuentaPorIdAsync(string cuentaId);

        Task<Cuenta> ObtenerCuentaPorNumeroCuentaAsync(string numeroCuenta);

        Task<List<Cuenta>> ObtenerCuentasPorIdClienteAsync(string idCliente);

        Task<long> ObtenerCountCuentasTipoAsync(TipoCuenta tipo);
    }
}
