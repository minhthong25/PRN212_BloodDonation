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
using Repository.Models;

namespace BloodBankSystem.AdminDisplay
{
    /// <summary>
    /// Interaction logic for EditUserInformation.xaml
    /// </summary>
    public partial class EditUserInformation : Window
    {
        private readonly User _user;
        public bool IsUpdated { get; private set; }
        public User UpdatedUser { get; private set; }

        public EditUserInformation(User user)
        {
            InitializeComponent();
            _user = user;
            LoadData();
        }

        private void LoadData()
        {
            txtFullName.Text = _user.FullName;
            txtEmail.Text = _user.Email;
            txtPhone.Text = _user.Phone ?? "";
            cmbRole.SelectedItem = cmbRole.Items.Cast<ComboBoxItem>()
                .FirstOrDefault(item => item.Content.ToString() == _user.Role);
            chkIsActive.IsChecked = _user.IsActive;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text) || 
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Full name and email cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (cmbRole.SelectedItem == null)
            {
                MessageBox.Show("Please select a role.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Update user object with new values
            _user.FullName = txtFullName.Text.Trim();
            _user.Email = txtEmail.Text.Trim();
            _user.Phone = string.IsNullOrWhiteSpace(txtPhone.Text) ? null : txtPhone.Text.Trim();
            _user.Role = (cmbRole.SelectedItem as ComboBoxItem).Content.ToString();
            _user.IsActive = chkIsActive.IsChecked ?? false;

            IsUpdated = true;
            UpdatedUser = _user;
            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
