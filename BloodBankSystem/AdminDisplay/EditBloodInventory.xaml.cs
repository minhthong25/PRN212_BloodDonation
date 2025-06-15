using System;
using System.Windows;
using Repository.Models;

namespace BloodBankSystem.AdminDisplay
{
    /// <summary>
    /// Interaction logic for EditBloodInventory.xaml
    /// </summary>
    public partial class EditBloodInventory : Window
    {
        private readonly BloodInventory _bloodInventory;
        public bool IsUpdated { get; private set; }
        public int NewQuantity { get; private set; }

        public EditBloodInventory(BloodInventory bloodInventory)
        {
            InitializeComponent();
            _bloodInventory = bloodInventory;
            LoadData();
        }

        private void LoadData()
        {
            txtBloodGroup.Text = $"Blood Group: {_bloodInventory.BloodGroup.GroupName}";
            txtQuantity.Text = _bloodInventory.Quantity.ToString();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtQuantity.Text, out int newQuantity))
            {
                if (newQuantity < 0)
                {
                    MessageBox.Show("Quantity cannot be negative.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                NewQuantity = newQuantity;
                IsUpdated = true;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Please enter a valid number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
