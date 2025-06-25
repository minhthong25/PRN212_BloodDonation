using System.Windows;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using Services.Interface;
using Services.Services;

namespace BloodBankSystem.UserDisplay
{
    /// <summary>
    /// Interaction logic for RecipientDisplay.xaml
    /// </summary>
    public partial class RecipientDisplay : Window
    {
        private User _currentUser;
        private readonly RecipientsService _service;
        private readonly BloodRequestService _bloodRequestService;
        private readonly BloodDonationDbContext _context;
        private int? _editingRequestId = null;

        public RecipientDisplay()
        {
            InitializeComponent();
            _context = new BloodDonationDbContext();
            _service = new RecipientsService(_context);
            _bloodRequestService = new BloodRequestService(_context);
            //LoadRecipients();
            LoadBloodRequests();
            LoadBloodGroups();
        }

        private void LoadBloodGroups()
        {
            cbBloodGroup.ItemsSource = _context.BloodGroups.ToList();
            cbBloodGroup.DisplayMemberPath = "GroupName";
            cbBloodGroup.SelectedValuePath = "BloodGroupId";
        }

        public void SetUser(User user)
        {
            _currentUser = user;

            var recipient = _context.Recipients.FirstOrDefault(r => r.RecipientId == _currentUser.UserId);
            if (recipient == null)
            {
                recipient = new Recipient
                {
                    RecipientId = _currentUser.UserId,
                    MedicalCondition = null
                };
                _context.Recipients.Add(recipient);
                _context.SaveChanges();
            }

            LoadBloodRequests();
        }

        //private void LoadRecipients()
        //{
        //    // Lấy danh sách Recipient từ service và gán cho DataGrid
        //    dgRecipients.ItemsSource = _service.GetAllRecipients();
        //}

        private void LoadBloodRequests()
        {
            if (_currentUser == null)
            {
                dgBloodRequests.ItemsSource = null;
                return;
            }

            var recipient = _context.Recipients.FirstOrDefault(r => r.RecipientId == _currentUser.UserId);
            if (recipient == null)
            {
                dgBloodRequests.ItemsSource = null;
                return;
            }

            var requests = _context.BloodRequests
                .Include(br => br.BloodGroup)
                .Include(br => br.Recipient)
                    .ThenInclude(r => r.RecipientNavigation)
                .Where(br => br.RecipientId == recipient.RecipientId)
                .ToList();

            dgBloodRequests.ItemsSource = requests;
        }


        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            var userDisplay = new UserDisplay();
            userDisplay.SetUser(_currentUser);
            userDisplay.Show();
            this.Close();
        }

        private void BtnRegisterBloodRequest_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser == null)
            {
                MessageBox.Show("Không xác định được người dùng hiện tại!");
                return;
            }

            if (cbBloodGroup.SelectedValue == null || string.IsNullOrWhiteSpace(tbQuantity.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            if (!int.TryParse(tbQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Số lượng phải là số nguyên dương!");
                return;
            }

            // Lấy recipient hiện tại theo user đăng nhập (1-1 mapping)
            var recipient = _context.Recipients.FirstOrDefault(r => r.RecipientId == _currentUser.UserId);
            if (recipient == null)
            {
                MessageBox.Show("Không tìm thấy thông tin người nhận!");
                return;
            }

            if (_editingRequestId.HasValue)
            {
                // Sửa
                var request = _context.BloodRequests.FirstOrDefault(r => r.RequestId == _editingRequestId.Value);
                if (request != null)
                {
                    request.BloodGroupId = (int)cbBloodGroup.SelectedValue;
                    request.Quantity = int.Parse(tbQuantity.Text);
                    request.Reason = tbReason.Text;
                    _context.SaveChanges();
                    MessageBox.Show("Cập nhật đăng ký thành công!");
                }
                _editingRequestId = null;
            }
            else
            {


                var request = new BloodRequest
                {
                    RecipientId = recipient.RecipientId,
                    BloodGroupId = (int)cbBloodGroup.SelectedValue,
                    Quantity = quantity,
                    Status = "Pending",
                    RequestDate = DateTime.Now,
                    RequestType = "Nhận máu",
                    Reason = tbReason.Text
                };

                _bloodRequestService.Add(request);
                MessageBox.Show("Đăng ký nhận máu thành công!");
            }
            tbQuantity.Text = "";
            tbReason.Text = "";
            LoadBloodRequests();
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadBloodRequests();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgBloodRequests.SelectedItem as BloodRequest;
            if (selected == null)
            {
                MessageBox.Show("Vui lòng chọn một đăng ký để xóa!");
                return;
            }

            var result = MessageBox.Show("Bạn có chắc muốn xóa đăng ký này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _bloodRequestService.Delete(selected.RequestId);
                LoadBloodRequests();
                MessageBox.Show("Xóa thành công!");
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgBloodRequests.SelectedItem as BloodRequest;
            if (selected == null)
            {
                MessageBox.Show("Vui lòng chọn một đăng ký để sửa!");
                return;
            }

            // Ví dụ: sửa ngay trên form nhập
            tbQuantity.Text = selected.Quantity.ToString();
            tbReason.Text = selected.Reason;
            cbBloodGroup.SelectedValue = selected.BloodGroupId;

            // Khi bấm nút Đăng ký nhận máu, kiểm tra nếu là sửa thì update thay vì add mới
            // Bạn có thể thêm biến trạng thái để biết đang sửa hay thêm mới
            _editingRequestId = selected.RequestId;
        }

    }
}
