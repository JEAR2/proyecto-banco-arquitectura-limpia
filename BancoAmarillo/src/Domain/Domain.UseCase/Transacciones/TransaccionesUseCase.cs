using credinet.exception.middleware.models;
using Domain.Model.Entidades;
using Domain.Model.Entidades.Enums;
using Domain.Model.Gateway;
using Domain.UseCase.Clientes;
using Domain.UseCase.Cuentas;
using Helpers.Commons.Exceptions;
using Helpers.ObjectsUtils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.UseCase.Transacciones
{
    /// <summary>
    /// Transacciones caso de uso
    /// </summary>
    public class TransaccionesUseCase : ITransaccionesUseCase
    {
        private readonly ITransaccionesRepository _transaccionesRepository;
        private readonly ICuentasUseCase _cuentaUseCase;
        private readonly IClienteUseCase _clienteUseCase;
        private readonly ITransaccionesEventsRepository _transaccionesEventsRepository;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="transaccionesRepository"></param>
        /// <param name="cuentasUseCase"></param>
        /// <param name="clienteUseCase"></param>
        /// <param name="transaccionesEventsRepository"></param>
        public TransaccionesUseCase(ITransaccionesRepository transaccionesRepository, ICuentasUseCase cuentasUseCase, 
            IClienteUseCase clienteUseCase, ITransaccionesEventsRepository transaccionesEventsRepository)
        {
            _transaccionesRepository = transaccionesRepository;
            _cuentaUseCase = cuentasUseCase;
            _clienteUseCase = clienteUseCase;
            _transaccionesEventsRepository = transaccionesEventsRepository;
        }

        /// <summary>
        /// Realizar transacción
        /// </summary>
        /// <param name="transaccion"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<Transaccion> RealizarTransaccion(Transaccion transaccion)
        {
            transaccion.ValidarValorTransaccion(transaccion.Valor);

            if (transaccion.TipoTransaccion == TipoTransaccion.TRANSFERENCIA)
            {
                await GestionarTransferencia(transaccion);               
            }

            if (transaccion.TipoTransaccion == TipoTransaccion.CONSIGNACION)
            {
                await GestionarConsignacion(transaccion);
            }

            if (transaccion.TipoTransaccion == TipoTransaccion.RETIRO)
            {
                await GestionarRetiro(transaccion);
            }
            return transaccion;
        }

        /// <summary>
        /// Gestionar retiro
        /// </summary>
        /// <param name="transaccion"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task GestionarRetiro(Transaccion transaccion)
        {
            var cuentaReceptora = await _cuentaUseCase.ObtenerCuentaPorIdAsync(transaccion.IdCuentaReceptora);
            if (cuentaReceptora == null)
            {
                throw new BusinessException(TipoExcepcionNegocio.ExcepcionTransaccionCuentaReceptoraNoExiste.GetDescription(),
                    (int)TipoExcepcionNegocio.ExcepcionTransaccionCuentaReceptoraNoExiste);
            }
            var valorTransaccion = transaccion.Valor;
            transaccion.Valor = valorTransaccion;
            transaccion.TipoMovimiento = TipoMovimiento.DEBITO;
            var transaccionAgregada = await _transaccionesRepository.CrearTransaccionAsync(transaccion);
            await _cuentaUseCase.AgregarTransaccionCuentaAsync(transaccionAgregada, transaccionAgregada.IdCuentaReceptora);
            var cliente = await _clienteUseCase.ObtenerClientePorId(cuentaReceptora.IdCliente);
            await _transaccionesEventsRepository.NotificarTransaccionRealizada(cliente.Correo, transaccion);
        }

        /// <summary>
        /// Gestionar consignacion
        /// </summary>
        /// <param name="transaccion"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task GestionarConsignacion(Transaccion transaccion)
        {
            var cuentaReceptora = await _cuentaUseCase.ObtenerCuentaPorIdAsync(transaccion.IdCuentaReceptora);
            if (cuentaReceptora == null)
            {
                throw new BusinessException(TipoExcepcionNegocio.ExcepcionTransaccionCuentaReceptoraNoExiste.GetDescription(),
                    (int)TipoExcepcionNegocio.ExcepcionTransaccionCuentaReceptoraNoExiste);
            }
            transaccion.TipoMovimiento = TipoMovimiento.CREDITO;
            var transaccionAgregada = await _transaccionesRepository.CrearTransaccionAsync(transaccion);
            await _cuentaUseCase.AgregarTransaccionCuentaAsync(transaccionAgregada, transaccionAgregada.IdCuentaReceptora);
            var cliente = await _clienteUseCase.ObtenerClientePorId(cuentaReceptora.IdCliente);
            await _transaccionesEventsRepository.NotificarTransaccionRealizada(cliente.Correo, transaccion);
        }

        /// <summary>
        /// Gestionar Transferencia
        /// </summary>
        /// <param name="transaccion"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task GestionarTransferencia(Transaccion transaccion)
        {
            var cuentaEmisora = await _cuentaUseCase.ObtenerCuentaPorIdAsync(transaccion.IdCuentaEmisora);
            if (cuentaEmisora == null)
            {
                throw new BusinessException(TipoExcepcionNegocio.ExcepcionTransaccionCuentaEmisoraNoExiste.GetDescription(),
                    (int)TipoExcepcionNegocio.ExcepcionTransaccionCuentaEmisoraNoExiste);
            }

            transaccion.ValidarEmisorEstado(cuentaEmisora);

            var cuentaReceptora = await _cuentaUseCase.ObtenerCuentaPorIdAsync(transaccion.IdCuentaReceptora);
            if (cuentaReceptora == null)
            {
                throw new BusinessException(TipoExcepcionNegocio.ExcepcionTransaccionCuentaReceptoraNoExiste.GetDescription(),
                    (int)TipoExcepcionNegocio.ExcepcionTransaccionCuentaReceptoraNoExiste);
            }

            var clienteEmisor = await _clienteUseCase.ObtenerClientePorId(cuentaEmisora.IdCliente);
            var clienteReceptor = await _clienteUseCase.ObtenerClientePorId(cuentaReceptora.IdCliente);

            transaccion.FechaMovimiento = DateTime.UtcNow.ToLocalTime();

            await DesplegarTransaccionParaEmisor(transaccion, clienteEmisor.Correo);
            await DesplegarTransaccionParaReceptor(transaccion, clienteReceptor.Correo);
        }

        /// <summary>
        /// Obtener transaccion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<Transaccion> ObtenerTransaccion(string id)
        {
            var transaccion = await _transaccionesRepository.ObtenerTransaccionPorIdAsync(id);
            if (transaccion == null)
            {
                throw new BusinessException(TipoExcepcionNegocio.ExcepcionTransaccionNoExiste.GetDescription(),
                       (int)TipoExcepcionNegocio.ExcepcionTransaccionNoExiste);
            }
            return transaccion;
        }

        /// <summary>
        /// desplegar las transacciones a cuenta emisora
        /// </summary>
        /// <param name="transaccionPorDesplegar"></param>
        /// <param name="correo"></param>
        /// <returns></returns>
        public async Task DesplegarTransaccionParaEmisor(Transaccion transaccionPorDesplegar, string correo)
        {
            var valorTransaccion = transaccionPorDesplegar.Valor;
            transaccionPorDesplegar.Valor = valorTransaccion;
            transaccionPorDesplegar.TipoMovimiento = TipoMovimiento.DEBITO;
            var transaccionAgregada = await _transaccionesRepository.CrearTransaccionAsync(transaccionPorDesplegar);
            await _cuentaUseCase.AgregarTransaccionCuentaAsync(transaccionAgregada, transaccionAgregada.IdCuentaEmisora);
            await _transaccionesEventsRepository.NotificarTransaccionRealizada(correo, transaccionAgregada);
        }

        /// <summary>
        /// desplegar las transacciones a cuenta receptora
        /// </summary>
        /// <param name="transaccionPorDesplegar"></param>
        /// <param name="correo"></param>
        /// <returns></returns>
        public async Task DesplegarTransaccionParaReceptor(Transaccion transaccionPorDesplegar, string correo)
        {
            transaccionPorDesplegar.TipoMovimiento = TipoMovimiento.CREDITO;
            var transaccionAgregada = await _transaccionesRepository.CrearTransaccionAsync(transaccionPorDesplegar);
            await _cuentaUseCase.AgregarTransaccionCuentaAsync(transaccionAgregada, transaccionAgregada.IdCuentaReceptora);
            await _transaccionesEventsRepository.NotificarTransaccionRealizada(correo, transaccionAgregada);
        }
    }
}
