using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.Validation;

namespace VctoonClient.Validations;

public class VctoonClientObjectValidator : ObjectValidator, ITransientDependency
{
    public VctoonClientObjectValidator(IOptions<AbpValidationOptions> options, IServiceScopeFactory serviceScopeFactory) :
        base(options, serviceScopeFactory)
    {
    }

    public async override Task<List<ValidationResult>> GetErrorsAsync(object validatingObject, string? name = null,
        bool allowNull = false)
    {
        if (validatingObject == null)
        {
            if (allowNull)
            {
                return new List<ValidationResult>(); //TODO: Returning an array would be more performent
            }
            else
            {
                return new List<ValidationResult>
                {
                    name == null
                        ? new ValidationResult("Given object is null!")
                        : new ValidationResult(name + " is null!", new[] {name})
                };
            }
        }

        var context = new ObjectValidationContext(validatingObject);

        using (var scope = ServiceScopeFactory.CreateScope())
        {
            foreach (var contributorType in Options.ObjectValidationContributors)
            {
                var contributor = (IObjectValidationContributor)
                    scope.ServiceProvider.GetRequiredService(contributorType);
                await contributor.AddErrorsAsync(context);
            }
        }

        return context.Errors;
    }
}