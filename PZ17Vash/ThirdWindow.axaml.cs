using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Tmds.DBus.Protocol;

namespace PZ17Vash;

public partial class ThirdWindow : Window
{
    public ThirdWindow()
    {
        InitializeComponent();
    }

    public ThirdWindow(List<Modelss.Address> addresses) : this()
    {
        DataGrid.ItemsSource = addresses;
    }

private void CloseWindow(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}