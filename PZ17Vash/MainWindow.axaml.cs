using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Avalonia.Controls;
using MySqlConnector;
using PZ17Vash.Modelss;


namespace PZ17Vash;

public partial class MainWindow : Window
{
    public List<User> data { get; set; }

    public MainWindow()
    {
        InitializeComponent();
        User user;
        data = new List<User>();
        string connectionStr = "server=10.10.1.24;user=user_01;password=user01pro;database=pro1;port=3306";
        using (var sqlConnection = new MySqlConnection(connectionStr))
        {
            sqlConnection.Open();

            using (var cmd = sqlConnection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM User";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    user = new User();
                    user.ID = reader.GetInt16("ID");
                    //user.ID = reader.GetInt16(0);
                    user.Name = reader.GetString(1);
                    user.Age = reader.GetInt16(4);
                    data.Add(user);
                    //asdas
                }
            }
            sqlConnection.Close();
        }

        gridforbd.ItemsSource = data;






    }
}