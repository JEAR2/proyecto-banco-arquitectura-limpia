using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.Entidades;
using Domain.Model.Entidades.Enums;

namespace Domain.UseCase.Tests.Builders
{
    public class TransaccionBuilder
    {
        private Transaccion _transaccion;

        public TransaccionBuilder()
        {
            _transaccion = new Transaccion();
        }

        public TransaccionBuilder WithId(string id)
        {
            _transaccion.Id = id;
            return this;
        }

        public TransaccionBuilder WithIdCuentaEmisora(string idCuentaEmisora)
        {
            _transaccion.IdCuentaEmisora = idCuentaEmisora;
            return this;
        }

        public TransaccionBuilder WithIdCuentaReceptora(string idCuentaReceptora)
        {
            _transaccion.IdCuentaReceptora = idCuentaReceptora;
            return this;
        }

        public TransaccionBuilder WithTipoTransaccion(TipoTransaccion tipoTransaccion)
        {
            _transaccion.TipoTransaccion = tipoTransaccion;
            return this;
        }

        public TransaccionBuilder WithValor(float valor)
        {
            _transaccion.Valor = valor;
            return this;
        }

        public TransaccionBuilder WithFechaMovimiento(DateTime fechaMovimiento)
        {
            _transaccion.FechaMovimiento = fechaMovimiento;
            return this;
        }

        public TransaccionBuilder WithTipoMovimiento(TipoMovimiento tipoMovimiento)
        {
            _transaccion.TipoMovimiento = tipoMovimiento;
            return this;
        }

        public Transaccion Build()
        {
            return _transaccion;
        }
    }

}
