using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TestEremex;

public partial class GridUserControl : MyControl
{
    public GridUserControl()
    {
        InitializeComponent();
        this.ComponentName = "Компонента2";
        Id = MainWindow.Id2;
    }
}