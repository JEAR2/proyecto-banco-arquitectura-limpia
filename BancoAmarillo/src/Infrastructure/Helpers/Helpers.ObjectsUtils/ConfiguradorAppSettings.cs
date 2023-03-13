using Helpers.ObjectsUtils.ApplicationSettings;

namespace Helpers.ObjectsUtils
{
    /// <summary>
    /// ConfiguradorAppSettings
    /// </summary>
    public class ConfiguradorAppSettings
    {
        /// <summary>
        /// AppId
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// AppSecret
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// Nombre del dominio
        /// </summary>
        public string DomainName { get; set; }

        /// <summary>
        /// MongoDBConnection
        /// </summary>
        public string MongoDBConnection { get; set; }

        /// <summary>
        /// Pais por defecto
        /// </summary>
        public string DefaultCountry { get; set; }

        /// <summary>
        /// Database
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// StorageConnection
        /// </summary>
        public string StorageConnection { get; set; }

        /// <summary>
        /// StorageContainerName
        /// </summary>
        public string StorageContainerName { get; set; }

        /// <summary>
        /// RedisCacheConnectionString
        /// </summary>
        public string RedisCacheConnectionString { get; set; }

        /// <summary>
        /// EndPoint de HealthChecks
        /// </summary>
        public string HealthChecksEndPoint { get; set; }

        /// <summary>
        /// Validation
        /// </summary>
        public ValidationSettings Validation { get; set; }

        /// <summary>
        /// Gets or sets the instancias redis.
        /// </summary>
        /// <value>
        /// The instancias redis.
        /// </value>
        public SettingInstanciaRedis InstanciasRedis { get; set; }

        /// <summary>
        /// Retorna o asigna el/la sql connection.
        /// </summary>
        /// <value>Conexión a mongo.</value>
        public string MongoConnection { get; set; }

        /// <summary>
        /// Retorna o asigna la conexion a redis cache
        /// </summary>
        public string RedisConnection { get; set; }

        /// <summary>
        /// Llave secreat del token
        /// </summary>
        public string KeyJwt { get; set; }

        /// <summary>
        /// Services bus connection
        /// </summary>
        public string ServicesBusConnection { get; set; }

        /// <summary>
        /// TopicTransacciones
        /// </summary>
        public string TopicTransacciones { get; set; }
    }
}