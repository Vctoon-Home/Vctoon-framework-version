using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Abp.Localization.Avalonia;
using Avalonia.Data;
using Avalonia.Data.Core.Plugins;
using Microsoft.Extensions.Localization;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;

namespace VctoonClient.Validations;

/// <summary>
/// copy code by DataAnnotationsValidationPlugin
/// </summary>
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
            var runtimeProperty = target.GetType().GetRuntimeProperty(memberName);
            nullable = runtimeProperty != null
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
        return new Accessor(reference, name, inner);
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
            _context = new ValidationContext(target);
            _context.MemberName = name;

            _validationLocalizer = App.Services.GetRequiredService<ILocalizationManager>()
                .GetResource<AbpValidationResource>();
            _objectValidator = App.Services.GetRequiredService<IObjectValidator>();
        }

        protected override void InnerValueChanged(object? value)
        {
            if (_context == null)
                return;
            var errors = _objectValidator
                .GetErrorsAsync(_context.ObjectInstance, _context.MemberName).GetAwaiter()
                .GetResult();

            if (errors.IsNullOrEmpty() || !errors.Any(e => e.MemberNames.Any(m => m.Contains(_context.MemberName!))))
                base.InnerValueChanged(value);
            else
                base.InnerValueChanged(new BindingNotification(
                    CreateException(errors), BindingErrorType.DataValidationError, value));
        }


        private static Exception CreateException(IList<ValidationResult> errors)
        {
            var res = errors.Count == 1
                ? (Exception) new DataValidationException(errors[0].ErrorMessage)
                : (Exception) new AggregateException(
                    errors.Select(
                        (Func<ValidationResult, DataValidationException>) (x =>
                            new DataValidationException((object) x.ErrorMessage))));

            return res;
        }
    }
}