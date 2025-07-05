using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Repository.Models;
using Services.Services;

namespace BloodBankSystem.AdminDisplay
{
    /// <summary>
    /// Interaction logic for BloodEvent.xaml
    /// </summary>
    public partial class BloodEvent : Window
    {
        private readonly LocationService _locationService;
        private readonly AppointmentService _appointmentService;

        public BloodEvent()
        {
            InitializeComponent();
            _locationService = new LocationService();
            _appointmentService = new AppointmentService();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // Load locations
                var locations = _locationService.GetAllLocations();
                if (locations != null && locations.Any())
                {
                    dgLocations.ItemsSource = locations;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditLocation_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var selectedLocation = button?.DataContext as Location;

            if (selectedLocation == null) return;

            // Clone để tránh chỉnh trực tiếp
            var locationClone = new Location
            {
                LocationId = selectedLocation.LocationId,
                Name = selectedLocation.Name,
                Address = selectedLocation.Address,
                EventDate = selectedLocation.EventDate,
                EventEndDate = selectedLocation.EventEndDate

            };

            var form = new LocationForm(locationClone);
            form.Owner = this;

            if (form.ShowDialog() == true)
            {
                try
                {
                    _locationService.UpdateLocation(form.LocationData);
                    LoadData(); // Refresh
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating location: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void EditAppointment_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var selectedLocation = button?.DataContext as Location;

            if (selectedLocation == null) return;

            // Clone để tránh chỉnh trực tiếp
            var locationClone = new Location
            {
                LocationId = selectedLocation.LocationId,
                Name = selectedLocation.Name,
                Address = selectedLocation.Address
            };

            var form = new LocationForm(locationClone);
            form.Owner = this;

            if (form.ShowDialog() == true)
            {
                try
                {
                    _locationService.UpdateLocation(form.LocationData);
                    LoadData(); // Refresh
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating appointment: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnAddLocation_Click(object sender, RoutedEventArgs e)
        {
            var form = new LocationForm();
            form.Owner = this;

            if (form.ShowDialog() == true)
            {
                try
                {
                    _locationService.AddLocation(form.LocationData);
                    LoadData(); // Refresh DataGrid
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding location: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnAddAppointment_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Open add appointment window
            MessageBox.Show("Add appointment functionality will be implemented soon.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DeleteLocation_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var location = button?.DataContext as Location;

            if (location == null) return;

            var confirm = MessageBox.Show($"Are you sure you want to delete location '{location.Name}'?",
                "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirm == MessageBoxResult.Yes)
            {
                try
                {
                    _locationService.DeleteLocation(location.LocationId);
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting location: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DeleteAppointment_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var appointment = button?.DataContext as Appointment;

            if (appointment == null) return;

            var confirm = MessageBox.Show(
                $"Are you sure you want to delete the appointment with donor '{appointment.Donor}'?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirm == MessageBoxResult.Yes)
            {
                try
                {
                    _appointmentService.DeleteAppointment(appointment.AppointmentId);
                    LoadData(); // refresh DataGrid
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting appointment: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnReturnAdmin_Click(object sender, RoutedEventArgs e)
        {
            AdminDisplay adminDisplay = new AdminDisplay();
            adminDisplay.Show();
            this.Close();
        }
    }
}