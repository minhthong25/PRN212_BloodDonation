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

namespace BloodBankSystem.AdminDisplay
{
    /// <summary>
    /// Interaction logic for AdminDisplay.xaml
    /// </summary>
    public partial class AdminDisplay : Window
    {
        public AdminDisplay()
        {
            InitializeComponent();
        }

        private void btnBloodEvent_Click(object sender, RoutedEventArgs e)
        {
            BloodEvent bloodEvent = new BloodEvent();
            bloodEvent.Show();
            this.Close();
        }

        private void btnBloodInfo_Click(object sender, RoutedEventArgs e)
        {
            BloodInformations bloodInfo = new BloodInformations();
            bloodInfo.Show();
            this.Close();
        }

        private void btnUserInfo_Click(object sender, RoutedEventArgs e)
        {
            UserInformation userInfo = new UserInformation();
            userInfo.Show();
            this.Close();
        }

        private void btnUserRequest_Click(object sender, RoutedEventArgs e)
        {
            UserRequest userRequest = new UserRequest();
            userRequest.Show();
            this.Close();
        }
    }
}
