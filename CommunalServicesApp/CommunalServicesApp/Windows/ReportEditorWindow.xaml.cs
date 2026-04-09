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
using System.Windows.Shapes;

namespace CommunalServicesApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для ReportEditorWindow.xaml
    /// </summary>
    public partial class ReportEditorWindow : Window
    {
        string titleMessage = "МУП \"Юго-Запад\"";

        int _selectedReportId = 0;

        public ReportEditorWindow(int selectedReportId)
        {
            InitializeComponent();

            LoadServiceTypes();
            LoadClients();
            LoadStatuses();
            LoadWorkers();
            LoadPriorities();

            if (selectedReportId != 0)
            {
                _selectedReportId = selectedReportId;

                TitleTextBlock.Text = "Редактирование";

                AddButton.Visibility = Visibility.Collapsed;
                EditButton.Visibility = Visibility.Visible;

                var report = AppDataClass.db.Reports.FirstOrDefault(r => r.ReportId == selectedReportId);

                ServiceTypesComboBox.SelectedValue = report.ServiceTypeId;
                TitleTextBox.Text = report.Title;
                NoteTextBox.Text = report.Note;
                ClientsComboBox.SelectedValue = report.ClientId;
                StatusesComboBox.SelectedValue = report.StatusId;
                WorkersComboBox.SelectedValue = report.WorkerId;
                PrioritiesComboBox.SelectedValue = report.PriorityId;
            }
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ServiceTypesComboBox.SelectedValue == null
                    || ClientsComboBox.SelectedValue == null
                    || StatusesComboBox.SelectedValue == null
                    || WorkersComboBox.SelectedValue == null
                    || PrioritiesComboBox.SelectedValue == null
                    || string.IsNullOrWhiteSpace(TitleTextBox.Text)
                    || string.IsNullOrWhiteSpace(NoteTextBox.Text))
                {
                    MessageBox.Show("Заполните все необходимые поля",
                        titleMessage,
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                Reports report = new Reports();

                report.ServiceTypeId = (int)ServiceTypesComboBox.SelectedValue;
                report.Title = TitleTextBox.Text;
                report.Note = NoteTextBox.Text;
                report.ClientId = (int)ClientsComboBox.SelectedValue;
                report.Datetime = DateTime.Now;
                report.StatusId = (int)StatusesComboBox.SelectedValue;
                report.WorkerId = (int)WorkersComboBox.SelectedValue;
                report.PriorityId = (int)PrioritiesComboBox.SelectedValue;
                report.IsActual = true;

                AppDataClass.db.Reports.Add(report);
                AppDataClass.db.SaveChanges();

                MessageBox.Show("Данные успешно добавлены",
                    titleMessage,
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}",
                    titleMessage,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void EditButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ServiceTypesComboBox.SelectedValue == null
                    || ClientsComboBox.SelectedValue == null
                    || StatusesComboBox.SelectedValue == null
                    || WorkersComboBox.SelectedValue == null
                    || PrioritiesComboBox.SelectedValue == null
                    || string.IsNullOrWhiteSpace(TitleTextBox.Text)
                    || string.IsNullOrWhiteSpace(NoteTextBox.Text))
                {
                    MessageBox.Show("Заполните все необходимые поля",
                        titleMessage,
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                var selectedReport = AppDataClass.db.Reports.FirstOrDefault(r => r.ReportId == _selectedReportId);

                selectedReport.ServiceTypeId = (int)ServiceTypesComboBox.SelectedValue;
                selectedReport.Title = TitleTextBox.Text;
                selectedReport.Note = NoteTextBox.Text;
                selectedReport.ClientId = (int)ClientsComboBox.SelectedValue;
                selectedReport.StatusId = (int)StatusesComboBox.SelectedValue;
                selectedReport.WorkerId = (int)WorkersComboBox.SelectedValue;
                selectedReport.PriorityId = (int)PrioritiesComboBox.SelectedValue;
                selectedReport.IsActual = true;

                AppDataClass.db.SaveChanges();

                MessageBox.Show("Данные успешно изменены",
                    titleMessage,
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}",
                    titleMessage,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void LoadServiceTypes()
        {
            try
            {
                ServiceTypesComboBox.ItemsSource = AppDataClass.db.ServiceTypes.ToList();

                ServiceTypesComboBox.DisplayMemberPath = "Name";
                ServiceTypesComboBox.SelectedValuePath = "ServiceTypeId";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}",
                    titleMessage,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void LoadClients()
        {
            try
            {
                ClientsComboBox.ItemsSource = AppDataClass.db.Clients.ToList();

                ClientsComboBox.DisplayMemberPath = "Surname";
                ClientsComboBox.SelectedValuePath = "ClientId";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}",
                    titleMessage,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        private void LoadStatuses()
        {
            try
            {
                StatusesComboBox.ItemsSource = AppDataClass.db.Statuses.ToList();

                StatusesComboBox.DisplayMemberPath = "Name";
                StatusesComboBox.SelectedValuePath = "StatusId";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}",
                    titleMessage,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void LoadWorkers()
        {
            try
            {
                WorkersComboBox.ItemsSource = AppDataClass.db.Workers.ToList();

                WorkersComboBox.DisplayMemberPath = "Surname";
                WorkersComboBox.SelectedValuePath = "WorkerId";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}",
                    titleMessage,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void LoadPriorities()
        {
            try
            {
                PrioritiesComboBox.ItemsSource = AppDataClass.db.Priorities.ToList();

                PrioritiesComboBox.DisplayMemberPath = "Name";
                PrioritiesComboBox.SelectedValuePath = "PriorityId";
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
