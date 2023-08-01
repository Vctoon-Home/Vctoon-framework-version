using System;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;

namespace VctoonClient.Localizations;

public class LocalizeExtension : MarkupExtension
{
    public LocalizeExtension()
    {
        Text = String.Empty;
    }

    public LocalizeExtension(string text)
    {
        Text = text;
    }

    public string Text { get; set; }
    public string? StringFormat { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var manager = App.Services.GetService<LocalizationManager>();

        var binding = new Binding($"[{Text}]")
        {
            Mode = BindingMode.OneWay,
            Source = manager,
            StringFormat = StringFormat
        };

        return binding;
    }
}