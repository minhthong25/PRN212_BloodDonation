using BloodBankSystem.AdminDisplay;
using Repository.Models;
using Services.Interface;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BloodBankSystem.UserDisplay
{
    public partial class DonorDisplay : Window
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IUserService _userService;
        private User _currentUser;
        private Appointment? _upcomingAppointment;

        public DonorDisplay()
        {
            InitializeComponent();
            _appointmentService = new AppointmentService();
            _userService = new UserService(); 
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
            if (_currentUser?.Donor?.TestResults?.Any() == true)
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
            _upcomingAppointment = _currentUser?.Donor?.Appointments?
                .Where(a => !a.IsCompleted && a.AppointmentDate > DateTime.Now)
                .OrderBy(a => a.AppointmentDate)
                .FirstOrDefault();

            if (_upcomingAppointment != null)
            {
                txtAppointmentDate.Text = $"Ngày hẹn: {_upcomingAppointment.AppointmentDate:dd/MM/yyyy HH:mm}";
                txtLocation.Text = $"Địa điểm: {_upcomingAppointment.Location?.Name}, {_upcomingAppointment.Location?.Address}";
                txtStatus.Text = "Trạng thái: Chưa hoàn thành";
            }
            else
            {
                txtAppointmentDate.Text = "Không có lịch hẹn sắp tới";
                txtLocation.Text = string.Empty;
                txtStatus.Text = string.Empty;
            }
        }

        private void LoadDonationHistory()
        {
            if (_currentUser?.Donor?.Appointments != null)
            {
                var completed = _currentUser.Donor.Appointments
                    .Where(a => a.IsCompleted)
                    .OrderByDescending(a => a.AppointmentDate)
                    .ToList();

                if (completed.Any())
                {
                    historyListBox.ItemsSource = completed;
                }
                else
                {
                    historyListBox.ItemsSource = new List<string> { "Không có lịch sử hiến máu" };
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

            var hasUpcoming = _currentUser.Donor.Appointments
                .Any(a => !a.IsCompleted && a.AppointmentDate >= DateTime.Now);

            if (hasUpcoming)
            {
                MessageBox.Show("Bạn đã có một lịch hẹn chưa hoàn thành. Không thể đăng ký thêm.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Mở cửa sổ chọn ngày giờ
            if (selectedEvent.EventDate == null || selectedEvent.EventEndDate == null)
            {
                MessageBox.Show("Sự kiện không có thời gian hợp lệ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var timeWindow = new TimeSelectionWindow(selectedEvent.EventDate.Value, selectedEvent.EventEndDate.Value);

            if (timeWindow.ShowDialog() != true || timeWindow.SelectedDateTime == null)
            {
                MessageBox.Show("Vui lòng chọn thời gian hợp lệ.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime selectedDate = timeWindow.SelectedDateTime.Value;

            if (selectedDate < selectedEvent.EventDate || selectedDate > selectedEvent.EventEndDate)
            {
                MessageBox.Show("Lịch hẹn phải nằm trong thời gian tổ chức sự kiện.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var appointment = new Appointment
                {
                    DonorId = _currentUser.Donor.DonorId,
                    LocationId = selectedEvent.LocationId,
                    AppointmentDate = selectedDate,
                    IsCompleted = false
                };

                _appointmentService.AddAppointment(appointment);
                MessageBox.Show("Đăng ký hiến máu thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                // Reload lại user sau khi thêm lịch hẹn
                _currentUser = _userService.GetUserWithDonor(_currentUser.UserId);
                LoadAppointments();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đăng ký thất bại: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




        private void EditAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (_upcomingAppointment == null)
            {
                MessageBox.Show("Không có lịch hẹn để sửa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var editWindow = new EditAppointmentWindow(_upcomingAppointment.AppointmentDate);
            if (editWindow.ShowDialog() == true)
            {
                DateTime newDate = editWindow.SelectedDateTime.Value;

                var eventStart = _upcomingAppointment.Location?.EventDate;
                var eventEnd = _upcomingAppointment.Location?.EventEndDate;

                if (eventStart == null || eventEnd == null || newDate < eventStart || newDate > eventEnd)
                {
                    MessageBox.Show($"Ngày hẹn phải nằm trong thời gian tổ chức sự kiện:\n{eventStart:dd/MM/yyyy HH:mm} - {eventEnd:dd/MM/yyyy HH:mm}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
                {
                    _upcomingAppointment.AppointmentDate = newDate;
                    _appointmentService.UpdateAppointment(_upcomingAppointment);

                    MessageBox.Show("Cập nhật lịch hẹn thành công.", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    ReloadUser();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật lịch hẹn: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void DeleteAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (_upcomingAppointment == null)
            {
                MessageBox.Show("Không có lịch hẹn để xóa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var confirm = MessageBox.Show("Bạn có chắc muốn xóa lịch hẹn này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm == MessageBoxResult.Yes)
            {
                try
                {
                    _appointmentService.DeleteAppointment(_upcomingAppointment.AppointmentId);

                    // Reload dữ liệu
                    ReloadUser();

                    MessageBox.Show("Xóa lịch hẹn thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa lịch hẹn: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void ReloadUser()
        {
            _currentUser = _userService.GetUserWithDonor(_currentUser.UserId)!;
            LoadAppointments();
            LoadDonationHistory();
        }
    }
}
