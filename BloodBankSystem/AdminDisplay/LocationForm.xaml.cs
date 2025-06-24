using Repository.Models;
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
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Name and Address cannot be empty.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            LocationData.Name = txtName.Text.Trim();
            LocationData.Address = txtAddress.Text.Trim();

            DialogResult = true;
            Close();
        }
    }
}
