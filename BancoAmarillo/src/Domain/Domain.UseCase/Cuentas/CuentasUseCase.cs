using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.Entidades;
using Domain.Model.Gateway;
using Domain.Model.Entidades.Enums;
using credinet.exception.middleware.models;
using Helpers.Commons.Exceptions;
using Helpers.ObjectsUtils.Extensions;
using static Microsoft.Azure.Amqp.Serialization.SerializableType;

namespace Domain.UseCase.Cuentas
{
    public class CuentasUseCase : ICuentasUseCase
    {
        private readonly ICuentaRepository _cuentaRepository;
        private readonly IClienteRepository _clienteRepository;

        public CuentasUseCase(ICuentaRepository cuentaRepository, IClienteRepository clienteRepository)
        {
            _cuentaRepository = cuentaRepository;
            _clienteRepository = clienteRepository;
        }

        /// <summary>
        /// Actualizar Cuenta
        /// </summary>
        /// <param name="cuenta"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<Cuenta> ActualizarCuentaAsync(Cuenta cuenta)
        {
            var cuentaencontrada = await _cuentaRepository.ObtenerCuentaPorIdAsync(cuenta.Id);

            if (cuentaencontrada == null)
                throw new BusinessException(TipoExcepcionNegocio.ExceptionCuentaNoExiste.GetDescription()
                   , (int)TipoExcepcionNegocio.ExceptionCuentaNoExiste);
            var clienteCuenta = await _clienteRepository.ObtenerClientePorId(cuenta.IdCliente);

            if (clienteCuenta == null)
            {
                throw new BusinessException(TipoExcepcionNegocio.ExceptionClienteNoExiste.GetDescription()
                    , (int)TipoExcepcionNegocio.ExceptionClienteNoExiste);
            }

            cuenta.FechaModificacion = DateTime.Now;
            var cuentaactualizada = await _cuentaRepository.ActualizarCuentaAsync(cuenta);

            var Index = clienteCuenta.Cuentas.FindIndex(c => c.Id == cuenta.Id);
            clienteCuenta.Cuentas[Index] = cuentaactualizada;
            await _clienteRepository.ActualizarCliente(clienteCuenta.Id, clienteCuenta);

            return cuentaactualizada;
        }

        /// <summary>
        /// Agregar Transaccion a la Cuenta
        /// </summary>
        /// <param name="transaccion"></param>
        /// <param name="IdcuentaObjetivo"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<Cuenta> AgregarTransaccionCuentaAsync(Transaccion transaccion, string IdcuentaObjetivo)
        {
            var cuentaencontrada = await _cuentaRepository.ObtenerCuentaPorIdAsync(IdcuentaObjetivo);

            if (cuentaencontrada == null)
                throw new BusinessException(TipoExcepcionNegocio.ExceptionCuentaNoExiste.GetDescription()
                   , (int)TipoExcepcionNegocio.ExceptionCuentaNoExiste);
            cuentaencontrada.ValidarEstado();

            var clienteCuenta = await _clienteRepository.ObtenerClientePorId(cuentaencontrada.IdCliente);

            if (clienteCuenta == null)
            {
                throw new BusinessException(TipoExcepcionNegocio.ExceptionClienteNoExiste.GetDescription()
                    , (int)TipoExcepcionNegocio.ExceptionClienteNoExiste);
            }

            if (transaccion.TipoMovimiento == TipoMovimiento.DEBITO)
            { cuentaencontrada.RealizarRetiro(transaccion.Valor); }

            if (transaccion.TipoMovimiento == TipoMovimiento.CREDITO)
            { cuentaencontrada.RealizarConsignacion(transaccion.Valor); }

            cuentaencontrada.Transacciones.Add(transaccion);

            cuentaencontrada.FechaModificacion = DateTime.Now;
            var cuentaactualizada = await _cuentaRepository.ActualizarCuentaAsync(cuentaencontrada);

            var Index = clienteCuenta.Cuentas.FindIndex(c => c.Id == cuentaencontrada.Id);
            clienteCuenta.Cuentas[Index] = cuentaactualizada;
            await _clienteRepository.ActualizarCliente(clienteCuenta.Id, clienteCuenta);

            return cuentaactualizada;
        }

        /// <summary>
        /// Cancelar Cuenta
        /// </summary>
        /// <param name="cuentaId"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<bool> CancelarCuentaAsync(string cuentaId)
        {
            var cuentaencontrada = await _cuentaRepository.ObtenerCuentaPorIdAsync(cuentaId);

            if (cuentaencontrada == null)
                throw new BusinessException(TipoExcepcionNegocio.ExceptionCuentaNoExiste.GetDescription()
                   , (int)TipoExcepcionNegocio.ExceptionCuentaNoExiste);
            var clienteCuenta = await _clienteRepository.ObtenerClientePorId(cuentaencontrada.IdCliente);

            if (clienteCuenta == null)
            {
                throw new BusinessException(TipoExcepcionNegocio.ExceptionClienteNoExiste.GetDescription()
                    , (int)TipoExcepcionNegocio.ExceptionClienteNoExiste);
            }

            cuentaencontrada.CancelarCuenta();
            cuentaencontrada.FechaModificacion = DateTime.Now;
            await _cuentaRepository.ActualizarCuentaAsync(cuentaencontrada);

            var Index = clienteCuenta.Cuentas.FindIndex(c => c.Id == cuentaencontrada.Id);
            clienteCuenta.Cuentas[Index] = cuentaencontrada;
            await _clienteRepository.ActualizarCliente(clienteCuenta.Id, clienteCuenta);
            return true;
        }

        /// <summary>
        /// Activar Cuenta
        /// </summary>
        /// <param name="cuentaId"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<Cuenta> ActivarCuentaAsync(string cuentaId)
        {
            var cuentaencontrada = await _cuentaRepository.ObtenerCuentaPorIdAsync(cuentaId);

            if (cuentaencontrada == null)
                throw new BusinessException(TipoExcepcionNegocio.ExceptionCuentaNoExiste.GetDescription()
                   , (int)TipoExcepcionNegocio.ExceptionCuentaNoExiste);

            var clienteCuenta = await _clienteRepository.ObtenerClientePorId(cuentaencontrada.IdCliente);

            if (clienteCuenta == null)
            {
                throw new BusinessException(TipoExcepcionNegocio.ExceptionClienteNoExiste.GetDescription()
                    , (int)TipoExcepcionNegocio.ExceptionClienteNoExiste);
            }
            cuentaencontrada.ValidarEstadoCancelada();

            if (cuentaencontrada.EstadoCuenta == EstadoCuenta.ACTIVA)
                throw new BusinessException(TipoExcepcionNegocio.ExceptionCuentaActiva.GetDescription()
            , (int)TipoExcepcionNegocio.ExceptionCuentaActiva);

            cuentaencontrada.EstadoCuenta = EstadoCuenta.ACTIVA;
            cuentaencontrada.FechaModificacion = DateTime.Now;
            await _cuentaRepository.ActualizarCuentaAsync(cuentaencontrada);

            var Index = clienteCuenta.Cuentas.FindIndex(c => c.Id == cuentaencontrada.Id);
            clienteCuenta.Cuentas[Index] = cuentaencontrada;
            await _clienteRepository.ActualizarCliente(clienteCuenta.Id, clienteCuenta);

            return cuentaencontrada;
        }

        /// <summary>
        /// Inactivar Cuenta
        /// </summary>
        /// <param name="cuentaId"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<bool> InactivarCuentaAsync(string cuentaId)
        {
            var cuentaencontrada = await _cuentaRepository.ObtenerCuentaPorIdAsync(cuentaId);

            if (cuentaencontrada == null)
                throw new BusinessException(TipoExcepcionNegocio.ExceptionCuentaNoExiste.GetDescription()
                   , (int)TipoExcepcionNegocio.ExceptionCuentaNoExiste);

            var clienteCuenta = await _clienteRepository.ObtenerClientePorId(cuentaencontrada.IdCliente);

            if (clienteCuenta == null)
            {
                throw new BusinessException(TipoExcepcionNegocio.ExceptionClienteNoExiste.GetDescription()
                    , (int)TipoExcepcionNegocio.ExceptionClienteNoExiste);
            }

            cuentaencontrada.ValidarEstadoCancelada();

            if (cuentaencontrada.EstadoCuenta == EstadoCuenta.INACTIVA)
                throw new BusinessException(TipoExcepcionNegocio.ExceptionCuentaInactiva.GetDescription()
            , (int)TipoExcepcionNegocio.ExceptionCuentaInactiva);

            cuentaencontrada.EstadoCuenta = EstadoCuenta.INACTIVA;
            cuentaencontrada.FechaModificacion = DateTime.Now;
            await _cuentaRepository.ActualizarCuentaAsync(cuentaencontrada);

            var Index = clienteCuenta.Cuentas.FindIndex(c => c.Id == cuentaencontrada.Id);
            clienteCuenta.Cuentas[Index] = cuentaencontrada;
            await _clienteRepository.ActualizarCliente(clienteCuenta.Id, clienteCuenta);
            return true;
        }

        /// <summary>
        /// Crear Cuenta
        /// </summary>
        /// <param name="cuenta"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<Cuenta> CrearCuentaAsync(Cuenta cuenta)
        {
           /* if (cuenta.TipoCuenta != TipoCuenta.AHORRO && cuenta.TipoCuenta != TipoCuenta.CORRIENTE)
            {
                throw new BusinessException(TipoExcepcionNegocio.ExceptioTipoDeCuentaInvalido.GetDescription()
                    , (int)TipoExcepcionNegocio.ExceptioTipoDeCuentaInvalido);
            }*/
            var clienteCuenta = await _clienteRepository.ObtenerClientePorId(cuenta.IdCliente);

            if (clienteCuenta == null)
            {
                throw new BusinessException(TipoExcepcionNegocio.ExceptionClienteNoExiste.GetDescription()
                    , (int)TipoExcepcionNegocio.ExceptionClienteNoExiste);
            }
            if (cuenta.GMF == true)
            {
                var listacuentas = await _cuentaRepository.ObtenerCuentasPorIdClienteAsync(cuenta.IdCliente);
                if (listacuentas != null)
                {
                    if (listacuentas.Any(c => c.GMF == true))
                        throw new BusinessException(TipoExcepcionNegocio.ExceptionClienteYaTieneGMF.GetDescription()
                            , (int)TipoExcepcionNegocio.ExceptionClienteYaTieneGMF);
                }
               
            }

            cuenta.ValidarMinimoAhorros();
            cuenta.EstadoCuenta = EstadoCuenta.ACTIVA;
            cuenta.ActualizarSaldoDisponible();
            cuenta.NumeroCuenta = await ObtenerNumeroCuenta(cuenta);
            cuenta.FechaCreacion = DateTime.Now;
            cuenta.FechaModificacion = DateTime.Now;

            if (clienteCuenta.Cuentas == null)
                clienteCuenta.Cuentas = new List<Cuenta>();

            var cuentaCreada = await _cuentaRepository.CrearCuentaAsync(cuenta);

            clienteCuenta.Cuentas.Add(cuentaCreada);

            await _clienteRepository.ActualizarCliente(clienteCuenta.Id, clienteCuenta);

            return cuentaCreada;
        }

        /// <summary>
        /// Calcula el siguiente Numero Cuenta según tipo
        /// </summary>
        /// <param name="cuenta"></param>
        /// <returns></returns>
        private async Task<string> ObtenerNumeroCuenta(Cuenta cuenta)
        {

           var countCuentas = await _cuentaRepository.ObtenerCountCuentasTipoAsync(cuenta.TipoCuenta);
           
            if (cuenta.TipoCuenta == TipoCuenta.AHORRO)
            {
                long siguienteCuentaAhorro = 4600000000 + countCuentas + 1;
                cuenta.NumeroCuenta = siguienteCuentaAhorro.ToString();
            }

            if (cuenta.TipoCuenta == TipoCuenta.CORRIENTE)
            {
                long siguienteCuentaCorreinte = 2300000000 + countCuentas + 1;
                cuenta.NumeroCuenta = siguienteCuentaCorreinte.ToString();
            }


            return cuenta.NumeroCuenta;
        }

        /// <summary>
        /// Obtener Cuenta por Id Cuenta
        /// </summary>
        /// <param name="cuentaId"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<Cuenta> ObtenerCuentaPorIdAsync(string cuentaId)
        {
            var cuenta = await _cuentaRepository.ObtenerCuentaPorIdAsync(cuentaId);
            if (cuenta == null)
                throw new BusinessException(TipoExcepcionNegocio.ExceptionCuentaNoExiste.GetDescription()
                   , (int)TipoExcepcionNegocio.ExceptionCuentaNoExiste);

            if (cuenta.EstadoCuenta == EstadoCuenta.CANCELADA)
                throw new BusinessException(TipoExcepcionNegocio.ExceptionCuentaCancelada.GetDescription()
                   , (int)TipoExcepcionNegocio.ExceptionCuentaCancelada);

            return cuenta;
        }

        /// <summary>
        /// Obtener Cuenta por Numero Cuenta
        /// </summary>
        /// <param name="numeroCuenta"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<Cuenta> ObtenerCuentaPorNumeroCuentaAsync(string numeroCuenta)
        {
            var cuenta = await _cuentaRepository.ObtenerCuentaPorNumeroCuentaAsync(numeroCuenta);
            if (cuenta == null)
                throw new BusinessException(TipoExcepcionNegocio.ExceptionCuentaNoExiste.GetDescription()
                   , (int)TipoExcepcionNegocio.ExceptionCuentaNoExiste);

            if (cuenta.EstadoCuenta == EstadoCuenta.CANCELADA)
                throw new BusinessException(TipoExcepcionNegocio.ExceptionCuentaCancelada.GetDescription()
                   , (int)TipoExcepcionNegocio.ExceptionCuentaCancelada);

            return cuenta;
        }

        /// <summary>
        /// <see cref="ICuentasUseCase.MarcarCuentaGMFAsync(string)"/>
        /// </summary>
        /// <param name="idCuenta"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<Cuenta> MarcarCuentaGMFAsync(string idCuenta)
        {
            var cuenta = await _cuentaRepository.ObtenerCuentaPorIdAsync(idCuenta);
            if (cuenta == null)
                throw new BusinessException(TipoExcepcionNegocio.ExceptionCuentaNoExiste.GetDescription()
                   , (int)TipoExcepcionNegocio.ExceptionCuentaNoExiste);

            if (cuenta.EstadoCuenta == EstadoCuenta.CANCELADA)
                throw new BusinessException(TipoExcepcionNegocio.ExceptionCuentaCancelada.GetDescription()
                   , (int)TipoExcepcionNegocio.ExceptionCuentaCancelada);

            cuenta.GMF = true;

            return await _cuentaRepository.ActualizarCuentaAsync(cuenta);
        }

        
    }
}