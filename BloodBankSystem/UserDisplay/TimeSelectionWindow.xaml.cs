using System;
using System.Windows;

namespace BloodBankSystem.UserDisplay
{
    public partial class TimeSelectionWindow : Window
    {
        public DateTime? SelectedDateTime { get; private set; }

        public TimeSelectionWindow(DateTime eventStart, DateTime eventEnd)
        {
            InitializeComponent();

            // Giới hạn ngày cho DatePicker theo thời gian sự kiện
            datePicker.DisplayDateStart = eventStart.Date;
            datePicker.DisplayDateEnd = eventEnd.Date;

            // Tạo danh sách giờ và phút
            for (int h = 0; h <= 24; h++) hourBox.Items.Add(h.ToString("D2"));
            for (int m = 0; m <= 60; m += 1) minuteBox.Items.Add(m.ToString("D2"));

            // Mặc định là giờ bắt đầu sự kiện
            datePicker.SelectedDate = eventStart.Date;
            hourBox.SelectedItem = eventStart.Hour.ToString("D2");
            minuteBox.SelectedItem = eventStart.Minute.ToString("D2");
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (datePicker.SelectedDate == null || hourBox.SelectedItem == null || minuteBox.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn đầy đủ ngày và giờ.");
                return;
            }

            var date = datePicker.SelectedDate.Value;
            int hour = int.Parse(hourBox.SelectedItem.ToString());
            int minute = int.Parse(minuteBox.SelectedItem.ToString());

            SelectedDateTime = new DateTime(date.Year, date.Month, date.Day, hour, minute, 0);
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
