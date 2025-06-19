using BloodBankSystem.AdminDisplay;
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

namespace BloodBankSystem.UserDisplay
{
    /// <summary>
    /// Interaction logic for DonorDisplay.xaml
    /// </summary>
    public partial class DonorDisplay : Window
    {
        private readonly IAppointmentService _appointmentService;
        private User _currentUser;

        public DonorDisplay()
        {
            InitializeComponent();
            _appointmentService = new AppointmentService();
        }

        public void SetUser(User user)
        {
            _currentUser = user;
            LoadDonorInfo();
            LoadTestResults();
            LoadAppointments();
            LoadDonationHistory();
            LoadEvents();
        }

        private void LoadEvents()
        {
            var locationService = new LocationService();
            var events = locationService.GetAllLocations();
            eventListBox.ItemsSource = events;
        }

        private void LoadDonorInfo()
        {
            if (_currentUser?.Donor != null)
            {
                txtFullName.Text = $"Họ tên: {_currentUser.FullName}";
                txtPhone.Text = $"Số điện thoại: {_currentUser.Phone ?? "Chưa cập nhật"}";
                txtCreatedAt.Text = $"Ngày tạo tài khoản: {_currentUser.CreatedAt:dd/MM/yyyy}";
                txtIsActive.Text = $"Trạng thái: {(_currentUser.IsActive ? "Hoạt động" : "Không hoạt động")}";
                txtBloodGroup.Text = $"Nhóm máu: {_currentUser.Donor.BloodGroup?.GroupName ?? "Chưa cập nhật"}";
                txtLastDonation.Text = $"Ngày hiến máu gần nhất: {_currentUser.Donor.LastDonationDate?.ToString("dd/MM/yyyy") ?? "Chưa có"}";
            }
            else
            {
                txtFullName.Text = "Chưa đăng ký hiến máu";
                txtPhone.Text = string.Empty;
                txtCreatedAt.Text = string.Empty;
                txtIsActive.Text = string.Empty;
                txtBloodGroup.Text = string.Empty;
                txtLastDonation.Text = string.Empty;
            }
        }

        private void LoadTestResults()
        {
            if (_currentUser?.Donor?.TestResults != null && _currentUser.Donor.TestResults.Any())
            {
                var latestTest = _currentUser.Donor.TestResults.OrderByDescending(t => t.TestDate).First();
                txtTestDate.Text = $"Ngày xét nghiệm: {latestTest.TestDate:dd/MM/yyyy}";
                txtResultNote.Text = $"Ghi chú kết quả: {latestTest.ResultNote}";
            }
            else
            {
                txtTestDate.Text = "Chưa có kết quả xét nghiệm";
                txtResultNote.Text = string.Empty;
            }
        }

        private void LoadAppointments()
        {
            if (_currentUser?.Donor?.Appointments != null && _currentUser.Donor.Appointments.Any())
            {
                var upcomingAppointment = _currentUser.Donor.Appointments
                    .Where(a => !a.IsCompleted && a.AppointmentDate > DateTime.Now)
                    .OrderBy(a => a.AppointmentDate)
                    .FirstOrDefault();

                if (upcomingAppointment != null)
                {
                    txtAppointmentDate.Text = $"Ngày hẹn: {upcomingAppointment.AppointmentDate:dd/MM/yyyy}";
                    txtLocation.Text = $"Địa điểm: {upcomingAppointment.Location?.Name}, {upcomingAppointment.Location?.Address}";
                    txtStatus.Text = $"Trạng thái: {(upcomingAppointment.IsCompleted ? "Đã hoàn thành" : "Chưa hoàn thành")}";
                }
                else
                {
                    txtAppointmentDate.Text = "Không có lịch hẹn sắp tới";
                    txtLocation.Text = string.Empty;
                    txtStatus.Text = string.Empty;
                }
            }
            else
            {
                txtAppointmentDate.Text = "Không có lịch hẹn";
                txtLocation.Text = string.Empty;
                txtStatus.Text = string.Empty;
            }
        }

        private void LoadDonationHistory()
        {
            if (_currentUser?.Donor?.Appointments != null)
            {
                var historyAppointments = _currentUser.Donor.Appointments
                    .Where(a => a.IsCompleted)
                    .OrderByDescending(a => a.AppointmentDate)
                    .ToList();

                if (historyAppointments.Any())
                {
                    var lastHistory = historyAppointments.First(); // Hiển thị lịch sử gần nhất
                    txtAppointmentDate1.Text = $"Ngày hiến: {lastHistory.AppointmentDate:dd/MM/yyyy}";
                    txtLocation1.Text = $"Địa điểm: {lastHistory.Location?.Name}, {lastHistory.Location?.Address}";
                    txtStatus1.Text = "Trạng thái: Đã hoàn thành";
                }
                else
                {
                    txtAppointmentDate1.Text = "Không có lịch sử hiến máu";
                    txtLocation1.Text = "";
                    txtStatus1.Text = "";
                }
            }
        }


        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            var userDisplay = new UserDisplay();
            userDisplay.SetUser(_currentUser);
            userDisplay.Show();
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void eventListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void RegisterEvent_Click(object sender, RoutedEventArgs e)
        {
            var selectedEvent = eventListBox.SelectedItem as Location;
            if (selectedEvent == null)
            {
                MessageBox.Show("Vui lòng chọn một sự kiện để đăng ký.");
                return;
            }

            if (_currentUser?.Donor == null)
            {
                MessageBox.Show("Không tìm thấy thông tin người hiến máu.");
                return;
            }

            try
            {
                var appointment = new Appointment
                {
                    DonorId = _currentUser.Donor.DonorId,
                    LocationId = selectedEvent.LocationId,
                    AppointmentDate = selectedEvent.EventDate ?? DateTime.Now, // fallback nếu null
                    IsCompleted = false
                };

                _appointmentService.AddAppointment(appointment);

                MessageBox.Show("Đăng ký hiến máu thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadAppointments(); // reload phần lịch hẹn
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đăng ký thất bại: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
