using AdminWPF.Utilities;
using BusinessLogic.Concrete;
using DAL.Concrete;
using DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AdminWPF.ViewModel
{
    public class UserControlViewModel : INotifyPropertyChanged
    {
        private readonly UserManager _userManager;
        public string ConnectionString { get; private set; }

        public UserControlViewModel()
        {
            LoadConfiguration();
            _userManager = new UserManager(new UserDAL(ConnectionString)); 
            Users = new ObservableCollection<User>();
            LoadUsers();

            FilterByNameCommand = new RelayCommand(FilterByName);


            CloseAllCommand = new RelayCommand(ExecuteExit);
        }

        public ICommand FilterByNameCommand { get; }
        public ICommand CloseAllCommand { get; }

        private void LoadConfiguration()
        {
            try
            {
                IConfiguration configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("C:\\Users\\Роксоляна\\source\\repos\\Administrator\\AdminWPF\\config.json", optional: false, reloadOnChange: true)
                    .Build();

                ConnectionString = configuration.GetConnectionString("Admin") ?? throw new Exception("Connection string not found.");
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error: {ex.Message}");
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


        private void FilterByName()
        {
            var sortedUsers = _userManager.SortUsersByName();
            Users.Clear();
            foreach (var user in sortedUsers)
            {
                Users.Add(user);
            }
        }
        private void LoadUsers()
        {
            var usersFromDb = _userManager.ReadUsers();
            foreach (var user in usersFromDb)
            {
                Users.Add(user);
            }
        }

        public void ExecuteExit()
        {
            Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is AdminMenuWindow)?.Close();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

