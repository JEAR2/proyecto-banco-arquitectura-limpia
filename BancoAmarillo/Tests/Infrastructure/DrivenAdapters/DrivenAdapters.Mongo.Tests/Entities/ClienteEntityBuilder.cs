using DrivenAdapters.Mongo.Entities;
using DrivenAdapters.Mongo.Entities.Enums;

namespace DrivenAdapters.Mongo.Tests.Entities
{
    public class ClienteEntityBuilder
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
        private List<CuentaEntity> cuentas;

        public ClienteEntityBuilder()
        {
        }

        public ClienteEntityBuilder WithId(string id)
        {
            this.id = id;
            return this;
        }

        public ClienteEntityBuilder WithTipoIdentificacion(TipoIdentificacion tipoIdentificacion)
        {
            this.tipoIdentificacion = tipoIdentificacion;
            return this;
        }

        public ClienteEntityBuilder WithNumeroIdentificacion(string numeroIdentificacion)
        {
            this.numeroIdentificacion = numeroIdentificacion;
            return this;
        }

        public ClienteEntityBuilder WithNombre(string nombre)
        {
            this.nombre = nombre;
            return this;
        }

        public ClienteEntityBuilder WithApellido(string apellido)
        {
            this.apellido = apellido;
            return this;
        }

        public ClienteEntityBuilder WithCorreo(string correo)
        {
            this.correo = correo;
            return this;
        }

        public ClienteEntityBuilder WithFechaNacimiento(DateTime fechaNacimiento)
        {
            this.fechaNacimiento = fechaNacimiento;
            return this;
        }

        public ClienteEntityBuilder WithFechaCreacion(DateTime fechaCreacion)
        {
            this.fechaCreacion = fechaCreacion;
            return this;
        }

        public ClienteEntityBuilder WithFechaModificacion(DateTime fechaModificacion)
        {
            this.fechaModificacion = fechaModificacion;
            return this;
        }

        public ClienteEntityBuilder WithUsuarioModificacion(string usuarioModificacion)
        {
            this.usuarioModificacion = usuarioModificacion;
            return this;
        }

        public ClienteEntityBuilder WithEstado(EstadoCliente estado)
        {
            this.estado = estado;
            return this;
        }

        public ClienteEntityBuilder WithCuentas(List<CuentaEntity> cuentas)
        {
            this.cuentas = cuentas;
            return this;
        }

        public ClienteEntity Build()
        {
            // Construir objeto Cliente con los valores asignados
            var cliente = new ClienteEntity()
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