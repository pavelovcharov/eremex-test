using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Eremex.AvaloniaUI.Controls.Docking;
using Newtonsoft.Json;

namespace TestEremex;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public static Guid Id1 = Guid.Parse("ABC60518-B090-4CA7-B1ED-8F9B8D968F10");
    public static Guid Id2 = Guid.Parse("78049F81-61C2-4EC3-9EAD-6ABC3CB711DF");
    public static Guid Id3 = Guid.Parse("CDE78AA5-67DD-4093-A5C1-52BB7ED30222");
    public static Guid Id4 = Guid.Parse("FC22AC15-B97E-4484-A910-6AB18C66946F");

    private ComponentManager _manager = new ComponentManager();

  

    private void Button_OnClick2(object? sender, RoutedEventArgs e)
    {
        RestoreLayout("Layout2.xml");
    }


    private void Button_OnClick1(object? sender, RoutedEventArgs e)
    {
        RestoreLayout("Layout1.xml");
    }

    private void Button_OnClick3(object? sender, RoutedEventArgs e)
    {
        RestoreLayout("Layout1.xml");
    }

    private void Button_OpenControlsAndSave1(object? sender, RoutedEventArgs e)
    {
        Dock(_manager.CreateVisualComponent(Id1));
        Dock(_manager.CreateVisualComponent(Id2));
        SaveLayout("Layout1.xml");
    }

    private void Button_OpenControlsAndSave2(object? sender, RoutedEventArgs e)
    {
        Dock(_manager.CreateVisualComponent(Id3));
        Dock(_manager.CreateVisualComponent(Id4));
        SaveLayout("Layout2.xml");
    }

    public void SaveLayout(string file)
    {
        using var stream = new MemoryStream();
        DockControl.SaveLayout(stream);
        stream.Position = 0;
        File.WriteAllBytes(file, stream.GetBuffer());
    }

    public void RestoreLayout(string file)
    {
        using var stream = File.OpenRead(file);



        DockControl.RestoreLayout(stream);
        //SerializationManager.Deserialize(DockControl, );


        DockControl.ClosedPanes.Clear();

        if (DockControl.Root != null)
            foreach (var pane in DockControl.GetItems().OfType<DockPane>())
            {
                if (pane.Content == null && !DockControl.ClosedPanes.Contains(pane))
                {
                    var idString = pane.Name;
                    if (Guid.TryParse(idString, out var id))
                    {
                        try
                        {
                            var control = _manager.CreateVisualComponent(id);


                            if (DockManager.GetDockItem(control) is DockPane oldPane)
                            {
                                oldPane.Content = null;
                            }
                            
                            // var visualParent = control.GetVisualParent();   //ПРИХОДИТСЯ ЧИСТИТЬ ПАРЕНТА ИНАЧЕ НЕ ВОССТАНАВЛИВАЕТСЯ И ПАДАЕТ
                            // if (visualParent is Grid grid)                  //ПРИХОДИТСЯ ЧИСТИТЬ ПАРЕНТА ИНАЧЕ НЕ ВОССТАНАВЛИВАЕТСЯ И ПАДАЕТ
                            //     grid.Children.Remove(control);              //ПРИХОДИТСЯ ЧИСТИТЬ ПАРЕНТА ИНАЧЕ НЕ ВОССТАНАВЛИВАЕТСЯ И ПАДАЕТ
                            // if (visualParent is ContentPresenter presenter) //МОЖЕТ Я КАК ТО НЕПРАВИЛЬНО УБИРАЮ КОМПОНЕНТУ... 
                            //     presenter.Content = null;                   //НО СТРАННО ЧТО ПАНЕЛЬ ПРОПАЛА А CONTROL ЕЩЕ НА НЕЙ ВСЁ ЕЩЕ ЛЕЖИТ

                            pane.Content = control;

                            var binding = new Binding
                            {
                                Source = control,
                                Path = nameof(control.ComponentName)
                            };

                            pane.Bind(DockPane.HeaderProperty, binding);
                        }
                        catch (Exception)
                        {
                            //TODO logs alerts

                            DockControl.Close(pane);
                        }
                    }
                }
            }
    }

    public void Dock(MyControl visualComponent)
    {
        var pane = new DockPane
        {
            //SerializationHelper.SetSerializationId(pane, visualComponent.Id.ToString());
            Name = visualComponent.Id.ToString()
        };

        var control = visualComponent;
        {
            if (DockManager.GetDockItem(control) is DockPane oldPane)
            {
                oldPane.Content = null;
            }
            
            // var visualParent = control.GetVisualParent();   //ЭТО ЕСЛИ ЗАКРЫЛИ КОМПОНЕНТУ НО МЫ ЕЁ СНОВА ОТКРЫВАЕМ.
            // if (visualParent is Grid grid)                  //ЭТО ЕСЛИ ЗАКРЫЛИ КОМПОНЕНТУ НО МЫ ЕЁ СНОВА ОТКРЫВАЕМ.
            //     grid.Children.Remove(control);              //ЭТО ЕСЛИ ЗАКРЫЛИ КОМПОНЕНТУ НО МЫ ЕЁ СНОВА ОТКРЫВАЕМ.
            // if (visualParent is ContentPresenter presenter) //ЭТО ЕСЛИ ЗАКРЫЛИ КОМПОНЕНТУ НО МЫ ЕЁ СНОВА ОТКРЫВАЕМ.
            //     presenter.Content = null;                   //ЭТО ЕСЛИ ЗАКРЫЛИ КОМПОНЕНТУ НО МЫ ЕЁ СНОВА ОТКРЫВАЕМ.

            pane.Content = control;

            var binding = new Binding
            {
                Source = visualComponent,
                Path = nameof(visualComponent.ComponentName)
            };

            pane.Bind(DockPane.HeaderProperty, binding);
        }
      
        if (DockControl.Root != null)
            DockControl.Dock(pane, DockControl.Root, DockType.Fill);
        else
            DockControl.Dock(pane);
    }
}

public class ComponentManager
{
    MyControl c1 = new TabUserControl() { Id = MainWindow.Id1};
    
    MyControl c2 = new TabUserControl() { Id = MainWindow.Id2};
    MyControl c3 = new GridUserControl() { Id = MainWindow.Id3};
    MyControl c4 = new GridUserControl() { Id = MainWindow.Id4};
    public MyControl CreateVisualComponent(Guid result)
    {
        if (result == MainWindow.Id1)
            return c1;
        
        if (result == MainWindow.Id2)
            return c2;
        if (result == MainWindow.Id3)
            return c3;
        if (result == MainWindow.Id4)
            return c4;
        
        
        return new MyControl() { ComponentName = result.ToString(), Id = result};
    }
}

public class MyControl : UserControl
{
    public string ComponentName { get; set; }
    public Guid Id { get; set; }
}