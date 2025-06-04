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
using Services.Services;
using Repository.Models;

namespace BloodBankSystem
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Window
    {
        private readonly UserService _userService;
        public RegisterPage()
        {
            InitializeComponent();
            _userService = new UserService();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Validate input fields
            if (string.IsNullOrEmpty(txtEmail.Text) || 
                string.IsNullOrEmpty(txtPassword.Password) || 
                string.IsNullOrEmpty(txtConfirmPassword.Password) ||
                string.IsNullOrEmpty(txtName.Text) ||
                string.IsNullOrEmpty(txtPhoneNumber.Text))
            {
                MessageBox.Show("Please fill in all fields!", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validate password match
            if (txtPassword.Password != txtConfirmPassword.Password)
            {
                MessageBox.Show("Passwords do not match!", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

                // Create new user
                var newUser = new User
                {
                    Email = txtEmail.Text,
                    Password = txtPassword.Password,
                    FullName = txtName.Text,
                    Phone = txtPhoneNumber.Text,
                    Role = "User" // Default role
                };

            try
            {
                _userService.Add(newUser);
                MessageBox.Show("Registration successful! Please login.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                
                // Return to login page
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Registration failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnReturn_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void ResetFields()
        {
            txtEmail.Text = string.Empty;
            txtPassword.Password = string.Empty;
            txtConfirmPassword.Password = string.Empty;
            txtName.Text = string.Empty;
            txtPhoneNumber.Text = string.Empty;
        }
    }
}
