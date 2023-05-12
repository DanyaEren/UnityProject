using UnityEngine;
using System.Data;
using System.Data.SQLite;

public class SQLiteDemo : MonoBehaviour
{
    void Start()
    {
        string connectionString = "Data Source=/Assets/Plugins/EscapeQuest.db;Version=3;";
        SQLiteConnection connection = new SQLiteConnection(connectionString);
        connection.Open();

        // Выполнение запросов к базе данных
        SQLiteCommand command = new SQLiteCommand("SELECT * FROM User", connection);
        SQLiteDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            string value = reader.GetString(0);
            Debug.Log(value);
        }

        connection.Close();
    }
}
