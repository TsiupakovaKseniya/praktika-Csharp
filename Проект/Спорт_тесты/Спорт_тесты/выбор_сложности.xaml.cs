using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OleDb;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Спорт_тесты
{
    /// <summary>
    /// Логика взаимодействия для выбор_сложности.xaml
    /// </summary>
    public partial class выбор_сложности : Window, INotifyPropertyChanged
    {
        private int _easyMax;
        public int EasyMax
        {
            get => _easyMax;
            set { _easyMax = value; RaisePropertyChanged(); }
        }

        private int _mediumMax;
        public int MediumMax
        {
            get => _mediumMax;
            set { _mediumMax = value; RaisePropertyChanged(); }
        }

        private int _hardMax;
        public int HardMax
        {
            get => _hardMax;
            set { _hardMax = value; RaisePropertyChanged(); }
        }

        private string TestName{ get; set; }

        private void RaisePropertyChanged([CallerMemberName] string propName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        public event PropertyChangedEventHandler PropertyChanged;
        public выбор_сложности(string name)
        {
            InitializeComponent();
            DataContext = this;
            TestName = name;

            NameBox.Text = TestName;

            LoadMaxScores(TestName);
        }

        private void LoadMaxScores(string testName)
        {
            var user = ((App)Application.Current).CurrentUser;
            if (user == null) return;

            string dataFolder = System.IO.Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
    "SportTestsDB");
            string dbPath = System.IO.Path.Combine(dataFolder, "db.accdb");
            string connStr = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";

            try
            {
                using (var conn = new OleDbConnection(connStr))
                {
                    conn.Open();

                    foreach (var level in new[] { "Лёгкий", "Средний", "Сложный" })
                    {
                        try
                        {
                            // 1) Находим testId
                            using (var cmdT = new OleDbCommand("SELECT идентификатор FROM Тесты WHERE [вид_спорта]=? AND [сложность]=?", conn))
                            {
                                cmdT.Parameters.AddWithValue("?", testName);
                                cmdT.Parameters.AddWithValue("?", level);
                                var o = cmdT.ExecuteScalar();
                                if (o == null) continue;
                                int testId = Convert.ToInt32(o);

                                // 2) Берём MAX(баллы)
                                int maxScore = 0;
                                using (var cmdR = new OleDbCommand("SELECT MAX(баллы) FROM результаты WHERE идентификатор_пользователя=? AND идентификатор_теста=?", conn))
                                {
                                    cmdR.Parameters.AddWithValue("?", user.Id);
                                    cmdR.Parameters.AddWithValue("?", testId);
                                    var r = cmdR.ExecuteScalar();
                                    if (r != DBNull.Value && r != null)
                                        maxScore = Convert.ToInt32(r);
                                }

                                switch (level)
                                {
                                    case "Лёгкий": EasyMax = maxScore; break;
                                    case "Средний": MediumMax = maxScore; break;
                                    case "Сложный": HardMax = maxScore; break;
                                }
                            }
                        }
                        catch (Exception exLevel)
                        {
                            MessageBox.Show($"Ошибка уровня «{level}»: {exLevel.Message}", "БД", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке результатов: {ex.Message}", "БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
            private void Difficulty_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.Tag is string level)
            {
                // Тут ваша логика: переход к тесту с выбранным уровнем
                RouteToTest(level);
            }
        }

        private void RouteToTest(string level)
        {
            var testWindow = new тест(TestName, level);
            testWindow.Show();
            this.Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Логика возврата, например:
            var mainWindow = new главная();
            mainWindow.Show();
            this.Close();
        }
    }
}
