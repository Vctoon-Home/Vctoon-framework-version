using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using Abp.Localization.Avalonia;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VctoonClient.Validations;
using Volo.Abp.Localization;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;

namespace VctoonClient;

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
        // var cultureValidationAttribute = SetCultureErrorMessage(validationAttribute);
        var result = _validationResultProvider.GetCultureMessageValidationAttribute(validationAttribute)?
            .GetValidationResult(validatingObject, validationContext);

        return result;
    }
}