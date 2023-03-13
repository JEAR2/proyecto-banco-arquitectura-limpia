using Domain.Model.Entidades.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrivenAdapters.ServiceBus.Entities
{
    /// <summary>
    /// TransaccionEntityServiceBus clase
    /// </summary>
    public class TransaccionEntityServiceBus
    {
            /// <summary>
            /// Id de la Transacción
            /// </summary>
            public string Id { get; set; }

            /// <summary>
            /// IdEmisor
            /// </summary>
            public string IdCuentaEmisora { get; set; }

            /// <summary>
            /// IdReceptor
            /// </summary>
            public string IdCuentaReceptora { get; set; }

            /// <summary>
            /// Tipo de Transacción realizada
            /// </summary>
            public TipoTransaccion TipoTransaccion { get; set; }

            /// <summary>
            /// valor de la transacción
            /// </summary>
            public float Valor { get; set; }

            /// <summary>
            /// Fecha del movimiento
            /// </summary>
            public DateTime FechaMovimiento { get; set; }

            /// <summary>
            /// Tipo de movimiento realizado
            /// </summary>
            public TipoMovimiento TipoMovimiento { get; set; }

            /// <summary>
            /// Correo
            /// </summary>
            public string Correo { get; set; }
        }
    }

