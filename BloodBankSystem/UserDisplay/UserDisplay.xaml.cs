using System;
using System.Collections.Generic;
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
using Repository.Models;

namespace BloodBankSystem.UserDisplay
{
    /// <summary>
    /// Interaction logic for UserDisplay.xaml
    /// </summary>
    public partial class UserDisplay : Window
    {
        private User _currentUser;

        public UserDisplay()
        {
            InitializeComponent();
        }

        public void SetUser(User user)
        {
            _currentUser = user;
            UpdateUserInfo();
        }

        private void UpdateUserInfo()
        {
            if (_currentUser != null)
            {
                txtUserInfo.Text = $"Name: {_currentUser.FullName}\n" +
                                 $"Email: {_currentUser.Email}\n" +
                                 $"Role: {_currentUser.Role}\n" +
                                 $"Phone: {_currentUser.Phone ?? "Not provided"}";
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
