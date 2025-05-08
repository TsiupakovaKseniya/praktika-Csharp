using System;
using System.Windows;
using System.Data.OleDb;
using System.IO;
using System.Text;

namespace Спорт_тесты
{
    public partial class авторизация : Window
    {
        public авторизация()
        {
            InitializeComponent();
        

        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string dataFolder = System.IO.Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
    "SportTestsDB");
            string dbPath = System.IO.Path.Combine(dataFolder, "db.accdb");
            string connStr = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";
            // Считываем введённые логин/пароль
            string username = UsernameBox.Text.Trim();
            string password = PasswordBox.Password;

            // Если поля пустые — сразу ошибка
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var conn = new OleDbConnection(connStr))
                {
                    conn.Open();

                    string sql = @"
                        SELECT COUNT(*) 
                        FROM Пользователи 
                        WHERE [имя_пользователя] = ? AND [пароль] = ?
                    ";
                  
                    using (var cmd = new OleDbCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("?", username);
                        cmd.Parameters.AddWithValue("?", password);

                        int count = (int)cmd.ExecuteScalar();

                        if (count > 0)
                        {
                            MessageBox.Show("Успешный вход!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                            var selectCmd = new OleDbCommand(
    "SELECT идентификатор, [имя], [имя_пользователя] FROM пользователи WHERE [имя_пользователя]=? AND [пароль]=?",
    conn);
                            selectCmd.Parameters.AddWithValue("?", username);
                            selectCmd.Parameters.AddWithValue("?", password);

                            using (var reader = selectCmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    Пользователь user = new Пользователь
                                    {
                                        Id = reader.GetInt32(0),
                                        Name = reader.GetString(1),
                                        Login = reader.GetString(2)
                                    };
                                    // Сохраняем в Application
                                    ((App)Application.Current).CurrentUser = user;
                                }
                            }

                            главная main = new главная();
                            main.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Неверный логин или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при подключении к БД:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            
            MessageBox.Show("Переход на форму регистрации", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Information);
          
            регистрация regWindow = new регистрация();
            regWindow.Show();
            this.Close();
        }
    }
}
