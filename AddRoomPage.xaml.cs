using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using Microsoft.Win32;
using System.IO;

namespace BookingWPF
{
    public partial class AddRoomPage : Page
    {
        private readonly int _hotelId;
        private string _selectedPhotoPath;

        public AddRoomPage(int hotelId)
        {
            InitializeComponent();
            _hotelId = hotelId;
        }

        private void AddRoom_Click(object sender, RoutedEventArgs e)
        {
            string roomType = (RoomTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string description = DescriptionTextBox.Text;

            if (string.IsNullOrWhiteSpace(roomType))
            {
                MessageBox.Show("Выберите тип номера", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(AreaTextBox.Text, out decimal area))
            {
                MessageBox.Show("Введите корректную площадь", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(CapacityTextBox.Text, out int capacity))
            {
                MessageBox.Show("Введите корректную вместимость", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(PriceTextBox.Text, out decimal price))
            {
                MessageBox.Show("Введите корректную цену", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@HotelID", _hotelId),
                    new SqlParameter("@RoomType", roomType),
                    new SqlParameter("@Area", area),
                    new SqlParameter("@Capacity", capacity),
                    new SqlParameter("@PricePerNight", price),
                    new SqlParameter("@Description", description ?? ""),
                    new SqlParameter("@PhotoPath", string.IsNullOrEmpty(_selectedPhotoPath) ? (object)DBNull.Value : _selectedPhotoPath)
                };

                DatabaseConnection.ExecuteNonQuery(
                    "INSERT INTO Rooms (HotelID, RoomType, Area, Capacity, PricePerNight, Description, PhotoPath) " +
                    "VALUES (@HotelID, @RoomType, @Area, @Capacity, @PricePerNight, @Description, @PhotoPath)", parameters);

                MessageBox.Show("Номер успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении номера: {ex.Message}", "Ошибка",
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
                Title = "Выберите фото номера",
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