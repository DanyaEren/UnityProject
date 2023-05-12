using System;
using System.Collections;
using System.Data.SQLite;
using UnityEngine;

public class Registration : MonoBehaviour
{
    string connectionString;
    private void Awake()
    {
        connectionString = "URI=file:" + Application.dataPath + "/DB/EscapeQuest.db";
    }

    public string playerNow;

    private bool _errorlogin;
    private bool _erroremail;
    private bool _errorpassword;

    [Header("Registration")]
    [SerializeField] private GameObject _menuWindow;
    public GameObject registrationWindow;
    public GameObject audentificationWindow;
    [SerializeField] private TMPro.TMP_Text _errorText;
    [SerializeField] private GameObject _errorGameObject;

    [Header("RegForNewPlayer")]
    [SerializeField] private TMPro.TMP_InputField _newLogin;
    [SerializeField] private TMPro.TMP_InputField _newemail;
    [SerializeField] private TMPro.TMP_InputField _newpassword;

    [SerializeField] private TMPro.TMP_Text _newLoginText;
    [SerializeField] private TMPro.TMP_Text _newemailText;
    [SerializeField] private TMPro.TMP_Text _newpasswordText;


    [Header("ForOldPlayer")]
    [SerializeField] private TMPro.TMP_InputField _email;
    [SerializeField] private TMPro.TMP_InputField _password;

    [SerializeField] private TMPro.TMP_Text _emailText;
    [SerializeField] private TMPro.TMP_Text _passwordText;

    public void RegisUser()
    {

        //Проверка данных регистрации
        if (_newLogin.text.Length >= 4)
        {
            _newLoginText.color = Color.black;
            _errorlogin = false;
        }
        else
        {
            _newLoginText.color = Color.red;
            _errorlogin = true;
        }

        if ((_newemail.text.Length > 10) && ((_newemail.text.Contains("@mail.ru")) || (_newemail.text.Contains("@gmail.com"))))
        {
            _newemailText.color = Color.black;
            _erroremail = false;
        }
        else
        {
            _newemailText.color = Color.red;
            _erroremail = true;
        }

        if (_newpassword.text.Length >= 6)
        {
            _newpasswordText.color = Color.black;
            _errorpassword = false;
        }
        else
        {
            _newpasswordText.color = Color.red;
            _errorpassword = true;
        }

        if (_newemailText.color == Color.red || _newLoginText.color == Color.red || _newpasswordText.color == Color.red)
        {
            StartCoroutine(ShowGameObjectFor5Seconds(_errorGameObject));
            _errorText.text = "Ошибка подсвечена красным. логин от 4 символом, пароль от 6 символов. Формат почты mail или gmail";
            return;
        }
        else
        {
            Debug.Log(_newLogin.text);
            Debug.Log(_newemail.text);
            Debug.Log(_newpassword.text);

            if (!CheckExistingEmail(_newemail.text))
            {
                string query = "INSERT INTO User (Nickname, Email, Password) VALUES (@Nickname, @Email, @Password)";

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    // Создание объекта команды для выполнения запроса
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        // Добавление параметров в запрос
                        command.Parameters.AddWithValue("@Nickname", _newLogin.text);
                        command.Parameters.AddWithValue("@Email", _newemail.text);
                        command.Parameters.AddWithValue("@Password", _newpassword.text);

                        // Открытие подключения к базе данных
                        connection.Open();

                        // Выполнение запроса
                        int rowsAffected = command.ExecuteNonQuery();

                        // Закрытие подключения к базе данных
                        connection.Close();

                        if (rowsAffected > 0)
                        {
                            Debug.Log("Запись успешно добавлена в базу данных.");
                            registrationWindow.SetActive(false);
                            audentificationWindow.SetActive(true);
                        }
                        else
                        {
                            Debug.Log("Не удалось добавить запись в базу данных.");
                        }
                    }

                }
            }
            else
            {
                StartCoroutine(ShowGameObjectFor5Seconds(_errorGameObject));
                _errorText.text = "Такая почта занята";
                return;
            }
        }
    }

    public void Audification()
    {
        string email = _email.text;
        string password = _password.text;

        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            using (SQLiteCommand dbCommand = connection.CreateCommand())
            {
                dbCommand.CommandText = "SELECT email,password FROM User";
                using (SQLiteDataReader reader = dbCommand.ExecuteReader())
                {
                    bool authenticated = false;
                    while (reader.Read())
                    {
                        string userEmail = reader["email"].ToString();
                        string userPassword = reader["password"].ToString();
                        playerNow = reader["email"].ToString();

                        if (email == userEmail && password == userPassword)
                        {
                            authenticated = true;
                            break;
                        }
                    }

                    if (authenticated)
                    {
                        Debug.Log("Authentication successful.");
                        audentificationWindow.SetActive(false);
                        _menuWindow.SetActive(true);
                    }
                    else
                    {
                        Debug.Log("Authentication failed.");
                        StartCoroutine(ShowGameObjectFor5Seconds(_errorGameObject));
                        _errorText.text = "Неправильная почта или пароль";
                    }
                }
            }
        }
    }

    public bool CheckExistingEmail(string email)
    {
        string query = "SELECT COUNT(*) FROM User WHERE Email = @Email";

        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Email", email);

                connection.Open();
                int count = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();

                return count > 0;
            }
        }
    }

    public void IDontHaveAcc()
    {
        audentificationWindow.SetActive(false);
        registrationWindow.SetActive(true);
    }

    public void IhaveAccount()
    {
        registrationWindow.SetActive(false);
        audentificationWindow.SetActive(true);
    }

    public IEnumerator ShowGameObjectFor5Seconds(GameObject obj)
    {
        obj.SetActive(true);
        yield return new WaitForSeconds(5f);
        obj.SetActive(false);
    }
}
