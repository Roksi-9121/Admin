using AdminWPF.Utilities;
using BusinessLogic.Concrete;
using DAL.Concrete;
using DAL;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace AdminWPF.ViewModel
{
    public class SignupWindowViewModel : INotifyPropertyChanged
    {
        private string _name;
        private string _email;
        private string _password;
        private string _selectedRole;
        public string ConnectionString { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public SignupWindowViewModel()
        {
            LoadConfiguration();
            SignupCommand = new RelayCommand(Signup, CanSignup);
        }

        private void LoadConfiguration()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("C:\\Users\\Роксоляна\\source\\repos\\Administrator\\AdminWPF\\config.json")
                .Build();

            ConnectionString = configuration.GetConnectionString("Admin") ?? "";
        }

        
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                OnPropertyChanged(nameof(SelectedRole));
            }
        }

        
        public ICommand SignupCommand { get; }

        private void Signup()
        {
            if (!SignupValidator.AreFieldsFilled(Name, Email, Password, SelectedRole) ||
                !SignupValidator.IsNameValid(Name))
            {
                return;
            }

            string hashedPassword = PasswordHelper.HashPassword(Password);

            DTO.User newUser = new DTO.User
            {
                User_name = Name,
                Email = Email,
                Password = hashedPassword,
                Is_active = true,
                Is_admin = SelectedRole == "Administrator"
            };

            UserManager userManager = new UserManager(new UserDAL(ConnectionString));
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

        private bool CanSignup()
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(Email) &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   !string.IsNullOrWhiteSpace(SelectedRole);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

