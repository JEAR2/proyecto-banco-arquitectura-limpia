using credinet.exception.middleware.models;
using EntryPoints.Grpc.Validaciones;
using FluentValidation.Results;
using Helpers.Commons.Exceptions;
using Xunit;

namespace EntryPoints.ReactWeb.Tests.Validaciones
{
    public class ValidacionTest
    {
        [Fact]
        public async Task Valida_Modelo_Valido()
        {
            var modeloValido = new ValidationResult();

            await Task.FromResult(modeloValido).ModeloValido();
        }

        [Fact]
        public async Task Valida_Modelo_No_Valido()
        {
            var errores = new List<ValidationFailure> { new ValidationFailure() };
            var modeloNoValido = new ValidationResult(errores);

            await Assert.ThrowsAsync<BusinessException>(() => Task.FromResult(modeloNoValido).ModeloValido());
        }
    }
}