using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Спорт_тесты
{
    /// <summary>
    /// Логика взаимодействия для главная.xaml
    /// </summary>
    public partial class главная : Window
    {
        private readonly Пользователь currentUser;

        public главная()
        {
            InitializeComponent();

            this.currentUser = ((App)Application.Current).CurrentUser;
        }

        private void Tile_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.Tag is string testName)
            {
                RouteToTest(testName);
            }
        }

        private void RouteToTest(string testName)
        {
            выбор_сложности wind = new выбор_сложности(testName);

            wind.Show();

            this.Close();
        }
    }
}
