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

    public partial class RequestDetailWindow : Window
    {
        private readonly BloodRequest _request;
        private readonly IBloodRequestService _service;

        public RequestDetailWindow(BloodRequest request)
        {
            InitializeComponent();
            _request = request;
            _service = new BloodRequestService();
            this.DataContext = _request;
        }

        private void Approve_Click(object sender, RoutedEventArgs e)
        {
            _request.Status = "Đã xác nhận";
            _service.UpdateRequestStatus(_request);
            this.Close();
        }

        private void Reject_Click(object sender, RoutedEventArgs e)
        {
            _request.Status = "Từ chối";
            _service.UpdateRequestStatus(_request);
            this.Close();
        }
    }
}