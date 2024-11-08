using AdminWPF.Utilities;
using BusinessLogic.Concrete;
using DAL.Concrete;
using DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace AdminWPF.ViewModel
{
    public class SearchUserViewModel : INotifyPropertyChanged
    {
        private readonly UserManager _userManager;
        public string ConnectionString { get; private set; }
        public ICommand UpdateStatusCommand { get; }


        public SearchUserViewModel()
        {
            LoadConfiguration();
            _userManager = new UserManager(new UserDAL(ConnectionString));
            Users = new ObservableCollection<User>();
            LoadUsers();
            SearchCommand = new RelayCommand(SearchUsers);
            UpdateStatusCommand = new RelayCommand(UpdateUserStatus);
        }

        private void LoadConfiguration()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("C:\\Users\\Роксоляна\\source\\repos\\Administrator\\AdminWPF\\config.json")
                .Build();

            ConnectionString = configuration.GetConnectionString("Admin") ?? "";
        }

        public ICommand SearchCommand { get; }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }

        private void LoadUsers()
        {
            var usersFromDb = _userManager.ReadUsers();
            Users.Clear(); 
            foreach (var user in usersFromDb)
            {
                Users.Add(user);
            }
        }

        private void SearchUsers()
        {
            Users.Clear();

            if (!string.IsNullOrEmpty(SearchText))
            {
                var usersFromDb = _userManager.SearchUsers(SearchText);
                foreach (var user in usersFromDb)
                {
                    Users.Add(user); 
                }
            }
        }

        private void UpdateUserStatus(object parameter)
        {
            if (parameter is User user)
            {
                bool success = _userManager.UpdateUserStatus(user.User_name, user.Is_active);

                if (success)
                {
                    
                    MessageBox.Show("Status updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                   
                    MessageBox.Show("Failed to update status. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
