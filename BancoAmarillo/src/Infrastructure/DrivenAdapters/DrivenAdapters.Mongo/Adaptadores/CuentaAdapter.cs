using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using credinet.comun.models.Credits;
using Domain.Model.Entidades;
using Domain.Model.Entidades.Enums;
using Domain.Model.Gateway;
using DrivenAdapters.Mongo.Entities;
using MongoDB.Driver;

namespace DrivenAdapters.Mongo.Adaptadores
{
    public class CuentaAdapter : ICuentaRepository
    {
        private readonly IContext _context;
        private readonly IMapper _mapper;
        private readonly FilterDefinitionBuilder<CuentaEntity> _filtro;

        public CuentaAdapter(IContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _filtro = Builders<CuentaEntity>.Filter;
        }

        public async Task<Cuenta> CrearCuentaAsync(Cuenta cuenta)
        {
            var nuevacuenta = _mapper.Map<CuentaEntity>(cuenta);
            await _context.Cuentas.InsertOneAsync(nuevacuenta);

            return _mapper.Map<Cuenta>(nuevacuenta);
        }

        public async Task<Cuenta> ActualizarCuentaAsync(Cuenta cuenta)
        {
            var nuevacuenta = _mapper.Map<CuentaEntity>(cuenta);
            await _context.Cuentas.ReplaceOneAsync(_filtro.Eq(u => u.Id, nuevacuenta.Id), nuevacuenta);

            return _mapper.Map<Cuenta>(nuevacuenta);
        }

        public async Task<Cuenta> ObtenerCuentaPorNumeroCuentaAsync(string numeroCuenta)
        {
            var cursor = await _context.Cuentas.FindAsync(_filtro.Eq(u => u.NumeroCuenta, numeroCuenta));
            var cuenta = cursor.FirstOrDefault();
            return _mapper.Map<Cuenta>(cuenta);
        }

        public async Task<List<Cuenta>> ObtenerCuentasPorIdClienteAsync(string idCliente)
        {
            var cursor = await _context.Cuentas.FindAsync(_filtro.Eq(cuenta => cuenta.IdCliente, idCliente));

            var cuentas = cursor.ToList();
            return _mapper.Map<List<Cuenta>>(cuentas);
        }

        public async Task<long> ObtenerCountCuentasTipoAsync(TipoCuenta tipo)
        {
            var count = await _context.Cuentas.CountDocumentsAsync(_filtro.Eq(cuenta => ((int)cuenta.TipoCuenta), ((int)tipo)));

            return count;
        }

        public async Task<Cuenta> ObtenerCuentaPorIdAsync(string cuentaId)
        {
            var cursor = await _context.Cuentas.FindAsync(_filtro.Eq(u => u.Id, cuentaId));
            var cuenta = cursor.FirstOrDefault();
            return _mapper.Map<Cuenta>(cuenta);
        }
    }
}