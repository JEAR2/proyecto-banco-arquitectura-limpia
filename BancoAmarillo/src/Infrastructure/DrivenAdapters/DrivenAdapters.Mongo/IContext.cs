using DrivenAdapters.Mongo.Entities;
using MongoDB.Driver;

namespace DrivenAdapters.Mongo
{
    /// <summary>
    /// Interfaz Mongo context contract.
    /// </summary>
    public interface IContext
    {
        /// <summary>
        /// Usuarios
        /// </summary>
        public IMongoCollection<UsuarioEntity> Usuarios { get; }
        /// <summary>
        /// Cuentas
        /// </summary>
        public IMongoCollection<CuentaEntity> Cuentas { get; }

        /// <summary>
        /// Usuarios
        /// </summary>
        public IMongoCollection<ClienteEntity> Clientes { get; }

        /// <summary>
        /// Transacciones
        /// </summary>
        public IMongoCollection<TransaccionEntity> Transacciones { get; }
    }
}