using AdminWPF.ViewModel;
using BusinessLogic.Concrete;
using DAL.Concrete;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace AdminWPF
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public string connectionString;
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = new LoginWindowViewModel();
            IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("C:\\Users\\Роксоляна\\source\\repos\\Administrator\\AdminWPF\\config.json")
            .Build();

            connectionString = configuration.GetConnectionString("Admin") ?? "";
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = TxtEmail.Text;
            string password = TxtPassword.Password;


            UserDAL userDal = new UserDAL(connectionString);
            UserManager userManager = new UserManager(userDal);


            var user = userManager.FindUserByEmailAndPassword(email, password);

            if (user != null)
            {
                if (user.Is_admin)
                {
                    MessageBox.Show("Login successful as Admin!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    AdminMenuWindow adminMenuWindow = new AdminMenuWindow();
                    adminMenuWindow.Show();
                }
                else
                {
                    MessageBox.Show("Login successful as User!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    UserMenuWindow userMenuWindow = new UserMenuWindow();
                    userMenuWindow.Show();
                }

                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid email or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
