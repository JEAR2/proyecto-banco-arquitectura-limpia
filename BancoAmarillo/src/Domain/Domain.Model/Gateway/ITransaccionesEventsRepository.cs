using Domain.Model.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Gateway
{
    public interface ITransaccionesEventsRepository
    {
        Task NotificarTransaccionRealizada(string correo, Transaccion transaccion);
    }
}
