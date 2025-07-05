using Repository.Models;
using Services.Interface;
using Services.Services;
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
    /// Interaction logic for UserRequest.xaml
    /// </summary>
    public partial class UserRequest : Window
    {
        private readonly IBloodRequestService _service;
        private List<BloodRequest> _requests;

        public UserRequest()
        {
            InitializeComponent();
            _service = new BloodRequestService();
            LoadRequests();
        }

        private void LoadRequests()
        {
            _requests = _service.GetAllRequestsWithUser();
            DisplaySorted("Ngày gần nhất");
        }

        private void ViewDetail_Click(object sender, RoutedEventArgs e)
        {
            var request = (sender as Button)?.Tag as BloodRequest;
            if (request != null)
            {
                var detailWindow = new RequestDetailWindow(request);
                detailWindow.ShowDialog();
                LoadRequests();
            }
        }
        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = (SortComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
            DisplaySorted(selected);
        }
        private void DisplaySorted(string? sortType)
        {
            if (_requests == null) return;

            if (sortType == "Ngày xa nhất")
            {

                RequestDataGrid.ItemsSource = _requests.OrderBy(r => r.RequestDate).ToList();
            }
            else
            {
                RequestDataGrid.ItemsSource = _requests.OrderByDescending(r => r.RequestDate).ToList();

                RequestDataGrid.ItemsSource = _requests.OrderBy(r => r.RequestDate).ToList();
            }

        }

    }

}