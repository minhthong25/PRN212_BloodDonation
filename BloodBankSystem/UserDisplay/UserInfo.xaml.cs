using System;
using System.Windows;
using Repository.Models;
using Services.Services;
using Services.Interface;
using System.Xml.Linq;

namespace BloodBankSystem.UserDisplay
{
    /// <summary>
    /// Interaction logic for UserInfo.xaml
    /// </summary>
    public partial class UserInfo : Window
    {
        private User _currentUser;
        private readonly IUserService _userService;
        private bool _isEditing = false;

        public UserInfo()
        {
            InitializeComponent();
            _userService = new UserService();
        }

        public void SetUser(User user)
        {
            _currentUser = user;
            UpdateUserInfo();
            SetEditMode(false);
        }

        private void UpdateUserInfo()
        {
            if (_currentUser != null)
            {
                txtPassword.Password = _currentUser.Password;
                txtConfirmPassword.Password = _currentUser.Password;
                txtFullName.Text = _currentUser.FullName;
                txtEmail.Text = _currentUser.Email;
                txtPhone.Text = _currentUser.Phone ?? "Chưa cập nhật";
                txtRole.Text = _currentUser.Role;
                txtCreatedAt.Text = _currentUser.CreatedAt.ToString("dd/MM/yyyy");
                txtStatus.Text = _currentUser.IsActive ? "Hoạt động" : "Không hoạt động";
            }
            // Validate password match
            if (txtPassword.Password != txtConfirmPassword.Password)
            {
                MessageBox.Show("Passwords do not match!", "Change Profile Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validate name format
            if (!_userService.ValidName(txtFullName.Text))
            {
                MessageBox.Show("Name can only contain letters, spaces, and Vietnamese characters!", "Change Profile Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validate phone number format
            if (!_userService.ValidPhoneNumber(txtPhone.Text))
            {
                MessageBox.Show("Invalid phone number format! Phone number must be 10-11 digits.", "Change Profile Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Check for duplicate phone number
            if (_userService.IsPhoneNumberExists(txtPhone.Text))
            {
                MessageBox.Show("This phone number is already registered!", "Change Profile Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }

        private void SetEditMode(bool isEditing)
        {
            _isEditing = isEditing;
            txtFullName.IsReadOnly = !isEditing;
            txtEmail.IsReadOnly = !isEditing;
            txtPhone.IsReadOnly = !isEditing;

            btnEdit.Visibility = isEditing ? Visibility.Collapsed : Visibility.Visible;
            btnSave.Visibility = isEditing ? Visibility.Visible : Visibility.Collapsed;
            btnCancel.Visibility = isEditing ? Visibility.Visible : Visibility.Collapsed;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            SetEditMode(true);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            UpdateUserInfo(); // Reset to original values
            SetEditMode(false);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text) || 
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Họ tên và email không được để trống!", 
                              "Lỗi", 
                              MessageBoxButton.OK, 
                              MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Update user object with new values
                _currentUser.FullName = txtFullName.Text.Trim();
                _currentUser.Email = txtEmail.Text.Trim();
                _currentUser.Phone = string.IsNullOrWhiteSpace(txtPhone.Text) ? null : txtPhone.Text.Trim();

                _userService.UpdateUser(_currentUser);
                MessageBox.Show("Cập nhật thông tin thành công!", 
                              "Thành công", 
                              MessageBoxButton.OK, 
                              MessageBoxImage.Information);
                
                SetEditMode(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật thông tin: {ex.Message}", 
                              "Lỗi", 
                              MessageBoxButton.OK, 
                              MessageBoxImage.Error);
            }
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            var userDisplay = new UserDisplay();
            userDisplay.SetUser(_currentUser);
            userDisplay.Show();
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
