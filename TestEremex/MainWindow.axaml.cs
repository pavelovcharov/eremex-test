using Avalonia.Controls;
using Avalonia.Interactivity;

namespace TestEremex;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        (DataContext as MyViewModel)?.Generate(); 
    }

    private void Button_OnClick2(object? sender, RoutedEventArgs e)
    {
        (DataContext as MyViewModel)?.UpdateGraph();
    }
}