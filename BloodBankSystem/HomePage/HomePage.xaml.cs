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
using Services.Interface;
using Repository.Models;
using Microsoft.Extensions.DependencyInjection;
using Services.Services;

namespace BloodBankSystem
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Window
    {
        private readonly ILocationService _locationService;
        private readonly IAppointmentService _appointmentService;
        private readonly IUserService _userService;

        public HomePage()
        {
            InitializeComponent();
            _locationService = new LocationService();
            _appointmentService = new AppointmentService();
            _userService = new UserService();
            LoadLocations();
        }

        private void LoadLocations()
        {
            try
            {
                var locations = _locationService.GetAllLocations();
                LocationDataGrid.ItemsSource = locations;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading locations: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnOpenMainWindow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening login window: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
