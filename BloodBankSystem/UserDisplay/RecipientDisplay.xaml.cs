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

namespace BloodBankSystem.UserDisplay
{
    /// <summary>
    /// Interaction logic for RecipientDisplay.xaml
    /// </summary>
    public partial class RecipientDisplay : Window
    {
        private User _currentUser;

        public RecipientDisplay()
        {
            InitializeComponent();
        }

        public void SetUser(User user)
        {
            _currentUser = user;
            // Add code to display recipient information
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            var userDisplay = new UserDisplay();
            userDisplay.SetUser(_currentUser);
            userDisplay.Show();
            this.Close();
        }
    }
}
