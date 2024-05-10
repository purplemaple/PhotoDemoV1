using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using PhotoDemoV1.ViewModels;

namespace PhotoDemoV1.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        this.DataContext = new MainWindowLayoutViewModel();
    }

    //�ö�����
    private void TopWindow_Click(object sender, RoutedEventArgs e)
    {
        this.Topmost = !this.Topmost;
    }

    //��С������
    private void MinimizeWindow_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Minimized;
    }

    //���/���»�ԭ����
    private void MaximizeOrRestoreWindow_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    }

    //�رմ���
    private void CloseWindow_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    //�����϶�
    private void Border_PointerPressed(object sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            this.BeginMoveDrag(e);
        }
    }

    //˫�����/���»�ԭ
    private void Border_DoubleTapped(object sender, TappedEventArgs e)
    {
        if (this.VisualRoot is not null)
        {
            var window = (Window)this.VisualRoot;
            if (window.WindowState == WindowState.Maximized)
            {
                window.WindowState = WindowState.Normal;
            }
            else
            {
                window.WindowState = WindowState.Maximized;
            }
        }
    }
}