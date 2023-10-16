using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using PZ17Vash.Modelss;

namespace PZ17Vash;

public partial class WindowData : Window
{
    private MySqlConnectionStringBuilder builder;
    public User NewUser { get; set; }
    public List<Streets> StreetsList { get; set; } = null;

    public WindowData()
    {
        InitializeComponent();
        NewUser = new User();
        StreetsList = new List<Streets>();
        DataContext = NewUser;
        
    }
    public WindowData(MySqlConnectionStringBuilder builder):this()
    {
        this.builder = builder;
        UpdateComboBox();
    }

    private void UpdateComboBox()
    {
        StreetsList.Clear();
        using (var connecction = new MySqlConnection(builder.ConnectionString))
        {
            connecction.Open();
            using (var cmd = connecction.CreateCommand() )
            {
                cmd.CommandText = "SELECT * FROM streets";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    StreetsList.Add(new Streets(reader.GetInt16("ID"), 
                        reader.GetString("NameStreet")));
                }
            }
            connecction.Close();
        }

        comboBox.ItemsSource = StreetsList;
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        if (comboBox.SelectedValue == null) return;
       
        using (var connection = new MySqlConnection(builder.ConnectionString))
        {
            connection.Open();
            using (var cmd = connection.CreateCommand())
            {
                
                cmd.CommandText = "INSERT INTO user (FirstName,Age,Address) VALUES (@name,@age,@street);";
                cmd.Parameters.AddWithValue("@name", NewUser.Name);
                cmd.Parameters.AddWithValue("@age", NewUser.Age);
                cmd.Parameters.AddWithValue("@street", (comboBox.SelectedItem as Streets).ID);
               

                var rowsCount = cmd.ExecuteNonQuery();
            }
            connection.Close();
        }
        
        Close();
    }

    private void Button_OnClick1(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}