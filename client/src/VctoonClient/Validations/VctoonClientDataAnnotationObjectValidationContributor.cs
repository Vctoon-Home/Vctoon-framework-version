using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.Extensions.Options;
using Volo.Abp.Validation;

namespace VctoonClient.Validations;

[Dependency(ReplaceServices = true)]
// [ExposeServices(typeof(IObjectValidationContributor))]
public class VctoonClientDataAnnotationObjectValidationContributor : DataAnnotationObjectValidationContributor,
    ITransientDependency
{
    public VctoonClientDataAnnotationObjectValidationContributor(IOptions<AbpValidationOptions> options,
        IServiceProvider serviceProvider) : base(options, serviceProvider)
    {
    }

    protected override void AddPropertyErrors(object validatingObject, PropertyDescriptor property,
        List<ValidationResult> errors)
    {
        // only add one error
        // if (!errors.IsNullOrEmpty()) return;

        var validationAttributes = property.Attributes.OfType<ValidationAttribute>().ToArray();
        if (validationAttributes.IsNullOrEmpty())
        {
            return;
        }

        var validationContext = new ValidationContext(validatingObject, ServiceProvider, null)
        {
            DisplayName = property.DisplayName,
            MemberName = property.Name
        };

        var attributeValidationResultProvider =
            ServiceProvider.GetRequiredService<IAttributeValidationResultProvider>();
        foreach (var attribute in validationAttributes)
        {
            var result = attributeValidationResultProvider.GetOrDefault(attribute, property.GetValue(validatingObject),
                validationContext);
            if (result != null)
            {
                errors.Add(result);

                // only add one error
                // return;
            }
        }
    }
}