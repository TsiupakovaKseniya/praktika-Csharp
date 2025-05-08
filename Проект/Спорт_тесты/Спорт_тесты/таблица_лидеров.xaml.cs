using System;
using System.Data;
using System.Data.OleDb;
using System.Windows;

namespace Спорт_тесты
{
    /// <summary>
    /// Логика взаимодействия для таблица_лидеров.xaml
    /// </summary>
    public partial class таблица_лидеров : Window
    {
        private readonly int _testId;
        private readonly Пользователь currentUser;

        public таблица_лидеров(int testId)
        {
            InitializeComponent();
            _testId = testId;
            this.currentUser = ((App)Application.Current).CurrentUser;
            LoadLeaderboard();
        }

        private void LoadLeaderboard()
        {
            string dataFolder = System.IO.Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
    "SportTestsDB");
            string dbPath = System.IO.Path.Combine(dataFolder, "db.accdb");
            string connString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";

            try
            {
                using (var conn = new OleDbConnection(connString))
                {
                    conn.Open();

                    // Ваш обновлённый запрос
                    string sql = @"SELECT TOP 10
                    п.имя_пользователя AS Логин,
                    р.баллы AS Балл
                    FROM 
                    результаты р
                    INNER JOIN пользователи п ON р.идентификатор_пользователя = п.идентификатор
                    WHERE 
                    р.идентификатор_пользователя = ?
                    AND
                    р.идентификатор_теста = ?
                    ORDER BY 
                    р.идентификатор DESC
                    ";

                    var cmd = new OleDbCommand(sql, conn);
                    cmd.Parameters.AddWithValue("?", currentUser.Id); // _userId — идентификатор пользователя
                    cmd.Parameters.AddWithValue("?", _testId); // _userId — идентификатор пользователя

                    var adapter = new OleDbDataAdapter(cmd);
                    var dt = new DataTable();
                    adapter.Fill(dt);

                    // Добавляем колонку с нумерацией
                    dt.Columns.Add("Рейтинг", typeof(int));
                    dt.Columns.Add("Баллы", typeof(string));

                    // Форматируем баллы (например, добавляем %)
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["Рейтинг"] = i + 1;
                        dt.Rows[i]["Баллы"] = $"{dt.Rows[i]["Балл"]}%";
                    }

                    dt.Columns.Remove("Балл");

                    // Перемещаем колонку "Рейтинг" в начало (опционально)
                    dt.Columns["Рейтинг"].SetOrdinal(0);
                    dt.Columns["Баллы"].SetOrdinal(2);

                    LeaderboardGrid.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки таблицы результатов:\n{ex.Message}",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            главная mainWindow = new главная();
            mainWindow.Show();
            this.Close();
        }
    }
}
