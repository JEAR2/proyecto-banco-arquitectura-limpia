using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.Entidades;
using Domain.Model.Entidades.Enums;
namespace Domain.UseCase.Tests.Builders
{
    public class ClienteBuilder
    {
        private Cliente _cliente;

        public ClienteBuilder()
        {
            _cliente = new Cliente();
        }

        public ClienteBuilder WithId(string id)
        {
            _cliente.Id = id;
            return this;
        }

        public ClienteBuilder WithTipoIdentificacion(TipoIdentificacion tipoIdentificacion)
        {
            _cliente.TipoIdentificacion = tipoIdentificacion;
            return this;
        }

        public ClienteBuilder WithNumeroIdentificacion(string numeroIdentificacion)
        {
            _cliente.NumeroIdentificacion = numeroIdentificacion;
            return this;
        }

        public ClienteBuilder WithNombre(string nombre)
        {
            _cliente.Nombre = nombre;
            return this;
        }

        public ClienteBuilder WithApellido(string apellido)
        {
            _cliente.Apellido = apellido;
            return this;
        }

        public ClienteBuilder WithCorreo(string correo)
        {
            _cliente.Correo = correo;
            return this;
        }

        public ClienteBuilder WithFechaNacimiento(DateTime fechaNacimiento)
        {
            _cliente.FechaNacimiento = fechaNacimiento;
            return this;
        }

        public ClienteBuilder WithFechaCreacion(DateTime fechaCreacion)
        {
            _cliente.FechaCreacion = fechaCreacion;
            return this;
        }

        public ClienteBuilder WithFechaModificacion(DateTime fechaModificacion)
        {
            _cliente.FechaModificacion = fechaModificacion;
            return this;
        }

        public ClienteBuilder WithUsuarioModificacion(string usuarioModificacion)
        {
            _cliente.UsuarioModificacion = usuarioModificacion;
            return this;
        }

        public ClienteBuilder WithEstado(EstadoCliente estado)
        {
            _cliente.Estado = estado;
            return this;
        }

        public ClienteBuilder WithCuentas(List<Cuenta> cuentas)
        {
            _cliente.Cuentas = cuentas;
            return this;
        }

        public Cliente Build()
        {
            return _cliente;
        }
    }

}
