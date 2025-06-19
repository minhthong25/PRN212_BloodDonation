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

namespace BloodBankSystem
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Window
    {
        private readonly IUserService _userService;
        private readonly IDonorService _donorService;
        private readonly IRecipientService _recipientService;
        private readonly IBloodGroupService _bloodGroupService;

        public RegisterPage()
        {
            InitializeComponent();
            _userService = new UserService();
            _donorService = new DonorService();
            _recipientService = new RecipientService();
            _bloodGroupService = new BloodGroupService();
            Loaded += RegisterPage_Loaded;
        }

        private void RegisterPage_Loaded(object sender, RoutedEventArgs e)
        {
            cbBloodGroup.ItemsSource = _bloodGroupService.GetAllBloodGroups();
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

            // Validate name format
            if (!_userService.ValidName(txtName.Text))
            {
                MessageBox.Show("Name can only contain letters, spaces, and Vietnamese characters!", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validate phone number format
            if (!_userService.ValidPhoneNumber(txtPhoneNumber.Text))
            {
                MessageBox.Show("Invalid phone number format! Phone number must be 10-11 digits.", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Check for duplicate phone number
            if (_userService.IsPhoneNumberExists(txtPhoneNumber.Text))
            {
                MessageBox.Show("This phone number is already registered!", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                var registeredUser = _userService.Register(newUser);
                if (registeredUser == null)
                {
                    MessageBox.Show("Email already exists!", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Create Donor record
                int selectedBloodGroupId = (int)cbBloodGroup.SelectedValue;
                var donor = new Donor
                {
                    DonorId = registeredUser.UserId,
                    BloodGroupId = selectedBloodGroupId
                };
                _donorService.CreateNewDonor(donor);

                // Create Recipient record
                var recipient = new Recipient
                {
                    MedicalCondition = null,
                    RecipientNavigation = registeredUser
                };
                _recipientService.AddRecipient(recipient);

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
    }
}
