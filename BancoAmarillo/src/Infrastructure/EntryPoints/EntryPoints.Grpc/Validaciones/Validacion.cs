using credinet.exception.middleware.models;
using FluentValidation.Results;
using Helpers.Commons.Exceptions;

namespace EntryPoints.Grpc.Validaciones
{
    public static class Validacion
    {
        public static async Task ModeloValido(this Task<ValidationResult> request)
        {
            var modelo = await request;
            if (!modelo.IsValid)
            {
                throw new BusinessException(String.Join(" - ",
                    modelo.Errors.Select(e => $"{e.ErrorMessage}")), (int)TipoExcepcionNegocio.ExceptionErrorEnModelo);
            }
        }
    }
}