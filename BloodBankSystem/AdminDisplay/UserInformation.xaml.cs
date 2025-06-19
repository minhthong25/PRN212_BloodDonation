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
using Services.Interface;

namespace BloodBankSystem.AdminDisplay
{
    /// <summary>
    /// Interaction logic for UserInformation.xaml
    /// </summary>
    public partial class UserInformation : Window
    {
        private readonly IUserService _userService;
        private User _selectedUser;
        private bool _isEditing = false;

        public UserInformation()
        {
            InitializeComponent();
            _userService = new UserService();
            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                var users = _userService.GetAll();
                if (users != null && users.Any())
                {
                    dgUsers.ItemsSource = users;
                }
                else
                {
                    MessageBox.Show("No user data available.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading user data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            _selectedUser = button.DataContext as User;
            
            if (_selectedUser == null) return;

            var editWindow = new EditUserInformation(_selectedUser);
            editWindow.Owner = this;
            
            if (editWindow.ShowDialog() == true && editWindow.IsUpdated)
            {
                try
                {
                    _userService.UpdateUser(editWindow.UpdatedUser);
                    MessageBox.Show("User information updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadUsers(); // Refresh the grid
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating user information: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SetEditMode(bool isEditing)
        {
            _isEditing = isEditing;
            btnSave.Visibility = isEditing ? Visibility.Visible : Visibility.Collapsed;
            btnCancel.Visibility = isEditing ? Visibility.Visible : Visibility.Collapsed;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedUser == null) return;

            try
            {
                _userService.UpdateUser(_selectedUser);
                MessageBox.Show("User information updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadUsers(); // Refresh the grid
                SetEditMode(false);
                dgUsers.IsReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating user information: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            LoadUsers(); // Reset to original values
            SetEditMode(false);
            dgUsers.IsReadOnly = true;
        }
    }
}
