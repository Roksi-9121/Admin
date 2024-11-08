using AdminWPF.ViewModel;
using DAL.Concrete;
using DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace AdminWPF.View
{
    /// <summary>
    /// Interaction logic for UserControlView.xaml
    /// </summary>
    public partial class UserControlView : UserControl
    {
        private readonly UserDAL _userDal;
        public string connectionString;

        public UserControlView()
        {
            InitializeComponent();
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("C:\\Users\\Роксоляна\\source\\repos\\Administrator\\AdminWPF\\config.json")
                .Build();

            connectionString = configuration.GetConnectionString("Admin") ?? "";
            _userDal = new UserDAL(connectionString);
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            // Перевіряємо, чи є вибраний користувач для видалення
            if (UsersDataGrid.SelectedItem is User selectedUser)
            {
                // Видаляємо користувача через DAL
                bool isDeleted = _userDal.RemoveUser(selectedUser.User_name);

                if (isDeleted)
                {
                    // Видаляємо користувача з колекції Users у ViewModel
                    (DataContext as UserControlViewModel)?.Users.Remove(selectedUser);

                    // Повідомляємо користувача про успішне видалення
                    MessageBox.Show("Користувач успішно видалений.", "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Повідомлення про помилку
                    MessageBox.Show("Не вдалося видалити користувача.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Будь ласка, виберіть користувача для видалення.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
