using Domain.Model.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.UseCase.Transacciones
{
    public interface ITransaccionesUseCase
    {
        Task<Transaccion> RealizarTransaccion(Transaccion transaccion);
        Task<Transaccion> ObtenerTransaccion(string id);
    }
}
