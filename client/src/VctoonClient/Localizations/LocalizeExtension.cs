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
    //
    // public override object ProvideValue(IServiceProvider serviceProvider)
    // {
    //     var manager = App.Services.GetService<LocalizationManager>();
    //
    //     // 强制触发 MultiBinding 的更新，即使我们实际上并不需要这个属性的值
    //     var dummyBinding = new Binding("CurrentCulture")
    //     {
    //         Mode = BindingMode.OneWay,
    //         Source = manager
    //     };
    //
    //     var localizationBinding = new Binding($"[{Text}]")
    //     {
    //         Mode = BindingMode.OneWay,
    //         Source = manager,
    //         StringFormat = StringFormat
    //     };
    //
    //     return new MultiBinding
    //     {
    //         Bindings =
    //         {
    //             dummyBinding,
    //             localizationBinding
    //         },
    //         Converter = new LocalizationConverter()
    //     };
    // }
}