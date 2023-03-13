using Domain.Model.Entidades;
using EntryPoints.Grpc.Dtos.Protos.Cliente;
using EntryPoints.Grpc.Validaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EntryPoints.ReactWeb.Tests.Validaciones
{
    public class ValidacionClienteTest
    {
        private readonly ValidacionCliente _clienteValidacion;

        public ValidacionClienteTest()
        {
            _clienteValidacion = new ValidacionCliente();
        }

        [Fact]
        public void Validacion_Nombre_Apellido_Fecha_Nacimiento_Cliente_request_Retorna_Error()
        {
            var cliente = new ClienteRequest()
            {
                Nombre = "c",
                Apellido = "a",
                FechaNacimiento = "21/01/2009",
            };

            var result = _clienteValidacion.Validate(cliente);

            Assert.NotNull(result);
            Assert.False(result.IsValid);
        }
    }
}