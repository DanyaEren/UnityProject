using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data.SQLite;
using static Unity.VisualScripting.Member;
using System.Data;
using UnityEngine.UIElements;

public class Exit : MonoBehaviour
{
    string connectionString;

    int topusers = 5;
    [SerializeField] private TMPro.TextMeshProUGUI[] users; // ������ ��� �������� ��������� ����� �������������
    [SerializeField] private TMPro.TextMeshProUGUI[] times; // ������ ��� �������� ��������� ����� �������
    private void Awake()
    {
        connectionString = "URI=file:" + Application.dataPath + "/DB/EscapeQuest.db";
    }

    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _topplayer;
    [SerializeField] private GameObject _crosshair;
    [SerializeField] private Camera _menuCamera;

    [SerializeField] MainMenuButton _menuButton;
    [SerializeField] Registration _registration;

    public double topMinute;
    public int topSecond;
    private int timeForDB = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _player.SetActive(false);
            _crosshair.SetActive(false);
            _topplayer.SetActive(true);
            _menuCamera.depth = 10;

            topMinute = _menuButton.minute;
            topSecond = _menuButton.secondint;

            Debug.Log(_registration.playerNow);
            Debug.Log("������ = " + topMinute);
            Debug.Log("������� = " + topSecond);
            timeForDB = Convert.ToInt32((topMinute * 60) + topSecond);

            string email = _registration.playerNow;

            // ���������� ���������� � ����� ������
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();

            // ���������, ���������� �� ������ � ����� email � ������� Achievements
            string sql = "SELECT time FROM Achievements WHERE email=@Email";
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", email);
            object result = command.ExecuteScalar();

            if (result == null) // ���� ������ email ��� ���, �� �������� ����� ������
            {
                sql = "INSERT INTO Achievements (email, time) VALUES (@Email, @Time)";
                command = new SQLiteCommand(sql, connection);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Time", timeForDB);
                command.ExecuteNonQuery();
            }
            else // ���� ������ � ����� email ��� ����������, �� �������� ��, ���� ����������
            {
                int time = Convert.ToInt32(result);

                if (timeForDB < time)
                {
                    sql = "UPDATE Achievements SET time=@Time WHERE email=@Email";
                    command = new SQLiteCommand(sql, connection);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Time", timeForDB);
                    command.ExecuteNonQuery();
                }
            }

            // �������� ���������� � ����� ������
            connection.Close();

            TopUser();
        }
    }

    public void TopUser()
    {
        SQLiteConnection connection = new SQLiteConnection(connectionString);
        connection.Open();

        // ���������� ������� � ���� ������
        string query = "SELECT email, time FROM Achievements ORDER BY time ASC";
        SQLiteCommand command = new SQLiteCommand(query, connection);
        SQLiteDataReader reader = command.ExecuteReader();

        // ����� ����������� ������� � ���������� ������������� � �������
        List<string> usersList = new List<string>();
        List<string> timesList = new List<string>();
        while (reader.Read())
        {
            usersList.Add(reader.GetString(reader.GetOrdinal("email")));
            timesList.Add(reader.GetInt32(reader.GetOrdinal("time")).ToString());
        }

        // �������������� ������� � �������
        string[] usersArray = usersList.ToArray();
        string[] timesArray = timesList.ToArray();

        // ���������� ��������� ����� �� �����
        for (int i = 0; i < users.Length; i++)
        {
            if (i < usersArray.Length)
            {
                users[i].text = usersArray[i];
            }
            if (i < timesArray.Length)
            {
                times[i].text = timesArray[i];
            }
        }

        // �������� ����������� � ���� ������
        connection.Close();
    }
}

 
