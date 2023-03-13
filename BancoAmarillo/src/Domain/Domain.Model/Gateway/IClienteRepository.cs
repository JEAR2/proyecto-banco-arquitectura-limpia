using Domain.Model.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Model.Gateway
{
    /// <summary>
    /// Interface IClienteRepository
    /// </summary>
    public interface IClienteRepository
    {
        /// <summary>
        /// Crear un nuevo cliente
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        Task<Cliente> CrearCliente(Cliente cliente);

        /// <summary>
        /// Actualizar un Cliente por Id
        /// </summary>
        /// <param name="idCliente"></param>
        /// <param name="cliente"></param>
        /// <returns></returns>
        Task<Cliente> ActualizarCliente(string idCliente, Cliente cliente);

        /// <summary>
        /// Eliminar un cliente por Id
        /// </summary>
        /// <param name="idCliente"></param>
        /// <returns></returns>
        Task<bool> EliminarCliente(string idCliente);

        /// <summary>
        /// Obtener Cliente por Id
        /// </summary>
        /// <param name="idCliente"></param>
        /// <returns></returns>
        Task<Cliente> ObtenerClientePorId(string idCliente);

        /// <summary>
        /// Obtener Todos los Clientes
        /// </summary>
        /// <returns></returns>
        Task<List<Cliente>> ObtenerClientes();
    }
}