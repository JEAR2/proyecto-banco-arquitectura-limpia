using DrivenAdapters.Mongo.Entities.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DrivenAdapters.Mongo.Entities
{
    /// <summary>
    /// UsuarioEntity
    /// </summary>
    public class UsuarioEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        ///
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// Correo
        /// </summary>
        ///
        [BsonElement(elementName: "correo")]
        public string Correo { get; set; }

        /// <summary>
        /// Clave
        /// </summary>
        ///
        [BsonElement(elementName: "clave")]
        public string Clave { get; set; }

        /// <summary>
        /// Rol
        /// </summary>
        ///
        [BsonElement(elementName: "rol")]
        public Rol Rol { get; set; }
    }
}