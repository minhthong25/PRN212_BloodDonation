using Repository.Models;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;

namespace BloodBankSystem.AdminDisplay
{
    public partial class LocationForm : Window
    {
        public Location LocationData { get; private set; }

        public LocationForm(Location? location = null)
        {
            InitializeComponent();
            LocationData = location ?? new Location();

            for (int i = 0; i < 24; i++)
            {
                cbHour.Items.Add(i.ToString("D2"));
                cbEndHour.Items.Add(i.ToString("D2"));
            }

            for (int i = 0; i < 60; i++)
            {
                cbMinute.Items.Add(i.ToString("D2"));
                cbEndMinute.Items.Add(i.ToString("D2"));
            }

            if (location != null)
            {
                txtName.Text = location.Name;
                txtAddress.Text = location.Address;

                if (location.EventDate.HasValue)
                {
                    dpEventDate.SelectedDate = location.EventDate.Value.Date;
                    cbHour.SelectedItem = location.EventDate.Value.Hour.ToString("D2");
                    cbMinute.SelectedItem = location.EventDate.Value.Minute.ToString("D2");
                }

                if (location.EventEndDate.HasValue)
                {
                    dpEndDate.SelectedDate = location.EventEndDate.Value.Date;
                    cbEndHour.SelectedItem = location.EventEndDate.Value.Hour.ToString("D2");
                    cbEndMinute.SelectedItem = location.EventEndDate.Value.Minute.ToString("D2");
                }

                LocationData.LocationId = location.LocationId;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text.Trim();
            string address = txtAddress.Text.Trim();
            Regex alphanumericRegex = new Regex("^[a-zA-Z0-9 ]+$");

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(address))
            {
                MessageBox.Show("Name and Address cannot be empty.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!alphanumericRegex.IsMatch(name))
            {
                MessageBox.Show("Name can only contain letters, numbers, and spaces. No special characters allowed.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!alphanumericRegex.IsMatch(address))
            {
                MessageBox.Show("Address can only contain letters, numbers, and spaces. No special characters allowed.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dpEventDate.SelectedDate == null || cbHour.SelectedItem == null || cbMinute.SelectedItem == null)
            {
                MessageBox.Show("Please select a complete start date and time.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dpEndDate.SelectedDate == null || cbEndHour.SelectedItem == null || cbEndMinute.SelectedItem == null)
            {
                MessageBox.Show("Please select a complete end date and time.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Parse start date
            DateTime startDate = dpEventDate.SelectedDate.Value;
            int startHour = int.Parse(cbHour.SelectedItem.ToString()!);
            int startMinute = int.Parse(cbMinute.SelectedItem.ToString()!);
            DateTime startDateTime = new DateTime(startDate.Year, startDate.Month, startDate.Day, startHour, startMinute, 0);

            // Parse end date
            DateTime endDate = dpEndDate.SelectedDate.Value;
            int endHour = int.Parse(cbEndHour.SelectedItem.ToString()!);
            int endMinute = int.Parse(cbEndMinute.SelectedItem.ToString()!);
            DateTime endDateTime = new DateTime(endDate.Year, endDate.Month, endDate.Day, endHour, endMinute, 0);

            // Validation
            if (startDateTime > endDateTime)
            {
                MessageBox.Show("Start date/time must be before or equal to end date/time.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Assign to LocationData
            LocationData.Name = txtName.Text.Trim();
            LocationData.Address = txtAddress.Text.Trim();
            LocationData.EventDate = startDateTime;
            LocationData.EventEndDate = endDateTime;

            DialogResult = true;
            Close();
        }
    }
}
