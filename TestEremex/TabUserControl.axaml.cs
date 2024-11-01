using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TestEremex;

public partial class TabUserControl : MyControl
{
    public TabUserControl()
    {
        InitializeComponent();
        this.ComponentName = "Компонента1";
        this.Id = MainWindow.Id1;
    }
}