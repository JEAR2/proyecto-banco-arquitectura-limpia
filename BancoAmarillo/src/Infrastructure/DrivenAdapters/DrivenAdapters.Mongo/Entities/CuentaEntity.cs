using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrivenAdapters.Mongo.Entities.Enums;
using Domain.Model.Entidades;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DrivenAdapters.Mongo.Entities
{
    public class CuentaEntity
    {
        /// <summary>
        /// Id Cuenta
        /// </summary>

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// Numero de Cuenta
        /// </summary>
        [BsonElement(elementName: "numeroCuenta")]
        public string NumeroCuenta { get; set; }

        /// <summary>
        /// Id del Cliente
        /// </summary>
        [BsonElement(elementName:"idCliente")]
        public string IdCliente { get; set; }

        /// <summary>
        /// Tipo de cuenta
        /// </summary>
        [BsonElement(elementName: "tipoCuenta")]
        public TipoCuenta TipoCuenta { get; set; }

        /// <summary>
        /// Estado de la cuenta
        /// </summary>
        [BsonElement(elementName: "estadoCuenta")]
        public EstadoCuenta EstadoCuenta { get; set; }

        /// <summary>
        /// Saldo de la cuenta
        /// </summary>
        [BsonElement(elementName: "saldo")]
        public float Saldo { get; set; }

        /// <summary>
        /// Saldo disponible
        /// </summary>
        [BsonElement(elementName: "saldoDisponible")]
        public float SaldoDisponible { get; set; }

        /// <summary>
        /// Gravamen al Movimiento Financiero
        /// </summary>
        [BsonElement(elementName: "gMF")]
        public bool GMF { get; set; }

        /// <summary>
        /// Fecha de Creación de la Cuenta
        /// </summary>
        [BsonElement(elementName: "fechaCreacion")]
        public DateTime FechaCreacion { get; set; }

        /// <summary>
        /// Fecha de Modificación de la Cuenta
        /// </summary>
        [BsonElement(elementName: "fechaModificacion")]
        public DateTime FechaModificacion { get; set; }

        /// <summary>
        /// Transacciones realizadas
        /// </summary>
        [BsonElement(elementName: "transacciones")]
        public List<TransaccionEntity> Transacciones { get; set; }
    }
}
