using CommunalServicesApp.ADO;
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

namespace CommunalServicesApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordTextBox.Text;

            var user = AppDataClass.db.Users.FirstOrDefault(u => u.Login == login);

            if (user == null)
            {
                MessageBox.Show("Ошибка! \nЛогин неверный", "МУП \"Юго-Запад\"", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (user.IsBlocked == true)
                {
                    MessageBox.Show("Ошибка! \nПользователь заблокирован", "МУП \"Юго-Запад\"", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (user.Password == password)
                    {
                        MessageBox.Show("Успешно!", "МУП \"Юго-Запад\"", MessageBoxButton.OK, MessageBoxImage.Information);
                        NavigationService.Navigate(new ReportsPage());
                    }
                    else
                    {
                        user.FailedAttempts++;
                        AppDataClass.db.SaveChanges();

                        if (user.FailedAttempts >= 3)
                        {
                            user.IsBlocked = true;
                            AppDataClass.db.SaveChanges();
                            MessageBox.Show($"Ошибка! \nПопытки закончились. Вы заблокированы", "МУП \"Юго-Запад\"", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            MessageBox.Show($"Ошибка! \nПароль неверный\nОсталось попыток: {3 - user.FailedAttempts}", "МУП \"Юго-Запад\"", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }
    }
}
