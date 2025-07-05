using Repository.Models;
using Services.Interface;
using Services.Services;
using System;
using System.Linq;
using System.Windows;

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

            try
            {
                if (_request.Recipient == null || _request.Recipient.RecipientNavigation == null || _request.BloodGroup == null)
                {
                    var full = _service.GetAllRequestsWithUser()
                                       .FirstOrDefault(r => r.RequestId == _request.RequestId);
                    if (full != null)
                    {
                        _request.Recipient = full.Recipient;
                        _request.BloodGroup = full.BloodGroup;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải đầy đủ thông tin người đăng ký.\n" + ex.Message);
            }

            this.DataContext = this;
        }

        public string FullName =>
            _request.Recipient?.RecipientNavigation?.FullName ?? "[Không có dữ liệu]";

        public string BloodGroup =>
            _request.BloodGroup?.GroupName ?? "[Không có dữ liệu]";

        public string RequestDateString =>
            _request.RequestDate.ToString("yyyy-MM-dd");

        public string DisplayStatus =>
            string.IsNullOrEmpty(_request.Status) || _request.Status == "Pending"
            ? "Đang chờ"
            : _request.Status;

        public int RequestId => _request.RequestId;

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