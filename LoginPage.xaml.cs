using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data.SqlClient;

namespace BookingWPF
{
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите email и пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@Email", email),
                    new SqlParameter("@Password", password)
                };

                using (SqlDataReader reader = DatabaseConnection.ExecuteReader(
                    "SELECT * FROM Users WHERE Email = @Email AND Password = @Password", parameters))
                {
                    if (reader.Read())
                    {
                        User currentUser = new User
                        {
                            UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            IsAdmin = reader.GetBoolean(reader.GetOrdinal("IsAdmin")),
                            RegistrationDate = reader.GetDateTime(reader.GetOrdinal("RegistrationDate")),
                            BonusPoints = reader.GetInt32(reader.GetOrdinal("BonusPoints"))
                        };

                        var mainWindow = (MainWindow)Application.Current.MainWindow;
                        mainWindow.SetCurrentUser(currentUser);

                        if (currentUser.IsAdmin)
                        {
                            NavigationService.Navigate(new AdminPanelPage(currentUser));
                        }
                        else
                        {
                            NavigationService.Navigate(new ProfilePage(currentUser));
                        }
                    }
                    else
                    {
                        MessageBox.Show("Неверный email или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при входе: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RegisterLink_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new RegisterPage());
        }
    }
}