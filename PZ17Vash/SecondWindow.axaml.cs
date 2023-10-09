using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using PZ17Vash.Modelss;


namespace PZ17Vash;

public partial class SecondWindow : Window
{
    public SecondWindow() 
    {
        InitializeComponent();
    }
    public SecondWindow(List<Streets> data):this()
    {
        
        DataGrid.ItemsSource = data;

    }

    private void CloseWindow(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}