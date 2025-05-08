using System.Data.OleDb;
using System;
using System.Windows;

namespace Спорт_тесты
{
    public partial class регистрация : Window
    {
        public регистрация()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameBox.Text.Trim();
            string loginl = loginBox.Text.Trim();
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            // Валидация полей
            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(loginl) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            string dataFolder = System.IO.Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
    "SportTestsDB");
            string dbPath = System.IO.Path.Combine(dataFolder, "db.accdb");
            string connString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";
            // Составляем строку подключения к Access (ACE). 
            // Файл БД должен лежать рядом с exe и иметь Build Action=Content, Copy if newer.
            /*string dbPath = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "db.accdb");
            string connString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";*/

            try
            {
                using (var conn = new OleDbConnection(connString))
                {
                    conn.Open();

                    // Сначала проверим, нет ли уже такого логина/почты
                    using (var checkCmd = new OleDbCommand(
                        @"SELECT COUNT(*) 
                          FROM Пользователи 
                          WHERE [имя_пользователя] = ?", conn))
                    {
                        checkCmd.Parameters.AddWithValue("?", loginl);
                        int exists = (int)checkCmd.ExecuteScalar();
                        if (exists > 0)
                        {
                            MessageBox.Show("Пользователь с таким логином уже существует",
                                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }

                    // Вставляем нового пользователя
                    using (var insertCmd = new OleDbCommand(
                        @"INSERT INTO Пользователи ([имя], [имя_пользователя], [пароль]) 
                          VALUES (?, ?, ?)", conn))
                    {
                        insertCmd.Parameters.AddWithValue("?", name);
                        insertCmd.Parameters.AddWithValue("?", loginl);
                        insertCmd.Parameters.AddWithValue("?", password);

                        int rows = insertCmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Регистрация прошла успешно!", "Успех",
                                            MessageBoxButton.OK, MessageBoxImage.Information);

                            авторизация auth = new авторизация();
                            auth.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Не удалось добавить пользователя", "Ошибка",
                                            MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при работе с базой данных:\n{ex.Message}",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            // Переход на форму авторизации
            авторизация authWindow = new авторизация();
            authWindow.Show();
            this.Close();
        }
    }
}
