using Repository.Models;
using System.Globalization;
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

            if (location != null)
            {
                txtName.Text = location.Name;
                txtAddress.Text = location.Address;
                txtEventStartDay.Text = location.EventDate?.ToString("yyyy-MM-dd HH:mm") ?? string.Empty;
                txtEventEndDay.Text = location.EventEndDate?.ToString("yyyy-MM-dd HH:mm") ?? string.Empty;
                LocationData.LocationId = location.LocationId;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Name and Address cannot be empty.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var format = "yyyy-MM-dd HH:mm";
            var culture = CultureInfo.InvariantCulture;

            if (!DateTime.TryParseExact(txtEventStartDay.Text.Trim(), format, culture, DateTimeStyles.None, out DateTime startDate))
            {
                MessageBox.Show("Invalid Start Date format. Use yyyy-MM-dd HH:mm (e.g. 2025-07-05 08:30).", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!DateTime.TryParseExact(txtEventEndDay.Text.Trim(), format, culture, DateTimeStyles.None, out DateTime endDate))
            {
                MessageBox.Show("Invalid End Date format. Use yyyy-MM-dd HH:mm (e.g. 2025-07-06 09:00).", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // So sánh hợp lệ
            if (startDate > endDate)
            {
                MessageBox.Show("Start date must be before or equal to end date.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            LocationData.Name = txtName.Text.Trim();
            LocationData.Address = txtAddress.Text.Trim();
            LocationData.EventDate = startDate;
            LocationData.EventEndDate = endDate;

            DialogResult = true;
            Close();
        }
    }
}
