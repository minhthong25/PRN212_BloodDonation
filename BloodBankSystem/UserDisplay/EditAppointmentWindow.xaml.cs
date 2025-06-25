using System;
using System.Windows;
using System.Windows.Controls;

namespace BloodBankSystem.UserDisplay // 🔥 Phải TRÙNG chính xác với x:Class trong .xaml
{
    public partial class EditAppointmentWindow : Window
    {
        public DateTime? SelectedDateTime { get; private set; }

        public EditAppointmentWindow(DateTime currentAppointmentTime)
        {
            InitializeComponent();

            datePicker.SelectedDate = currentAppointmentTime.Date;

            for (int h = 0; h <= 24; h++) hourBox.Items.Add(h.ToString("D2"));
            for (int m = 0; m <= 60; m += 1) minuteBox.Items.Add(m.ToString("D2"));

            hourBox.SelectedItem = currentAppointmentTime.Hour.ToString("D2");
            minuteBox.SelectedItem = currentAppointmentTime.Minute.ToString("D2");
        }

        private void Save_Click(object sender, RoutedEventArgs e)
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
