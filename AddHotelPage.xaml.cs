using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using Microsoft.Win32;
using System.IO;
using System.Collections.Generic;

namespace BookingWPF
{
    public partial class AddHotelPage : Page
    {
        private string _selectedPhotoPath;
        private readonly AdminPanelPage _adminPanel;

        public AddHotelPage(AdminPanelPage adminPanel)
        {
            InitializeComponent();
            _adminPanel = adminPanel;
        }

        private void AddHotel_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            string city = CityTextBox.Text;
            string description = DescriptionTextBox.Text;

            if (!int.TryParse((RatingComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(), out int rating))
            {
                MessageBox.Show("Выберите рейтинг отеля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(BasePriceTextBox.Text, out decimal basePrice))
            {
                MessageBox.Show("Введите корректную базовую цену", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(city))
            {
                MessageBox.Show("Заполните обязательные поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Использование БД
                using (SqlConnection connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Добавляем отель
                            SqlCommand cmd = new SqlCommand(@"
                                INSERT INTO Hotels (Name, City, Rating, Description, BasePrice, IsPopular, PhotoPath)
                                VALUES (@Name, @City, @Rating, @Description, @BasePrice, @IsPopular, @PhotoPath);
                                SELECT SCOPE_IDENTITY();", connection, transaction);

                            cmd.Parameters.AddRange(new SqlParameter[] {
                                new SqlParameter("@Name", name),
                                new SqlParameter("@City", city),
                                new SqlParameter("@Rating", rating),
                                new SqlParameter("@Description", description ?? ""),
                                new SqlParameter("@BasePrice", basePrice),
                                new SqlParameter("@IsPopular", false),
                                new SqlParameter("@PhotoPath", string.IsNullOrEmpty(_selectedPhotoPath) ? 
                                    (object)DBNull.Value : _selectedPhotoPath)
                            });

                            // Получаем ID добавленного отеля
                            int hotelId = Convert.ToInt32(cmd.ExecuteScalar());

                            transaction.Commit();
                            MessageBox.Show("Отель успешно добавлен!", "Успех", 
                                MessageBoxButton.OK, MessageBoxImage.Information);
                            _adminPanel.RefreshData();
                            NavigationService.GoBack();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Ошибка при добавлении отеля: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении отеля: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void SelectPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Изображения|*.jpg;*.jpeg;*.png;*.gif|Все файлы|*.*",
                Title = "Выберите фото отеля",
                InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Photos")
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string selectedFile = openFileDialog.FileName;
                    
                    // Проверяем, что файл находится в папке Photos
                    if (!selectedFile.StartsWith(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Photos")))
                    {
                        MessageBox.Show("Пожалуйста, выберите фото из папки Photos", "Предупреждение", 
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Сохраняем относительный путь (Photos/имя_файла.расширение)
                    _selectedPhotoPath = "Photos/" + Path.GetFileName(selectedFile);
                    PhotoPathTextBox.Text = selectedFile;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при выборе файла: {ex.Message}", "Ошибка", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}