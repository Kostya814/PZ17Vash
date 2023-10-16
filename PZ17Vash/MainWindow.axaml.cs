using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using HarfBuzzSharp;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySqlConnector;
using PZ17Vash.Modelss;
using Tmds.DBus.Protocol;
using Address = PZ17Vash.Modelss.Address;


namespace PZ17Vash;

public partial class MainWindow : Window
{
    public List<User> data { get; set; }
    public List<Streets> StreetsList { get; set; }
    public List<Address> AddressList;

    private MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
    {
        Server = "localhost",
        UserID = "root",
        Password = "123456",
        Database = "test",
    };

    public MainWindow()
    {
        InitializeComponent();
        updateGrid();


    }

    public void updateGrid()
    {
        User user;
        data = new List<User>();

        using (var sqlConnection = new MySqlConnection(builder.ConnectionString))
        {
            sqlConnection.Open();

            using (var cmd = sqlConnection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM user";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    user = new User();
                    user.ID = reader.GetInt16("ID");
                    user.Name = reader.GetString(1);
                    user.Age = reader.GetInt16(4);
                    data.Add(user);
                }
            }


            sqlConnection.Close();
        }

        gridforbd.ItemsSource = data;
        filter.ItemsSource = data.Select(u => u.Name).Distinct().ToList();
    }

    private void OnClickAddress(object? sender, RoutedEventArgs e)
    {
        AddressList = new List<Address>();
        using (var sqlConnections = new MySqlConnection(builder.ConnectionString))
        {
            sqlConnections.Open();
            using (var cmd = sqlConnections.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM address INNER JOIN streets ON address.Street = streets.ID";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    AddressList.Add(new Address(reader.GetInt16("ID"), reader.GetString("NameCity"),
                        reader.GetString("NameStreet")));
                }
            }

            sqlConnections.Close();
        }

        ThirdWindow secondWindow = new ThirdWindow(AddressList);
        secondWindow.ShowDialog(this);
    }

    private void OnCLickStreets(object? sender, RoutedEventArgs e)
    {
        StreetsList = new List<Streets>();
        using (var sqlConnections = new MySqlConnection(builder.ConnectionString))
        {
            sqlConnections.Open();
            using (var cmd = sqlConnections.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM streets";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    StreetsList.Add(new Streets(reader.GetInt16("ID"), reader.GetString("NameStreet")));
                }
            }

            sqlConnections.Close();
        }



        SecondWindow secondWindow = new SecondWindow(StreetsList);
        secondWindow.ShowDialog(this);
    }

    private void TextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        var findbyname = data.Where(u => u.Name.Contains(TextBox.Text)).ToList();
        gridforbd.ItemsSource = findbyname;
        gridforbd.UpdateLayout();
    }

    private void Filter_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var findbyname = data.Where(u => u.Name.Contains(filter.SelectedValue.ToString())).ToList();
        gridforbd.ItemsSource = findbyname;
        gridforbd.UpdateLayout();
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        MainWindow main = new MainWindow();
        main.Show();

    }
    private void OnClick(object? sender, RoutedEventArgs e)
    {
        User insertUser = gridforbd.SelectedItem as User;
        using (var sqlConnections = new MySqlConnection(builder.ConnectionString))
        {
            sqlConnections.Open();
            using (var cmd = sqlConnections.CreateCommand())
            {
                User removeUser = gridforbd.SelectedItem as User;
                cmd.CommandText = "UPDATE user SET FirstName = @firstname,Age = @age WHERE ID = @id";
                cmd.Parameters.AddWithValue("@id", insertUser.ID);
                cmd.Parameters.AddWithValue("@firstname", insertUser.Name);
                cmd.Parameters.AddWithValue("@age", insertUser.Age);
                var rowsCount = cmd.ExecuteNonQuery();
            }
            updateGrid();
            sqlConnections.Close();
        }
    }

    private async void clickDelete(object? sender, RoutedEventArgs e)
    {
        var box = MessageBoxManager
            .GetMessageBoxStandard("Вы уверены", "Вы уверены что хотите удалить запись?",
                ButtonEnum.YesNo);
        var result =  await box.ShowAsync();
        if (result == ButtonResult.No) return;
        if (gridforbd.SelectedItem == null) return;
        using (var sqlConnections = new MySqlConnection(builder.ConnectionString))
        {
            sqlConnections.Open();
            using (var cmd = sqlConnections.CreateCommand())
            {
                User removeUser = gridforbd.SelectedItem as User;
                cmd.CommandText = "DELETE FROM user WHERE ID = @id ";
                cmd.Parameters.AddWithValue("@id", removeUser.ID);
                var rowsCount = cmd.ExecuteNonQuery();
            }
            updateGrid();
            sqlConnections.Close();
        }
    }
}
   