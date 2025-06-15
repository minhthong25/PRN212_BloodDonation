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

namespace BloodBankSystem.AdminDisplay
{
    /// <summary>
    /// Interaction logic for BloodInformations.xaml
    /// </summary>
    public partial class BloodInformations : Window
    {
        private readonly BloodInventoryService _bloodInventoryService;
        private BloodInventory _selectedInventory;
        private bool _isEditing = false;

        public BloodInformations()
        {
            InitializeComponent();
            _bloodInventoryService = new BloodInventoryService();
            LoadBloodInventory();
        }

        private void LoadBloodInventory()
        {
            try
            {
                var bloodInventory = _bloodInventoryService.GetAllBloodInventory();
                if (bloodInventory != null && bloodInventory.Any())
                {
                    dgBloodInventory.ItemsSource = bloodInventory;
                }
                else
                {
                    MessageBox.Show("No blood inventory data available.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading blood inventory: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            _selectedInventory = button.DataContext as BloodInventory;
            
            if (_selectedInventory == null) return;

            var editWindow = new EditBloodInventory(_selectedInventory);
            editWindow.Owner = this;
            
            if (editWindow.ShowDialog() == true && editWindow.IsUpdated)
            {
                _selectedInventory.Quantity = editWindow.NewQuantity;
                SetEditMode(true);
            }
        }

        private void SetEditMode(bool isEditing)
        {
            _isEditing = isEditing;
            btnSave.Visibility = isEditing ? Visibility.Visible : Visibility.Collapsed;
            btnCancel.Visibility = isEditing ? Visibility.Visible : Visibility.Collapsed;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedInventory == null) return;

            try
            {
                var result = _bloodInventoryService.UpdateBloodInventory(_selectedInventory.BloodGroupId, _selectedInventory.Quantity);
                if (result != null)
                {
                    MessageBox.Show("Quantity updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadBloodInventory(); // Refresh the grid
                    SetEditMode(false);
                }
                else
                {
                    MessageBox.Show("Failed to update quantity.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating quantity: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            LoadBloodInventory(); // Reset to original values
            SetEditMode(false);
        }
    }
}
