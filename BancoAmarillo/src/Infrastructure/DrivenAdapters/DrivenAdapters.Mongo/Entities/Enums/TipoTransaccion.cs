using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrivenAdapters.Mongo.Entities.Enums
{
    /// <summary>
    /// Enum TipoTransaccion
    /// </summary>
    public enum TipoTransaccionEntity
    {
        /// <summary>
        /// Tipo Transferencia
        /// </summary>
        TRANSFERENCIA = 1,

        /// <summary>
        /// Tipo Consignación
        /// </summary>
        CONSIGNACION = 2,

        /// <summary>
        /// Tipo Retiro
        /// </summary>
        RETIRO = 3,
    }
}