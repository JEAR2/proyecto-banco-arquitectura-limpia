using Domain.Model.Entidades;
using Domain.Model.Entidades.Enums;

namespace Domain.Model.Tests.Builders
{
    public class ClienteBuilder
    {
        private string id;
        private TipoIdentificacion tipoIdentificacion;
        private string numeroIdentificacion;
        private string nombre;
        private string apellido;
        private string correo;
        private DateTime fechaNacimiento;
        private DateTime fechaCreacion;
        private DateTime fechaModificacion;
        private string usuarioModificacion;
        private EstadoCliente estado;
        private List<Cuenta> cuentas;

        public ClienteBuilder()
        {
        }

        public ClienteBuilder WithId(string id)
        {
            this.id = id;
            return this;
        }

        public ClienteBuilder WithTipoIdentificacion(TipoIdentificacion tipoIdentificacion)
        {
            this.tipoIdentificacion = tipoIdentificacion;
            return this;
        }

        public ClienteBuilder WithNumeroIdentificacion(string numeroIdentificacion)
        {
            this.numeroIdentificacion = numeroIdentificacion;
            return this;
        }

        public ClienteBuilder WithNombre(string nombre)
        {
            this.nombre = nombre;
            return this;
        }

        public ClienteBuilder WithApellido(string apellido)
        {
            this.apellido = apellido;
            return this;
        }

        public ClienteBuilder WithCorreo(string correo)
        {
            this.correo = correo;
            return this;
        }

        public ClienteBuilder WithFechaNacimiento(DateTime fechaNacimiento)
        {
            this.fechaNacimiento = fechaNacimiento;
            return this;
        }

        public ClienteBuilder WithFechaCreacion(DateTime fechaCreacion)
        {
            this.fechaCreacion = fechaCreacion;
            return this;
        }

        public ClienteBuilder WithFechaModificacion(DateTime fechaModificacion)
        {
            this.fechaModificacion = fechaModificacion;
            return this;
        }

        public ClienteBuilder WithUsuarioModificacion(string usuarioModificacion)
        {
            this.usuarioModificacion = usuarioModificacion;
            return this;
        }

        public ClienteBuilder WithEstado(EstadoCliente estado)
        {
            this.estado = estado;
            return this;
        }

        public ClienteBuilder WithCuentas(List<Cuenta> cuentas)
        {
            this.cuentas = cuentas;
            return this;
        }

        public Cliente Build()
        {
            // Construir objeto Cliente con los valores asignados
            var cliente = new Cliente()
            {
                Id = id,
                TipoIdentificacion = tipoIdentificacion,
                NumeroIdentificacion = numeroIdentificacion,
                Nombre = nombre,
                Apellido = apellido,
                Correo = correo,
                FechaNacimiento = fechaNacimiento,
                FechaCreacion = fechaCreacion,
                FechaModificacion = fechaModificacion,
                UsuarioModificacion = usuarioModificacion,
                Estado = estado,
                Cuentas = cuentas
            };

            return cliente;
        }
    }
}