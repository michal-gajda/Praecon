namespace Praecon.WinUI.ValueConverters;

using System.Globalization;
using System.Windows.Data;

public sealed class BoolToCheckBoxConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue;
        }

        return Binding.DoNothing;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue;
        }

        return Binding.DoNothing;
    }
}
