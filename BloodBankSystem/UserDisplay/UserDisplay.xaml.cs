using System.Windows;
using Repository.Models;
using Services.Services;
using Services.Interface;

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



        private void btnRegisterDonor_Click(object sender, RoutedEventArgs e)
        {
            var userService = new UserService();
            var fullUser = userService.GetUserWithDonor(_currentUser.UserId); 
            var donorDisplay = new DonorDisplay();
            donorDisplay.SetUser(fullUser);
            donorDisplay.Show();
            this.Close();
        }
        private void btnRegisterRecipient_Click(object sender, RoutedEventArgs e)
        {
            var recipientDisplay = new RecipientDisplay();
            recipientDisplay.Show();
            this.Close();
        }
        private void btnViewUserInfo_Click(object sender, RoutedEventArgs e)
        {
            var userInfo = new UserInfo();
            userInfo.SetUser(_currentUser);
            userInfo.Show();
            this.Close();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?", 
                                       "Confirm Logout", 
                                       MessageBoxButton.YesNo, 
                                       MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }
        private void Window_Closed(object sender, System.EventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}
