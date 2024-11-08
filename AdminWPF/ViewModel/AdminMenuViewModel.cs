using AdminWPF.Utilities;
using AdminWPF.View;
using DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AdminWPF.ViewModel
{
    public class AdminMenuViewModel : INotifyPropertyChanged
    {
        
        private bool _isUserControlActive;
        public bool IsUserControlActive
        {
            get => _isUserControlActive;
            set
            {
                _isUserControlActive = value;
                OnPropertyChanged(nameof(IsUserControlActive));
                UpdateSelectedControl();
            }
        }

        private bool _isAddNewUserActive;
        public bool isAddNewUserActive
        {
            get => _isAddNewUserActive;
            set
            {
                _isAddNewUserActive = value;
                OnPropertyChanged(nameof(isAddNewUserActive));
                UpdateSelectedControl();
            }
        }

        private bool _isSettingActive;
        public bool IsSettingActive
        {
            get => _isSettingActive;
            set
            {
                _isSettingActive = value;
                OnPropertyChanged(nameof(IsSettingActive));
                UpdateSelectedControl();
            }
        }

        private bool _isSearchUserActive;
        public bool isSearchUserActive
        {
            get => _isSearchUserActive;
            set
            {
                _isSearchUserActive = value;
                OnPropertyChanged(nameof(isSearchUserActive));
                UpdateSelectedControl();
            }
        }


        private UserControl _currentControl;
        public UserControl CurrentControl
        {
            get => _currentControl;
            set
            {
                _currentControl = value;
                OnPropertyChanged(nameof(CurrentControl));
            }
        }



        
        public ICommand ShowUserControlCommand { get; }
        public ICommand ShowSearchUserCommand { get; }
        public ICommand ShowAddNewUserCommand { get; }
        public ICommand ShowSettingCommand { get; }
        public ICommand LogoutCommand { get; }
        

        public AdminMenuViewModel()
        {
            ShowUserControlCommand = new RelayCommand(() => IsUserControlActive = true);
            ShowSearchUserCommand = new RelayCommand(() => isSearchUserActive = true);
            ShowAddNewUserCommand = new RelayCommand(() => isAddNewUserActive = true);
            ShowSettingCommand = new RelayCommand(() => IsSettingActive = true);
            LogoutCommand = new RelayCommand(ExecuteLogout);

        }


        private void UpdateSelectedControl()
        {
            if (IsUserControlActive)
                CurrentControl = new UserControlView(); 
            else if (isAddNewUserActive)
                CurrentControl = new AddNewUserView(); 
            else if (IsSettingActive)
                CurrentControl = new SettingView(); 
            else if(isSearchUserActive)
                CurrentControl = new SearchUser();
        }

        private void ExecuteLogout()
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();

            Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is AdminMenuWindow)?.Close();

            
        }

       

        

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }    
}
