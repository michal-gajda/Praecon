namespace Praecon.WinUI.ValueConverters;

using System.Globalization;
using System.Windows.Data;

public sealed class BoolToCheckBoxConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => this.Default(value);

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => this.Default(value);

    private object Default(object? value) => value is bool boolValue
        ? boolValue
        : Binding.DoNothing;
}
