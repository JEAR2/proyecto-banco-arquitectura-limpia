using AutoMapper;
using credinet.exception.middleware.models;
using Domain.Model.Entidades;
using Domain.Model.Gateway;
using Helpers.Commons.Exceptions;
using PasswordGenerator;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Domain.UseCase.Clientes
{
    /// <summary>
    /// <see cref=IClienteUseCase""/>
    /// </summary>
    public class ClienteUseCase : IClienteUseCase
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="clienteRepository"></param>
        public ClienteUseCase(IClienteRepository clienteRepository, IAuthRepository authRepository, IMapper mapper)
        {
            _clienteRepository = clienteRepository;
            _authRepository = authRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// <see cref="IClienteUseCase.CrearCliente(Cliente)"/>
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        public async Task<Cliente> CrearCliente(Cliente cliente)
        {
            var password = new Password(10);

            var usuario = new Usuario()
            {
                Correo = cliente.Correo,
                Clave = password.Next()
            };

            var usuarioCreado = await _authRepository.RegistrarUsuario(usuario);
            cliente.Id = usuarioCreado.Id;
            cliente.FechaCreacion = DateTime.Now;
            cliente.FechaModificacion = DateTime.Now;
            cliente.Estado = Model.Entidades.Enums.EstadoCliente.ACTIVO;
            cliente.Cuentas = new List<Cuenta>();
            return await _clienteRepository.CrearCliente(cliente);
        }

        /// <summary>
        /// <see cref="IClienteUseCase.ObtenerClientePorId(string)"/>
        /// </summary>
        /// <param name="idCliente"></param>
        /// <returns></returns>
        public Task<Cliente> ObtenerClientePorId(string idCliente)
        {
            return _clienteRepository.ObtenerClientePorId(idCliente);
        }

        /// <summary>
        /// <see cref="IClienteUseCase.ObtenerClientes"/>
        /// </summary>
        /// <returns></returns>
        public Task<List<Cliente>> ObtenerClientes()
        {
            return _clienteRepository.ObtenerClientes();
        }

        /// <summary>
        /// <see cref="IClienteUseCase.ActualizarCliente(string, Cliente)"/>
        /// </summary>
        /// <param name="idCliente"></param>
        /// <param name="cliente"></param>
        /// <returns></returns>
        public async Task<Cliente> ActualizarCliente(string idCliente, Cliente cliente)
        {
            var existeCliente = await ValidarCliente(idCliente);

            //var clienteActualizado = _mapper.Map(cliente, existeCliente);
            existeCliente.Nombre = cliente.Nombre;
            existeCliente.Apellido = cliente.Apellido;
            existeCliente.Cuentas = cliente.Cuentas;
            existeCliente.FechaModificacion = DateTime.Now;
            existeCliente.UsuarioModificacion = cliente.UsuarioModificacion;
            return await _clienteRepository.ActualizarCliente(idCliente, existeCliente);
        }

        /// <summary>
        /// <see cref="IClienteUseCase.EliminarCliente(string)"/>
        /// </summary>
        /// <param name="idCliente"></param>
        /// <returns></returns>
        public async Task<Cliente> EliminarCliente(string idCliente)
        {
            var cliente = await ValidarCliente(idCliente);
            cliente.ValidarCuentasCanceladas();
            await _clienteRepository.EliminarCliente(idCliente);
            cliente.FechaModificacion = DateTime.Now;
            cliente.Estado = Model.Entidades.Enums.EstadoCliente.INACTIVO;
            return cliente;
        }

        /// <summary>
        /// Método para validar que exista un cliente
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        private async Task<Cliente> ValidarCliente(string Id)
        {
            var cliente = await _clienteRepository.ObtenerClientePorId(Id);
            if (cliente is null)
                throw new BusinessException("Cliente no encontrado", (int)TipoExcepcionNegocio.ExceptionClienteNoEncontrado);

            return cliente;
        }
    }
}