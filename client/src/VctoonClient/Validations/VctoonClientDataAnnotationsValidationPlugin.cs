using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Abp.Localization.Avalonia;
using Avalonia.Data;
using Avalonia.Data.Core.Plugins;
using Microsoft.Extensions.Localization;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;

namespace VctoonClient;

public class VctoonClientDataAnnotationsValidationPlugin : IDataValidationPlugin
{
    /// <inheritdoc />
    [RequiresUnreferencedCode("DataValidationPlugin might require unreferenced code.")]
    public bool Match(WeakReference<object?> reference, string memberName)
    {
        object target;
        reference.TryGetTarget(out target);
        bool? nullable;
        if (target == null)
        {
            nullable = new bool?();
        }
        else
        {
            PropertyInfo runtimeProperty = target.GetType().GetRuntimeProperty(memberName);
            nullable = (object) runtimeProperty != null
                ? new bool?(runtimeProperty.GetCustomAttributes<ValidationAttribute>().Any<ValidationAttribute>())
                : new bool?();
        }

        return nullable.GetValueOrDefault();
    }

    /// <inheritdoc />
    [RequiresUnreferencedCode("DataValidationPlugin might require unreferenced code.")]
    public IPropertyAccessor Start(
        WeakReference<object?> reference,
        string name,
        IPropertyAccessor inner)
    {
        return (IPropertyAccessor) new Accessor(reference, name, inner);
    }

    [RequiresUnreferencedCode("DataValidationPlugin might require unreferenced code.")]
    private sealed class Accessor : DataValidationBase
    {
        private readonly ValidationContext? _context;

        private IStringLocalizer _validationLocalizer;

        private IObjectValidator _objectValidator;

        public Accessor(WeakReference<object?> reference, string name, IPropertyAccessor inner)
            : base(inner)
        {
            object target;
            if (!reference.TryGetTarget(out target))
                return;
            this._context = new ValidationContext(target);
            this._context.MemberName = name;

            _validationLocalizer = App.Services.GetRequiredService<ILocalizationManager>()
                .GetResource<AbpValidationResource>();
            _objectValidator = App.Services.GetRequiredService<IObjectValidator>();
        }

        protected override void InnerValueChanged(object? value)
        {
            if (this._context == null)
                return;

            var bindingNotification = value as BindingNotification;

            List<ValidationResult> errors = _objectValidator
                .GetErrorsAsync(this._context.ObjectInstance, this._context.MemberName,true).GetAwaiter()
                .GetResult();

            // 本地化错误信息
            foreach (var validationResult in errors)
            {
                validationResult.ErrorMessage = _validationLocalizer[validationResult.ErrorMessage];
                validationResult.ErrorMessage = _validationLocalizer[validationResult.ErrorMessage];
            }

            if (errors.IsNullOrEmpty())
                base.InnerValueChanged(value);
            else
                base.InnerValueChanged((object) new BindingNotification(
                    Accessor.CreateException((IList<ValidationResult>) errors), BindingErrorType.DataValidationError,
                    value));


            // if (Validator.TryValidateProperty(bindingNotification.Value, this._context,
            //         (ICollection<ValidationResult>) errors))
            //     base.InnerValueChanged(value);
            // else
            //     base.InnerValueChanged((object) new BindingNotification(
            //         Accessor.CreateException((IList<ValidationResult>) errors), BindingErrorType.DataValidationError,
            //         value));
        }


        private static Exception CreateException(IList<ValidationResult> errors)
        {
            var res = errors.Count == 1
                ? (Exception) new DataValidationException((object) errors[0].ErrorMessage)
                : (Exception) new AggregateException(
                    (IEnumerable<Exception>) errors.Select<ValidationResult, DataValidationException>(
                        (Func<ValidationResult, DataValidationException>) (x =>
                            new DataValidationException((object) x.ErrorMessage))));

            return res;
        }
    }
}