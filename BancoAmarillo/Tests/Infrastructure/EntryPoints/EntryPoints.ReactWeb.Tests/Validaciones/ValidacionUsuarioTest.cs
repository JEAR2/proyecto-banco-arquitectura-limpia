using EntryPoints.Grpc.Dtos.Protos;
using EntryPoints.Grpc.Dtos.Protos.Cliente;
using EntryPoints.Grpc.Validaciones;
using Xunit;

namespace EntryPoints.ReactWeb.Tests.Validaciones
{
    public class ValidacionUsuarioTest
    {
        private readonly ValidacionUsuario _usuarioValidacion;

        public ValidacionUsuarioTest()
        {
            _usuarioValidacion = new ValidacionUsuario();
        }

        [Fact]
        public void Validacion_Correo_Usuario_request_Retorna_Error()
        {
            var usuario = new UsuarioProto()
            {
                Correo = "example@correo.com"
            };

            var result = _usuarioValidacion.Validate(usuario);

            Assert.NotNull(result);
            Assert.False(result.IsValid);
        }
    }
}