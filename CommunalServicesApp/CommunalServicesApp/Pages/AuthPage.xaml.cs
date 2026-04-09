using CommunalServicesApp.ADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
        string titleMessage = "МУП \"Юго-Запад\"";

        public AuthPage()
        {
            InitializeComponent();
        }

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string login = LoginTextBox.Text.Trim();
                string password = PasswordTextBox.Password.Trim();

                if (string.IsNullOrWhiteSpace(login))
                {
                    MessageBox.Show("Введите логин",
                        titleMessage,
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }
                else if (login.Length < 4)
                {
                    MessageBox.Show("Логин должен состоять минимум из 4-х символов",
                        titleMessage,
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Введите пароль",
                        titleMessage,
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }
                else if (password.Length < 4)
                {
                    MessageBox.Show("Пароль должен состоять минимум из 4-х символов",
                        titleMessage,
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                var user = AppDataClass.db.Users.FirstOrDefault(u => u.Login == login);

                if (user == null)
                {
                    MessageBox.Show("Данный пользователь не найден",
                        titleMessage,
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
                else
                {
                    if (user.IsBlocked == true)
                    {
                        MessageBox.Show("Вы заблокированы \nОбратитесь к администратору",
                            titleMessage,
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                    else
                    {
                        string passwordHash = PasswordHash(password);

                        if (passwordHash == user.Password)
                        {
                            user.FailedAttempts = 0;
                            AppDataClass.db.SaveChanges();

                            MessageBox.Show("Успешный вход \nДобро пожаловать",
                                titleMessage,
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);

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

                                MessageBox.Show("Вы ввели неверный пароль\nПопыток больше не осталось\nВы заблокированы",
                                    titleMessage,
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                            }
                            else
                            {
                                MessageBox.Show($"Вы ввели неверный пароль\nОсталось попыток: {3 - user.FailedAttempts}",
                                    titleMessage,
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                            }      
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}",
                    titleMessage,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private string PasswordHash(string password)
        {
            var bytes = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}
