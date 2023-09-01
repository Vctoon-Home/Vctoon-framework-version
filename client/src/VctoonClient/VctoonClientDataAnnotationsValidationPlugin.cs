using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Avalonia.Data;
using Avalonia.Data.Core.Plugins;

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
        nullable = (object) runtimeProperty != null ? new bool?(runtimeProperty.GetCustomAttributes<ValidationAttribute>().Any<ValidationAttribute>()) : new bool?();
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

      public Accessor(WeakReference<object?> reference, string name, IPropertyAccessor inner)
        : base(inner)
      {
        object target;
        if (!reference.TryGetTarget(out target))
          return;
        this._context = new ValidationContext(target);
        this._context.MemberName = name;
      }

      protected override void InnerValueChanged(object? value)
      {
        if (this._context == null)
          return;
        List<ValidationResult> errors = new List<ValidationResult>();
        if (System.ComponentModel.DataAnnotations.Validator.TryValidateProperty(value, this._context, (ICollection<ValidationResult>) errors))
          base.InnerValueChanged(value);
        else
          base.InnerValueChanged((object) new BindingNotification(Accessor.CreateException((IList<ValidationResult>) errors), BindingErrorType.DataValidationError, value));
      }

      private static Exception CreateException(IList<ValidationResult> errors) => errors.Count == 1 ? (Exception) new DataValidationException((object) errors[0].ErrorMessage) : (Exception) new AggregateException((IEnumerable<Exception>) errors.Select<ValidationResult, DataValidationException>((Func<ValidationResult, DataValidationException>) (x => new DataValidationException((object) x.ErrorMessage))));
    }
}