using EntryPoints.Grpc.Dtos.Protos;
using FluentValidation;
using System.Text.RegularExpressions;

namespace EntryPoints.Grpc.Validaciones
{
    public class ValidacionUsuario : AbstractValidator<UsuarioProto>
    {
        public ValidacionUsuario()
        {
            RuleFor(u => u.Correo).Must(e => Regex.IsMatch(e,
                @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                .WithMessage("Correo es invalido").NotNull();
            RuleFor(u => u.Clave).Must(e =>
            {
                var regex = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");

                return regex.IsMatch(e);
            }).WithMessage("La contraseña debe tener al menos 1 carácter especial, una mayúscula 1 numero");
        }
    }
}