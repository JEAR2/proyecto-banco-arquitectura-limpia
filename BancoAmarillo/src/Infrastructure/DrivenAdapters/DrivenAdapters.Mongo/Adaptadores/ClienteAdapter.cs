using AutoMapper;
using Domain.Model.Entidades;
using Domain.Model.Entidades.Enums;
using Domain.Model.Gateway;
using DrivenAdapters.Mongo.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DrivenAdapters.Mongo.Adaptadores
{
    /// <summary>
    /// <see cref="IClienteRepository"/>
    /// </summary>
    public class ClienteAdapter : IClienteRepository
    {
        private readonly IContext _context;
        private readonly IMapper _mapper;
        private readonly FilterDefinitionBuilder<ClienteEntity> _filtro;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        public ClienteAdapter(IContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _filtro = Builders<ClienteEntity>.Filter;
        }

        /// <summary>
        /// <see cref="IClienteRepository.CrearCliente(Cliente)"/>
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        public async Task<Cliente> CrearCliente(Cliente cliente)
        {
            ClienteEntity clienteEntity = _mapper.Map<ClienteEntity>(cliente);
            await _context.Clientes.InsertOneAsync(clienteEntity);
            return _mapper.Map<Cliente>(clienteEntity);
        }

        /// <summary>
        /// <see cref="IClienteRepository.ObtenerClientePorId(string)"/>
        /// </summary>
        /// <param name="idCliente"></param>
        /// <returns></returns>
        public async Task<Cliente> ObtenerClientePorId(string idCliente)
        {
            var clienteEntity = await _context.Clientes.FindAsync(_filtro.Eq(cliente => cliente.Id, idCliente));
            return _mapper.Map<Cliente>(clienteEntity.FirstOrDefault());
        }

        /// <summary>
        /// <see cref="IClienteRepository.ObtenerClientes"/>
        /// </summary>
        /// <returns></returns>
        public async Task<List<Cliente>> ObtenerClientes()
        {
            var cursor = await _context.Clientes.FindAsync(_filtro.Eq(cliente => cliente.Estado, Entities.Enums.EstadoCliente.ACTIVO));
            var clientes = cursor.ToList();
            return _mapper.Map<List<Cliente>>(clientes);
        }

        /// <summary>
        /// <see cref="IClienteRepository.ActualizarCliente(string, Cliente)"/>
        /// </summary>
        /// <param name="idCliente"></param>
        /// <param name="cliente"></param>
        /// <returns></returns>
        public async Task<Cliente> ActualizarCliente(string idCliente, Cliente cliente)
        {
            var cursor = await _context.Clientes.FindAsync(c => c.Id.Equals(idCliente));
            var clienteEntity = cursor.FirstOrDefault();
            var clienteActualizado = _mapper.Map(cliente, clienteEntity);

            await _context.Clientes.ReplaceOneAsync(_filtro.Eq(c => c.Id, idCliente), clienteActualizado);

            return _mapper.Map<Cliente>(clienteActualizado);
        }

        /// <summary>
        /// <see cref="IClienteRepository.EliminarCliente(string)"/>
        /// </summary>
        /// <param name="idCliente"></param>
        /// <returns></returns>
        public async Task<bool> EliminarCliente(string idCliente)
        {
            try
            {
                var update = Builders<ClienteEntity>.Update.Set(cliente => cliente.Estado, Entities.Enums.EstadoCliente.INACTIVO);
                var resultado = await _context.Clientes.UpdateOneAsync(_filtro.Eq(cliente => cliente.Id, idCliente), update);
                return true;
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}