using AutoMapper;
using Domain.Model.Entidades;
using Domain.Model.Gateway;
using DrivenAdapters.Mongo.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrivenAdapters.Mongo.Adaptadores
{
    /// <summary>
    /// Transacción Adapter
    /// </summary>
    public class TransaccionAdapter : ITransaccionesRepository
    {
        private readonly IContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        public TransaccionAdapter(IContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// crear
        /// </summary>
        /// <param name="transaccion"></param>
        /// <returns></returns>
        public async Task<Transaccion> CrearTransaccionAsync(Transaccion transaccion)
        {
            var nuevaTransaccion = _mapper.Map<TransaccionEntity>(transaccion);
            await _context.Transacciones.InsertOneAsync(nuevaTransaccion);

            return _mapper.Map<Transaccion>(nuevaTransaccion);
        }

        /// <summary>
        /// obtener
        /// </summary>
        /// <param name="transaccionId"></param>
        /// <returns></returns>
        public async Task<Transaccion> ObtenerTransaccionPorIdAsync(string transaccionId)
        {
            var cursor = await _context.Transacciones.FindAsync(transaccion => transaccion.Id == transaccionId);
            var transaccion = cursor.FirstOrDefault();
            return _mapper.Map<Transaccion>(transaccion);
        }
    }
}
