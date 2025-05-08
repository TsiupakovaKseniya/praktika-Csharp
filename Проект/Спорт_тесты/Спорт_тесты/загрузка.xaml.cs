using System;
using System.Windows;
using System.Windows.Threading;

namespace Спорт_тесты
{
    /// <summary>
    /// Логика взаимодействия для загрузка.xaml
    /// </summary>
    public partial class загрузка : Window
    {
        private readonly DispatcherTimer _timer;
        private int _progress;

        public загрузка()
        {
            InitializeComponent();

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(5)
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_progress >= 100)
            {
                _timer.Stop();
             
                авторизация auth = new авторизация();

                auth.Show();

                this.Close();
            }
            else
            {
                _progress++;
                ProgressBar.Value = _progress;
                ProgressText.Text = $"{_progress}%";
            }
        }
    }
}
