using System;
using System.Windows;

namespace BloodBankSystem.UserDisplay
{
    public partial class TimeSelectionWindow : Window
    {
        public DateTime SelectedTime { get; private set; }

        public TimeSelectionWindow(DateTime start, DateTime end)
        {
            InitializeComponent();

            if (end <= start)
            {
                MessageBox.Show("Không có khung giờ hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }

            for (DateTime time = start; time < end; time = time.AddMinutes(30))
            {
                comboTimeSlots.Items.Add(time);
            }

            if (comboTimeSlots.Items.Count > 0)
                comboTimeSlots.SelectedIndex = 0;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (comboTimeSlots.SelectedItem != null)
            {
                SelectedTime = (DateTime)comboTimeSlots.SelectedItem;
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn thời gian.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }

}
