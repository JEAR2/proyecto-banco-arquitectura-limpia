using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrivenAdapters.Mongo.Entities.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DrivenAdapters.Mongo.Entities
{
    public class TransaccionEntity
    {
        /// <summary>
        /// Id de la Transacción
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// IdEmisor
        /// </summary>
        [BsonElement(elementName: "idCuentaEmisora")]
        public string IdCuentaEmisora { get; set; }

        /// <summary>
        /// IdReceptor
        /// </summary>
        [BsonElement(elementName: "idCuentaReceptora")]
        public string IdCuentaReceptora { get; set; }

        /// <summary>
        /// Tipo de Transacción realizada
        /// </summary>
        [BsonElement(elementName:"tipoTransaccion")]
        public TipoTransaccionEntity TipoTransaccion { get; set; }

        /// <summary>
        /// valor de la transacción
        /// </summary>
        [BsonElement(elementName: "valor")]
        public float Valor { get; set; }

        /// <summary>
        /// Fecha del movimiento
        /// </summary>
        [BsonElement(elementName: "fechaMovimiento")]
        public DateTime FechaMovimiento { get; set; }

        /// <summary>
        /// Tipo de movimiento realizado
        /// </summary>
        [BsonElement(elementName: "tipoMovimiento")]
        public TipoMovimientoEntity TipoMovimiento { get; set; }

    }


}
