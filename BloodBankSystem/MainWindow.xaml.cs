using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BloodBankSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            
            var email = txtEmail.Text;
            var password = txtPassword.Password;

           
          
           
            {
                MessageBox.Show("Wrong email or password!!");
            }
        }

        public void ResetFields()
        {
            txtEmail.Text = string.Empty;
            txtPassword.Password = string.Empty; // Assuming txtPassword is a PasswordBox
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
           
            this.Hide();
        }
    }
}