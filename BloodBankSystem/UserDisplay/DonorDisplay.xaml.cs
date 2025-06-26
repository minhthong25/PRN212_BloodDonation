using BloodBankSystem.AdminDisplay;
using Repository.Models;
using Services.Interface;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BloodBankSystem.UserDisplay
{
    public partial class DonorDisplay : Window
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IUserService _userService;
        private User _currentUser;
        private Appointment? _upcomingAppointment;
        private Donor? _currentDonor;

        public DonorDisplay()
        {
            InitializeComponent();
            _appointmentService = new AppointmentService();
            _userService = new UserService(); 
        }

        public void SetUser(User user)
        {
            _currentUser = user;
            _currentDonor = user.Donor;
            LoadDonorInfo();
            LoadTestResults();
            LoadAppointments();
            LoadDonationHistory();
            LoadEvents();
        }

        private void LoadEvents()
        {
            var locationService = new LocationService();
            var events = locationService.GetAllLocations();
            eventListBox.ItemsSource = events;
        }

        private void LoadDonorInfo()
        {
            if (_currentUser != null)
            {
                txtFullName.Text = $"Name: {_currentUser.FullName}";
                txtPhone.Text = $"Phone: {_currentUser.Phone ?? "Not updated"}";
                txtCreatedAt.Text = $"Account created: {_currentUser.CreatedAt:dd/MM/yyyy}";
                txtIsActive.Text = $"Status: {(_currentUser.IsActive ? "Active" : "Inactive")}";
                txtBloodGroup.Text = $"Blood group: {_currentUser.Donor?.BloodGroup?.GroupName ?? "Not updated"}";
                txtLastDonation.Text = $"Last donation date: {_currentUser.Donor?.LastDonationDate?.ToString("dd/MM/yyyy") ?? "None"}";
            }
            else
            {
                txtFullName.Text = "Not registered as donor";
                txtPhone.Text = string.Empty;
                txtCreatedAt.Text = string.Empty;
                txtIsActive.Text = string.Empty;
                txtBloodGroup.Text = string.Empty;
                txtLastDonation.Text = string.Empty;
            }
        }

        private void LoadTestResults()
        {
            if (_currentUser?.Donor?.TestResults?.Any() == true)
            {
                var latestTest = _currentUser.Donor.TestResults.OrderByDescending(t => t.TestDate).First();
                txtTestDate.Text = $"Test date: {latestTest.TestDate:dd/MM/yyyy}";
                txtResultNote.Text = $"Test result note: {latestTest.ResultNote}";
            }
            else
            {
                txtTestDate.Text = "No test results";
                txtResultNote.Text = string.Empty;
            }
        }

        private void LoadAppointments()
        {
            _upcomingAppointment = _currentUser?.Donor?.Appointments?
                .Where(a => !a.IsCompleted && a.AppointmentDate > DateTime.Now)
                .OrderBy(a => a.AppointmentDate)
                .FirstOrDefault();

            if (_upcomingAppointment != null)
            {
                txtAppointmentDate.Text = $"Appointment date: {_upcomingAppointment.AppointmentDate:dd/MM/yyyy HH:mm}";
                txtLocation.Text = $"Location: {_upcomingAppointment.Location?.Name}, {_upcomingAppointment.Location?.Address}";
                txtStatus.Text = "Status: Not completed";
            }
            else
            {
                txtAppointmentDate.Text = "No upcoming appointments";
                txtLocation.Text = string.Empty;
                txtStatus.Text = string.Empty;
            }
        }

        private void LoadDonationHistory()
        {
            if (_currentUser?.Donor?.Appointments != null)
            {
                var completed = _currentUser.Donor.Appointments
                    .Where(a => a.IsCompleted)
                    .OrderByDescending(a => a.AppointmentDate)
                    .ToList();

                if (completed.Any())
                {
                    historyListBox.ItemsSource = completed;
                }
                else
                {
                    historyListBox.ItemsSource = new List<string> { "No donation history" };
                }
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
            Application.Current.Shutdown();
        }

        private void RegisterEvent_Click(object sender, RoutedEventArgs e)
        {
            var selectedEvent = eventListBox.SelectedItem as Location;
            if (selectedEvent == null)
            {
                MessageBox.Show("Please select an event to register.");
                return;
            }

            if (_currentUser == null || _currentDonor == null)
            {
                MessageBox.Show("Donor information not found.");
                return;
            }

            var hasUpcoming = _currentDonor.Appointments
                .Any(a => !a.IsCompleted && a.AppointmentDate >= DateTime.Now);

            if (hasUpcoming)
            {
                MessageBox.Show("You already have an unfinished appointment. Cannot register for another.", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (selectedEvent.EventDate == null || selectedEvent.EventEndDate == null)
            {
                MessageBox.Show("Event does not have a valid time.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var timeWindow = new TimeSelectionWindow(selectedEvent.EventDate.Value, selectedEvent.EventEndDate.Value);

            if (timeWindow.ShowDialog() != true || timeWindow.SelectedDateTime == null)
            {
                MessageBox.Show("Please select a valid time.", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime selectedDate = timeWindow.SelectedDateTime.Value;

            if (selectedDate < selectedEvent.EventDate || selectedDate > selectedEvent.EventEndDate)
            {
                MessageBox.Show("Appointment must be within the event time.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var appointment = new Appointment
                {
                    DonorId = _currentDonor.DonorId,
                    LocationId = selectedEvent.LocationId,
                    AppointmentDate = selectedDate,
                    IsCompleted = false
                };

                _appointmentService.AddAppointment(appointment);

                _currentUser = _userService.GetUserWithDonor(_currentUser.UserId);
                _currentDonor = _currentUser.Donor;

                LoadAppointments();
                LoadDonationHistory();
                LoadDonorInfo();

                MessageBox.Show("Blood donation registration successful!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Registration failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (_upcomingAppointment == null)
            {
                MessageBox.Show("No appointment to edit.", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var editWindow = new EditAppointmentWindow(_upcomingAppointment.AppointmentDate);
            if (editWindow.ShowDialog() == true)
            {
                DateTime newDate = editWindow.SelectedDateTime.Value;

                var eventStart = _upcomingAppointment.Location?.EventDate;
                var eventEnd = _upcomingAppointment.Location?.EventEndDate;

                if (eventStart == null || eventEnd == null || newDate < eventStart || newDate > eventEnd)
                {
                    MessageBox.Show($"Appointment date must be within the event time:\n{eventStart:dd/MM/yyyy HH:mm} - {eventEnd:dd/MM/yyyy HH:mm}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
                {
                    _upcomingAppointment.AppointmentDate = newDate;
                    _appointmentService.UpdateAppointment(_upcomingAppointment);

                    MessageBox.Show("Appointment updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    ReloadUser();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating appointment: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DeleteAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (_upcomingAppointment == null)
            {
                MessageBox.Show("No appointment to delete.", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var confirm = MessageBox.Show("Are you sure you want to delete this appointment?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm == MessageBoxResult.Yes)
            {
                try
                {
                    _appointmentService.DeleteAppointment(_upcomingAppointment.AppointmentId);

                    // Reload data
                    ReloadUser();

                    MessageBox.Show("Appointment deleted successfully.", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting appointment: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ReloadUser()
        {
            _currentUser = _userService.GetUserWithDonor(_currentUser.UserId)!;
            _currentDonor = _currentUser.Donor;
            LoadAppointments();
            LoadDonationHistory();
        }
    }
}
