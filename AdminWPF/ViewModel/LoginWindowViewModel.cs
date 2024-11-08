using AdminWPF.Utilities;
using BusinessLogic.Concrete;
using DAL.Concrete;
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
    public class LoginWindowViewModel : INotifyPropertyChanged
    {
       

        public event PropertyChangedEventHandler PropertyChanged;

        public LoginWindowViewModel()
        {
            OpenSignupCommand = new RelayCommand(OpenSignup);
        }

        public ICommand OpenSignupCommand { get; }

       

        private void OpenSignup()
        {
            SignupWindow signupWindow = new SignupWindow();
            signupWindow.Show();

            
            Application.Current.Windows[0]?.Close();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
