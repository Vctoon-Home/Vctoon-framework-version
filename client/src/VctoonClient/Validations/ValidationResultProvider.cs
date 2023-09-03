using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Abp.Localization.Avalonia;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.Localization;
using Volo.Abp.Validation.Localization;

namespace VctoonClient.Validations;

public class ValidationResultProvider : ITransientDependency
{
    private readonly ILocalizationManager _localizationManager;
    private readonly IOptions<AbpLocalizationOptions> _localizationOptions;

    public ValidationResultProvider(ILocalizationManager localizationManager,
        IOptions<AbpLocalizationOptions> localizationOptions)
    {
        _localizationManager = localizationManager;
        _localizationOptions = localizationOptions;
    }


    public ValidationAttribute? GetCultureMessageValidationAttribute(ValidationAttribute validationAttribute)
    {
        // 反射获取 validationAttribute的ErrorMessageString
        var errorMessageString = validationAttribute.GetType()
            .GetProperty("ErrorMessageString",
                BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.NonPublic)
            ?.GetValue(validationAttribute) as string;
        if (errorMessageString.IsNullOrEmpty())
            return null;

        var localizer = GetStringLocalizer(validationAttribute);

        var cultureErrorMessage = GetCultureErrorMessage(errorMessageString!, localizer);

// 获取FieldInfo
        FieldInfo fieldInfo = typeof(ValidationAttribute).GetField("_errorMessageResourceAccessor",
            BindingFlags.NonPublic | BindingFlags.Instance);

        Func<string> cultureAccessor = () => cultureErrorMessage;

        fieldInfo.SetValue(validationAttribute, cultureAccessor);

        return validationAttribute;
    }

    private IStringLocalizer GetStringLocalizer(ValidationAttribute validationAttribute)
    {
        IStringLocalizer localizer = null!;


        if (!validationAttribute.ErrorMessageResourceName.IsNullOrEmpty())
        {
            localizer = _localizationManager.GetResource(validationAttribute.ErrorMessageResourceName!);
        }
        else
        {
            localizer = _localizationManager.GetResource(validationAttribute.ErrorMessageResourceType ??
                                                         _localizationOptions?.Value?.DefaultResourceType ??
                                                         typeof(AbpValidationResource));
        }

        return localizer;
    }

    private string GetCultureErrorMessage(string originErrorString, IStringLocalizer localizer)
    {
        if (originErrorString.IsNullOrEmpty())
            return originErrorString;

        var str = localizer[originErrorString];

        return str;
    }
}