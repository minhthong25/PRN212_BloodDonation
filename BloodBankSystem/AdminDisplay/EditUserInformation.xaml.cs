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
using System.Xml.Linq;
using Repository.Models;
using Services.Interface;
using Services.Services;

namespace BloodBankSystem.AdminDisplay
{
    /// <summary>
    /// Interaction logic for EditUserInformation.xaml
    /// </summary>
    public partial class EditUserInformation : Window
    {
        private readonly User _user;
        private readonly IUserService _userService;
        public bool IsUpdated { get; private set; }
        public User UpdatedUser { get; private set; }

        public EditUserInformation(User user)
        {
            InitializeComponent();
            _user = user;
            _userService = new UserService();
            LoadData();
        }

        private void LoadData()
        {
            txtFullName.Text = _user.FullName;
            txtEmail.Text = _user.Email;
            txtPhone.Text = _user.Phone ?? "";
            cmbRole.SelectedItem = cmbRole.Items.Cast<ComboBoxItem>()
                .FirstOrDefault(item => item.Content.ToString() == _user.Role);
            chkIsActive.IsChecked = _user.IsActive;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text) || 
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Full name and email cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (cmbRole.SelectedItem == null)
            {
                MessageBox.Show("Please select a role.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validate name format
            if (!_userService.ValidName(txtFullName.Text))
            {
                MessageBox.Show("Name can only contain letters, spaces, and Vietnamese characters!", "Change Profile Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validate phone number format
            if (!string.IsNullOrWhiteSpace(txtPhone.Text) && !_userService.ValidPhoneNumber(txtPhone.Text))
            {
                MessageBox.Show("Invalid phone number format! Phone number must be 10-11 digits.", "Change Profile Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Check for duplicate phone number (if changed)
            if (!string.IsNullOrWhiteSpace(txtPhone.Text) && txtPhone.Text != _user.Phone && _userService.IsPhoneNumberExists(txtPhone.Text))
            {
                MessageBox.Show("This phone number is already registered!", "Change Profile Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Update user object with new values
            _user.FullName = txtFullName.Text.Trim();
            _user.Email = txtEmail.Text.Trim();
            _user.Phone = string.IsNullOrWhiteSpace(txtPhone.Text) ? null : txtPhone.Text.Trim();
            _user.Role = (cmbRole.SelectedItem as ComboBoxItem).Content.ToString();
            _user.IsActive = chkIsActive.IsChecked ?? false;

            IsUpdated = true;
            UpdatedUser = _user;
            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
