using EntryPoints.Grpc.Dtos.Protos.Cliente;
using FluentValidation;

namespace EntryPoints.Grpc.Validaciones
{
    public class ValidacionCliente : AbstractValidator<ClienteRequest>
    {
        public ValidacionCliente()
        {
            RuleFor(c => c.Nombre).MinimumLength(2).WithMessage("El nombre debe contener mínimo 2 caracteres");
            RuleFor(c => c.Apellido).MinimumLength(2).WithMessage("El apellido debe contener mínimo 2 caracteres");
            RuleFor(c => c.FechaNacimiento).Must(f =>
            {
                return (DateTime.Today.Year - DateTime.Parse(f).Year) >= 18;
            }).WithMessage("El cliente debe ser mayor de edad [18 años]");
        }
    }
}