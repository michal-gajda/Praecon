namespace Praecon.WinUI.Views;

using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

using CommunityToolkit.Mvvm.DependencyInjection;

using Praecon.WinUI.ViewModels;

public partial class UpdateArticleView : Window
{
    private const int GWL_EXSTYLE = -20;
    private const int GWL_STYLE = -16;
    private const int SWP_FRAMECHANGED = 0x0020;
    private const int SWP_NOACTIVATE = 0x0010;
    private const int SWP_NOMOVE = 0x0002;
    private const int SWP_NOSIZE = 0x0001;
    private const int SWP_NOZORDER = 0x0004;
    private const uint WM_SETICON = 0x0080;
    private const int WS_EX_DLGMODALFRAME = 0x0001;
    private const int WS_MAXIMIZEBOX = 0x10000;
    private const int WS_MINIMIZEBOX = 0x20000;

    public UpdateArticleViewModel ViewModel => (UpdateArticleViewModel)this.DataContext;

    public UpdateArticleView()
    {
        this.InitializeComponent();

        this.SourceInitialized += (sender, e) =>
        {
            if (sender is Window window)
            {
                IntPtr handle = new WindowInteropHelper(window).Handle;
                int value = GetWindowLong(handle, GWL_STYLE);
                _ = SetWindowLong(handle, GWL_STYLE, value & ~WS_MAXIMIZEBOX & ~WS_MINIMIZEBOX);

                int exStyle = GetWindowLong(handle, GWL_EXSTYLE);
                _ = SetWindowLong(handle, GWL_EXSTYLE, exStyle | WS_EX_DLGMODALFRAME);

                _ = SendMessage(handle, WM_SETICON, IntPtr.Zero, IntPtr.Zero);
                _ = SendMessage(handle, WM_SETICON, new IntPtr(1), IntPtr.Zero);

                _ = SetWindowPos(handle, IntPtr.Zero, x: 0, y: 0, cx: 0, cy: 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_NOACTIVATE | SWP_FRAMECHANGED);
            }
        };

        this.DataContext = Ioc.Default.GetService<UpdateArticleViewModel>();
    }

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SendMessage(IntPtr hWnd, uint unMsg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);
}
