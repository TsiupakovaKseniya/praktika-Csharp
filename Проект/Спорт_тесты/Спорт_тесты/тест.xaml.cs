using System;
using System.Windows;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;
using System.Runtime.CompilerServices;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Спорт_тесты
{
    public partial class тест : Window, INotifyPropertyChanged
    {
        private readonly Пользователь currentUser;
        private bool _answered;
        private string Category { get; set; }
        private string Level { get; set; }
        private int TestId { get; set; }

        private readonly ObservableCollection<вопрос> _questions = new ObservableCollection<вопрос>();
        private int _currentIndex;
        private DispatcherTimer _timer;
        private int _initialTime;

        // Текущий вопрос
        private вопрос _currentQuestion;
        public вопрос CurrentQuestion
        {
            get => _currentQuestion;
            set
            {
                if (_currentQuestion == value) return;
                _currentQuestion = value;
                OnPropertyChanged(nameof(CurrentQuestion));
                OnPropertyChanged(nameof(WeightText));
            }
        }

        public string WeightText
        {
            get
            {
                if (CurrentQuestion == null) return "";
                int w = CurrentQuestion.вес;
                string suffix = (w % 10 == 1 && w % 100 != 11) ? "балл"
                                : (w % 10 >= 2 && w % 10 <= 4
                                && (w % 100 < 10 || w % 100 >= 20)) ? "балла"
                                : "баллов";
                return $"{w} {suffix}";
            }
        }

// Оставшееся время
        private int _timeRemaining;
        public int TimeRemaining
        {
            get => _timeRemaining;
            set
            {
                if (_timeRemaining == value) return;
                _timeRemaining = value;
                OnPropertyChanged(nameof(TimeRemaining));
            }
        }

        // Сколько ответов угадано верно
        public int Score { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public ответ SelectedAnswer { get; set; }

       static string dataFolder = System.IO.Path.Combine(
Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
"SportTestsDB");
        static string dbPath = System.IO.Path.Combine(dataFolder, "db.accdb");
        string connStr = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";


        public тест(string category, string level)
        {
            InitializeComponent();

            this.currentUser = ((App)Application.Current).CurrentUser;

            DataContext = this;

            Category = category;
            Level = level;

            if (Level == "Сложный")
            {
                _initialTime = 60;
            }
            else if (Level == "Средний")
            {
                _initialTime = 45;
            }
            else
            {
                _initialTime = 30;
            }

            try
            {
                using (var conn = new OleDbConnection(connStr))
                {
                    conn.Open();
                    
                    string sql = @"
                        SELECT COUNT(*) 
                        FROM Тесты 
                        WHERE [сложность] = ? AND [вид_спорта] = ?
                    ";

                    using (var cmd = new OleDbCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("?", Level);
                        cmd.Parameters.AddWithValue("?", Category);

                        int count = (int)cmd.ExecuteScalar();

                        if (count > 0)
                        {

                            var selectCmd = new OleDbCommand(
    "SELECT [идентификатор] FROM Тесты  WHERE [сложность] = ? AND [вид_спорта] = ?",
    conn);
                            selectCmd.Parameters.AddWithValue("?", Level);
                            selectCmd.Parameters.AddWithValue("?", Category);


                            using (var reader = selectCmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    TestId = reader.GetInt32(0);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Тест не найден");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при подключении к БД:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            try
            {
                // Путь к файлу Assets/<Category>_bg.png
                var relativePath = $"/Assets/{Category}_bg.png";
                // создаём URI ресурса, упакованного в приложение
                var uri = new Uri($"pack://application:,,,{relativePath}", UriKind.Absolute);
                var img = new BitmapImage(uri);
                this.Background = new ImageBrush(img)
                {
                    Stretch = Stretch.UniformToFill,
                    Opacity = 1.0
                };
            }
            catch
            {
                MessageBox.Show("Картинка фона не найдена");
            }

            LoadQuestionsFromDb(TestId);

            // 3) Готовим таймер
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += Timer_Tick;

            // 4) Запускаем первый вопрос
            _currentIndex = 0;
            ShowCurrentQuestion();

        }

        private void LoadQuestionsFromDb(int testId)
        {
            using (var conn = new OleDbConnection(connStr))
            {
                conn.Open();
                
                // 2.1. Читаем вопросы
                string qSql = "SELECT идентификатор, содержание, вес FROM вопросы WHERE идентификатор_теста = ? ORDER BY идентификатор";
                try
                {
                    using (var qCmd = new OleDbCommand(qSql, conn))
                    {
                        qCmd.Parameters.AddWithValue("?", testId);

                        using (var qReader = qCmd.ExecuteReader())
                        {
                            while (qReader.Read())
                            {
                                var q = new вопрос
                                {
                                    идентификатор = qReader.GetInt32(0),
                                    содержание = qReader.GetString(1),
                                    вес = qReader.GetInt32(2)
                                };

                                // 2.2. Читаем ответы для этого вопроса
                                string aSql = "SELECT идентификатор, содержание, правильный, комментарий FROM Ответы WHERE идентификатор_вопроса = ?";
                                using (var aCmd = new OleDbCommand(aSql, conn))
                                {
                                    aCmd.Parameters.AddWithValue("?", q.идентификатор);

                                    using (var aReader = aCmd.ExecuteReader())
                                    {
                                        while (aReader.Read())
                                        {
                                            string comment = aReader.IsDBNull(3)
                                                ? string.Empty                    
                                                : aReader.GetString(3);
                                            q.ответы.Add(new ответ
                                            {
                                                идентификатор = aReader.GetInt32(0),
                                                содержание = aReader.GetString(1),
                                                правильный = aReader.GetBoolean(2),
                                                комментарий = comment
                                            });
                                        }
                                    }
                                }

                                _questions.Add(q);
                            }
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при подключении к БД:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ShowCurrentQuestion()
        {
            if (_currentIndex >= _questions.Count)
            {
                FinishTest();
                return;
            }

            CurrentQuestion = _questions[_currentIndex];

            QuestionNumberTextBlock.Text = $"Вопрос {_currentIndex + 1} из {_questions.Count}";
            // Сброс выделения
            foreach (var ans in CurrentQuestion.ответы) ans.IsSelected = false;

            // Запускаем таймер
            TimeRemaining = _initialTime;
            _timer.Start();

            FeedbackTextBlock.Visibility = Visibility.Collapsed;
            AnswersListBox.IsEnabled = true;
            _answered = false;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeRemaining--;
            if (TimeRemaining <= 0)
            {
                _timer.Stop();
                SubmitAnswer();    // автоматический пропуск (считается неверным)
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_answered)
                       {
                           // Если ещё не проверили — проверяем
                SubmitAnswer();
                       }
                else
                       {
                           // Иначе — переходим к следующему
                _currentIndex++;
                ShowCurrentQuestion();
                AnswersListBox.IsHitTestVisible = true;
                AnswersListBox.Focusable = true;
                ActionButton.Content = "Ответить";
            }
        }

        private void SubmitAnswer()
        {
            _timer.Stop();

                   // Находим выбранный и правильный ответы
            var selected = CurrentQuestion.ответы.FirstOrDefault(a => a.IsSelected);
            var correct = CurrentQuestion.ответы.FirstOrDefault(a => a.правильный);

            bool isCorrect = selected != null && selected.правильный;
            if (isCorrect) Score += CurrentQuestion.вес;

            // Выводим сообщение
            FeedbackTextBlock.Text = isCorrect
                       ? "Правильно!"
                       : $"Неправильно. Правильный ответ: {correct?.комментарий}";
            FeedbackTextBlock.Visibility = Visibility.Visible;

                   // Пометим, что ответ дан — дальше ждём нажатия «Далее»
            _answered = true;
            AnswersListBox.IsHitTestVisible = false;
            AnswersListBox.Focusable = false;

            ActionButton.Content = "Далее";
        }

        private void FinishTest()
        {
            // сумма всех весов вопросов
            int totalWeight = _questions.Sum(q => q.вес);
             // процент от максимума
            int totalScore = (int)Math.Round(Score / (double)totalWeight * 100);

            QuestionNumberTextBlock.Visibility = Visibility.Collapsed;
            QuestionTextBlock.Visibility = Visibility.Collapsed;
            AnswersListBox.Visibility = Visibility.Collapsed;
            FeedbackTextBlock.Visibility = Visibility.Collapsed;
            ActionButton.Visibility = Visibility.Collapsed;
            TimerBox.Visibility = Visibility.Collapsed;
            WeightTexBox.Visibility = Visibility.Collapsed;

            ResultPanel.Visibility = Visibility.Visible;

            FinalScoreText.Text =
                $"Тест завершён!\n" +
                $"Набрано баллов: {Score} из {totalWeight}\n" +
                $"Процент: {totalScore}%";

            // Запись в БД
            try
            {
                using (var conn = new OleDbConnection(connStr))
                {
                    conn.Open();

                    string sql = @"INSERT INTO результаты 
                         (идентификатор_пользователя, идентификатор_теста, баллы) 
                         VALUES (?, ?, ?)";

                    using (var cmd = new OleDbCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("?", currentUser.Id);
                        cmd.Parameters.AddWithValue("?", TestId);
                        cmd.Parameters.AddWithValue("?", totalScore);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении результата:\n{ex.Message}",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void OnPropertyChanged(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            главная main = new главная();
            main.Show();
            this.Close();
        }

        private void LeaderboardButton_Click(object sender, RoutedEventArgs e)
        {
            // Создайте окно таблицы лидеров
            таблица_лидеров leaderboard = new таблица_лидеров(TestId);
            leaderboard.Show();
            this.Close();
        }
    } 
}
