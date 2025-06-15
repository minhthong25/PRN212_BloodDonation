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

                // Load appointments
                var appointments = _appointmentService.GetAllAppointments();
                if (appointments != null && appointments.Any())
                {
                    dgAppointments.ItemsSource = appointments;
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
            var location = button.DataContext as Location;
            
            if (location == null) return;

            // TODO: Open edit location window
            MessageBox.Show("Edit location functionality will be implemented soon.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EditAppointment_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var appointment = button.DataContext as Appointment;
            
            if (appointment == null) return;

            // TODO: Open edit appointment window
            MessageBox.Show("Edit appointment functionality will be implemented soon.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnAddLocation_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Open add location window
            MessageBox.Show("Add location functionality will be implemented soon.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnAddAppointment_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Open add appointment window
            MessageBox.Show("Add appointment functionality will be implemented soon.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
