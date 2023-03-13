using DrivenAdapters.Mongo.Entities.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace DrivenAdapters.Mongo.Entities
{
    /// <summary>
    /// Clase ClienteEntity
    /// </summary>
    public class ClienteEntity
    {
        /// <summary>
        /// Id de CLiente
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// Tipo de Identificación
        /// </summary>
        [BsonElement(elementName: "tipoIdentificacion")]
        public TipoIdentificacion TipoIdentificacion { get; set; }

        /// <summary>
        /// Número de Identificación
        /// </summary>
        [BsonElement(elementName: "numeroIdentificacion")]
        public string NumeroIdentificacion { get; set; }

        /// <summary>
        /// Nombre del Cliente
        /// </summary>
        [BsonElement(elementName: "nombre")]
        public string Nombre { get; set; }

        /// <summary>
        /// Apellido del Cliente
        /// </summary>
        [BsonElement(elementName: "apellido")]
        public string Apellido { get; set; }

        /// <summary>
        /// Correo del Cliente
        /// </summary>
        [BsonElement(elementName: "correo")]
        public string Correo { get; set; }

        /// <summary>
        /// Fecha de Nacimiento del Cliente
        /// </summary>
        [BsonElement(elementName: "fechaNacimiento")]
        public DateTime FechaNacimiento { get; set; }

        /// <summary>
        /// Fecha de Creación del Cliente
        /// </summary>
        [BsonElement(elementName: "fechaCreacion")]
        public DateTime FechaCreacion { get; set; }

        /// <summary>
        /// Fecha de Modificación del Cliente
        /// </summary>
        [BsonElement(elementName: "fechaModificacion")]
        public DateTime FechaModificacion { get; set; }

        /// <summary>
        /// Usuario que realiza la modificación
        /// </summary>
        [BsonElement(elementName: "usuarioModificacion")]
        public string UsuarioModificacion { get; set; }

        /// <summary>
        /// Estado del Cliente
        /// </summary>
        [BsonElement(elementName: "estado")]
        public EstadoCliente Estado { get; set; }

        /// <summary>
        /// Cuenta asignadas al Cliente
        /// </summary>
        [BsonElement(elementName: "cuentas")]
        public List<CuentaEntity> Cuentas { get; set; }
    }
}