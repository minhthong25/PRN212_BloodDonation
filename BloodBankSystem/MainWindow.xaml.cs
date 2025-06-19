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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BloodBankSystem.UserDisplay;
using Services.Services;
using Repository.Models;
using Services.Interface;

namespace BloodBankSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IUserService _userService;
        private readonly ILocationService _locationService;
        private readonly IAppointmentService _appointmentService;

        public MainWindow()
        {
            InitializeComponent();
            _userService = new UserService();
            _locationService = new LocationService();
            _appointmentService = new AppointmentService();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var email = txtEmail.Text;
            var password = txtPassword.Password;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both email and password!", "Login Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            User? user = _userService.checkLogin(email, password);
            if (user != null)
            {
                if(user.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        var adminDisplay = new AdminDisplay.AdminDisplay();
                        adminDisplay.Show();
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error opening Admin Display: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    var userDisplay = new UserDisplay.UserDisplay();
                    userDisplay.SetUser(user);
                    userDisplay.Show();
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Invalid email or password!", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                ResetFields();
            }
        }

        public void ResetFields()
        {
            txtEmail.Text = string.Empty;
            txtPassword.Password = string.Empty;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            var registerPage = new RegisterPage();
            registerPage.Show();
            this.Hide();
        }
    }
}