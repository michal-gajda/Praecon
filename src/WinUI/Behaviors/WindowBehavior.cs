namespace Praecon.WinUI.Behaviors;

using System.Windows;

public static class WindowBehavior
{
    private static readonly Type ownerType = typeof(WindowBehavior);

    [AttachedPropertyBrowsableForType(typeof(Window))]
    public static void SetClose(DependencyObject target, bool value)
    {
        target.SetValue(CloseProperty, value);
    }

    public static readonly DependencyProperty CloseProperty =
        DependencyProperty.RegisterAttached(
            "Close",
            typeof(bool),
            ownerType,
            new UIPropertyMetadata(false, (sender, e) =>
            {
                if (!(e.NewValue is bool) || !(bool)e.NewValue)
                {
                    return;
                }

                var window = sender as Window ?? Window.GetWindow(sender);

                window?.Close();
            }));
}
