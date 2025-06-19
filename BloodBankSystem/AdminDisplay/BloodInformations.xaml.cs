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
using Services.Interface;

namespace BloodBankSystem.AdminDisplay
{
    public class BloodGroupInventoryViewModel
    {
        public string GroupName { get; set; }
        public int Quantity { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    /// <summary>
    /// Interaction logic for BloodInformations.xaml
    /// </summary>
    public partial class BloodInformations : Window
    {
        private readonly IBloodGroupService _bloodGroupService;
        private readonly IBloodInventoryService _bloodInventoryService;

        public BloodInformations()
        {
            InitializeComponent();
            _bloodGroupService = new BloodGroupService();
            _bloodInventoryService = new BloodInventoryService();
            LoadAllBloodGroupsTable();
        }

        private void LoadAllBloodGroupsTable()
        {
            var groups = _bloodGroupService.GetAllBloodGroups();
            var inventory = _bloodInventoryService.GetAllBloodInventory();
            var list = groups
                .Where(g => g.GroupName != "Unknown")
                .Select(g => {
                    var inv = inventory.FirstOrDefault(i => i.BloodGroupId == g.BloodGroupId);
                    return new BloodGroupInventoryViewModel
                    {
                        GroupName = g.GroupName,
                        Quantity = inv?.Quantity ?? 0,
                        UpdatedAt = inv?.UpdatedAt
                    };
                }).ToList();
            dgAllBloodGroups.ItemsSource = list;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var list = dgAllBloodGroups.ItemsSource as List<BloodGroupInventoryViewModel>;
            if (list == null) return;

            foreach (var item in list)
            {
                if (item.Quantity <= 0)
                {
                    MessageBox.Show($"Quantity for group {item.GroupName} must be a positive number!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            var inventory = _bloodInventoryService.GetAllBloodInventory();
            var groups = _bloodGroupService.GetAllBloodGroups();

            foreach (var item in list)
            {
                var group = groups.FirstOrDefault(g => g.GroupName == item.GroupName);
                if (group == null) continue;
                var inv = inventory.FirstOrDefault(i => i.BloodGroupId == group.BloodGroupId);
                if (inv != null && inv.Quantity != item.Quantity)
                {
                    inv.Quantity = item.Quantity;
                    inv.UpdatedAt = DateTime.Now;
                    _bloodInventoryService.UpdateBloodInventory(inv.BloodGroupId, inv.Quantity);
                }
            }
            MessageBox.Show("Update Success", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadAllBloodGroupsTable();
        }
    }
}
