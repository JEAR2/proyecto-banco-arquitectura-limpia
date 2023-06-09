using DrivenAdapters.Mongo.Entities;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;

namespace DrivenAdapters.Mongo
{
    /// <summary>
    /// Context is an implementation of <see cref="IContext"/>
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Context : IContext
    {
        private readonly IMongoDatabase _database;

        /// <summary>
        /// crea una nueva instancia de la clase <see cref="Context"/>
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="databaseName"></param>
        public Context(string connectionString, string databaseName)
        {
            MongoClient _mongoClient = new MongoClient(connectionString);
            _database = _mongoClient.GetDatabase(databaseName);
        }

        /// <summary>
        /// <see cref="IContext.Usuarios"/>
        /// </summary>
        public IMongoCollection<UsuarioEntity> Usuarios
        => _database.GetCollection<UsuarioEntity>("Usuarios");

        /// <summary>
        /// <see cref="IContext.Cuentas"/>
        /// </summary>
        public IMongoCollection<CuentaEntity> Cuentas
        => _database.GetCollection<CuentaEntity>("Cuentas");


        /// <summary>
        /// <see cref="IContext.Clientes"/>
        /// </summary>
        public IMongoCollection<ClienteEntity> Clientes
            => _database.GetCollection<ClienteEntity>("Clientes");

        /// <summary>
        /// <see cref="IContext.Transacciones"/>
        /// </summary>
        public IMongoCollection<TransaccionEntity> Transacciones
            => _database.GetCollection<TransaccionEntity>("Transacciones");
    }
}