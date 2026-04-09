using CommunalServicesApp.ADO;
using CommunalServicesApp.Windows;
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
    /// Логика взаимодействия для ReportsPage.xaml
    /// </summary>
    public partial class ReportsPage : Page
    {
        string titleMessage = "МУП \"Юго-Запад\"";

        public ReportsPage()
        {
            InitializeComponent();

            LoadData();
        }

        private void LoadData()
        {
            ReportsDataGrid.ItemsSource = AppDataClass.db.Reports.ToList();
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            ReportEditorWindow reportEditorWindow = new ReportEditorWindow(0);
            reportEditorWindow.ShowDialog();

            LoadData();
        }

        private void EditButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ReportsDataGrid.SelectedItem is Reports selectedReport)
                {
                    ReportEditorWindow reportEditorWindow = new ReportEditorWindow(selectedReport.ReportId);
                    reportEditorWindow.ShowDialog();

                    LoadData();
                }
                else
                {
                    MessageBox.Show("Выберите заявку для редактирования",
                        titleMessage,
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
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

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ReportsDataGrid.SelectedItem is Reports selectedReport)
                {
                    if (MessageBox.Show($"Вы уверены, что хотите удалить заявку номер: {selectedReport.ReportId}",
                        titleMessage,
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        AppDataClass.db.Reports.Remove(selectedReport);
                        AppDataClass.db.SaveChanges();

                        MessageBox.Show("Данные успешно удалены",
                            titleMessage,
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);

                        LoadData();
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
    }
}
