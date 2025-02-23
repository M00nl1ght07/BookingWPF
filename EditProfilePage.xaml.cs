using System;

using System.Windows;

using System.Windows.Controls;

using System.Data.SqlClient;
using System.Collections.Generic;



namespace BookingWPF

{

    public partial class EditProfilePage : Page

    {

        private User _currentUser;



        public EditProfilePage(User user)

        {

            InitializeComponent();

            _currentUser = user;

            LoadUserData();

        }



        private void LoadUserData()

        {

            NameTextBox.Text = _currentUser.Name;

            EmailTextBox.Text = _currentUser.Email;

        }



        private void SaveChanges_Click(object sender, RoutedEventArgs e)

        {

            string newName = NameTextBox.Text;

            string newEmail = EmailTextBox.Text;

            string newPassword = PasswordBox.Password;



            if (string.IsNullOrWhiteSpace(newName) || string.IsNullOrWhiteSpace(newEmail))

            {

                MessageBox.Show("Заполните обязательные поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;

            }



            try

            {

                string query = "UPDATE Users SET Name = @Name, Email = @Email";

                var parameters = new List<SqlParameter>

                {

                    new SqlParameter("@UserID", _currentUser.UserID),

                    new SqlParameter("@Name", newName),

                    new SqlParameter("@Email", newEmail)

                };



                if (!string.IsNullOrWhiteSpace(newPassword))

                {

                    query += ", Password = @Password";

                    parameters.Add(new SqlParameter("@Password", newPassword));

                }



                query += " WHERE UserID = @UserID";



                DatabaseConnection.ExecuteNonQuery(query, parameters.ToArray());



                MessageBox.Show("Изменения сохранены успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                NavigationService.Navigate(new ProfilePage(_currentUser));

            }

            catch (Exception ex)

            {

                MessageBox.Show($"Ошибка при сохранении изменений: {ex.Message}", "Ошибка",

                    MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }



        private void Cancel_Click(object sender, RoutedEventArgs e)

        {

            NavigationService.Navigate(new ProfilePage(_currentUser));

        }

    }

}
