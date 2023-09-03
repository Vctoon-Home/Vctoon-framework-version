using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace VctoonClient.Validations;

public class VctoonClientAttributeValidationResultProvider : IAttributeValidationResultProvider, ITransientDependency
{
    private readonly ValidationResultProvider _validationResultProvider;


    public VctoonClientAttributeValidationResultProvider(ValidationResultProvider validationResultProvider)
    {
        _validationResultProvider = validationResultProvider;
    }

    public ValidationResult? GetOrDefault(ValidationAttribute validationAttribute, object? validatingObject,
        ValidationContext validationContext)
    {
        var result = _validationResultProvider.GetCultureMessageValidationAttribute(validationAttribute)?
            .GetValidationResult(validatingObject, validationContext);

        return result;
    }
}