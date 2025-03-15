using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using Microsoft.Win32;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Globalization;

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

            if (!ValidateCoordinates())
            {
                return;
            }

            try
            {
                using (SqlConnection connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            SqlCommand cmd = new SqlCommand(@"
                                INSERT INTO Hotels (Name, City, Rating, Description, BasePrice, IsPopular, PhotoPath, Latitude, Longitude)
                                VALUES (@Name, @City, @Rating, @Description, @BasePrice, @IsPopular, @PhotoPath, @Latitude, @Longitude);
                                SELECT SCOPE_IDENTITY();", connection, transaction);

                            cmd.Parameters.AddWithValue("@Name", name);
                            cmd.Parameters.AddWithValue("@City", city);
                            cmd.Parameters.AddWithValue("@Rating", rating);
                            cmd.Parameters.AddWithValue("@Description", description);
                            cmd.Parameters.AddWithValue("@BasePrice", basePrice);
                            cmd.Parameters.AddWithValue("@IsPopular", false);
                            cmd.Parameters.AddWithValue("@PhotoPath", _selectedPhotoPath ?? "");
                            cmd.Parameters.AddWithValue("@Latitude", double.Parse(LatitudeTextBox.Text, CultureInfo.InvariantCulture));
                            cmd.Parameters.AddWithValue("@Longitude", double.Parse(LongitudeTextBox.Text, CultureInfo.InvariantCulture));

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

        private void CoordinateTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Разрешаем ввод цифр, точки и минуса
            Regex regex = new Regex(@"^[0-9.-]+$");
            e.Handled = !regex.IsMatch(e.Text);
        }

        private bool ValidateCoordinates()
        {
            if (!double.TryParse(LatitudeTextBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double lat) ||
                !double.TryParse(LongitudeTextBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double lon))
            {
                MessageBox.Show("Координаты должны быть числами");
                return false;
            }

            if (lat < -90 || lat > 90)
            {
                MessageBox.Show("Широта должна быть в диапазоне от -90 до 90");
                return false;
            }

            if (lon < -180 || lon > 180)
            {
                MessageBox.Show("Долгота должна быть в диапазоне от -180 до 180");
                return false;
            }

            return true;
        }
    }
}