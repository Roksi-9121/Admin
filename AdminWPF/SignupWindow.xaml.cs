using AdminWPF.Utilities;
using AdminWPF.ViewModel;
using BusinessLogic.Concrete;
using DAL;
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
    public partial class SignupWindow : Window
    {
        public string connectionString;
        public SignupWindow()
        {
            InitializeComponent();
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("C:\\Users\\Роксоляна\\source\\repos\\Administrator\\AdminWPF\\config.json")
                .Build();

            connectionString = configuration.GetConnectionString("Admin") ?? "";
        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonSignup_Click(object sender, RoutedEventArgs e)
        {
            string name = TxtName.Text;
            string email = TxtEmail.Text;
            string password = TxtPassword.Password;
            string role = (ComboRole.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (!SignupValidator.AreFieldsFilled(name, email, password, role) ||
                !SignupValidator.IsNameValid(name))
            {
                return;
            }

            string hashedPassword = PasswordHelper.HashPassword(password);

            DTO.User newUser = new DTO.User
            {
                User_name = name,
                Email = email,
                Password = hashedPassword,
                Is_active = true,
                Is_admin = role == "Administrator"
            };


            UserManager userManager = new UserManager(new UserDAL(connectionString));
            var result = userManager.CreateUser(newUser);


            if (result != null)
            {
                MessageBox.Show("Signup successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);


                if (newUser.Is_admin)
                {
                    AdminMenuWindow adminMenuWindow = new AdminMenuWindow();
                    adminMenuWindow.Show();
                }
                else
                {
                    UserMenuWindow userMenuWindow = new UserMenuWindow();
                    userMenuWindow.Show();
                }


                Application.Current.Windows[0]?.Close();
            }
            else
            {
                MessageBox.Show("Signup failed. Please try again.", "Signup Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static bool AreFieldsFilled(string name, string email, string password, string role)
        {
            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(role))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }


        public static bool IsNameValid(string name)
        {
            if (name.Length <= 3)
            {
                MessageBox.Show("Name must be more than 3 characters.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
    }
}
