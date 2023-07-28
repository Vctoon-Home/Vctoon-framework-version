using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;

namespace VctoonClient.Localizations;

public class LocalizationConverter : IMultiValueConverter
{
    public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
    {
        // 我们实际上并不关心第一个绑定的值，我们只是使用它来触发 MultiBinding 的更新
        return values[1];
    }

    public IList<object> ConvertBack(object value, IList<Type> targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}