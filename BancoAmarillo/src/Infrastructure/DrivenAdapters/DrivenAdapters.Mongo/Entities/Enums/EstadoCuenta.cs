using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrivenAdapters.Mongo.Entities.Enums
{
    public enum EstadoCuenta
    {
        /// <summary>
        /// Estado cuenta Activa
        /// </summary>
        ACTIVA = 1,

        /// <summary>
        /// Estado cuenta Inactiva
        /// </summary>
        INACTIVA = 2,

        /// <summary>
        /// Estado cuenta cancelada
        /// </summary>
        CANCELADA = 3,
    }
}
